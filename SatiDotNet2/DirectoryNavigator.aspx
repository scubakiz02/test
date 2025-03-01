<%@ Page Title="Directory Navigator" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="DirectoryNavigator.aspx.vb" Inherits="DirectoryNavigator" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="MainUpdatePanel" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" CssClass="ContentAutoScaler">
                <asp:Panel ID="TitlePanel" runat="server" CssClass="ContentAutoScaler" HorizontalAlign="Center" Style="padding-top: 10px">
                    <asp:Label ID="TitleLabel" runat="server" Text="File Directory Navigator" Font-Size="X-Large" Font-Bold="true"></asp:Label>
                </asp:Panel>
                <asp:Panel ID="ContentOrderPanel" runat="server" CssClass="ContentAutoScaler" Style="padding-top: 20px">
                    <table style="width: calc(100% - 10px); border: none; border-spacing: 0px">
                        <tr>
                            <td style="width: 30px; background-color: white; height: 50px;"></td>
                            <td style="width: calc(100% - 87%); min-width: 200px; background-color: #e0dede; padding-left: 20px; height: 50px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="NavBW" runat="server" Height="37px" Text="&#x21e6;" Font-Size="27px" Width="40px" ForeColor="Red" BackColor="#363636" />
                                            <asp:Button ID="NavFW" runat="server" Height="37px" Text="&#x21e8;" Font-Size="27px" Width="40px" ForeColor="#0acc00" BackColor="#363636" />
                                        </td>
                                        <td>&nbsp;&nbsp;
                                            <asp:Label ID="FontSizeLabel" runat="server" Text="Font: 12px" ForeColor="DarkRed"></asp:Label>

                                            <asp:Button ID="ScaleDown" runat="server" Text="-" Width="26px" ForeColor="White" BackColor="#363636" />
                                            <asp:Button ID="ScaleUp" runat="server" Text="+" Width="26px" ForeColor="White" BackColor="#363636" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="background-color: #e0dede; height: 50px;">
                                <table>
                                    <tr>
                                        <td style="height: 27px; padding-left: 20px; padding-right: 20px; width: 100%">
                                            <asp:Label ID="WrittenPathLabel" runat="server" Font-Italic="True" Font-Size="15px">Currently showing </asp:Label>
                                        </td>
                                        <td style="height: 27px;">
                                            <asp:Panel ID="Panel1" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:UpdateProgress ID="UpdateProgressLoading" runat="server" style="width: 40px">
                                                                <ProgressTemplate>
                                                                    &nbsp;<img src="../Color/Animated_LoadingBigger.gif" />
                                                                </ProgressTemplate>
                                                            </asp:UpdateProgress>
                                                        </td>
                                                        <td>
                                                            <asp:Panel ID="ButtonClickerPanel" runat="server" DefaultButton="SearchButton">
                                                                <asp:TextBox ID="SearchKey" runat="server" Font-Italic="true" placeholder="Search Current Directory" Width="200px"></asp:TextBox>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                        <td style="height: 27px; padding-right: 20px">
                                            <asp:Button ID="SearchButton" runat="server" Height="23px" Text="&#9906; Search" Width="65px" ForeColor="White" BackColor="#363636" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 40px; background-color: white; height: 49px;"></td>
                        </tr>
                        <tr>
                            <td style="width: 40px; background-color: white"></td>
                            <td style="width: calc(100% - 75%); min-width: 200px; background-color: #e0dede;">
                                <asp:Panel ID="OptionsPanel" runat="server" Style="min-width: 200px; padding-left: 20px;" HorizontalAlign="left" Height="475px">
                                    <table style="height: 458px; width: 100%">
                                        <tr>
                                            <td style="width: 209px">
                                                <asp:Panel ID="FormFileDetailsPanel" runat="server" Style="min-width: 200px;" ScrollBars="Vertical" Height="145px" BackColor="White">
                                                    <asp:FormView ID="FormFileDetails" runat="server" Font-Size="12px">
                                                        <ItemTemplate>
                                                            <b>File Name  : <%#DataBinder.Eval(Container.DataItem, "FullName")%></b>
                                                            <br />
                                                            Created    : <%#DataBinder.Eval(Container.DataItem, "CreationTime")%>
                                                            <br />
                                                            Last Update:<%#DataBinder.Eval(Container.DataItem, "LastWriteTime")%><br />Last Opened: <%#DataBinder.Eval(Container.DataItem, "LastAccessTime")%>
                                                            <br />
                                                            <i><%#DataBinder.Eval(Container.DataItem, "Attributes")%></i>
                                                            <br />
                                                            <%#DataBinder.Eval(Container.DataItem, "Length")%>bytes.
                                                        </ItemTemplate>
                                                    </asp:FormView>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 40px; width: 209px;">
                                                <asp:RadioButton ID="FormsRB" runat="server" Text="Forms" AutoPostBack="true" Font-Size="22px" Font-Bold="true" Width="190px" Height="30px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 40px; width: 209px;">
                                                <asp:RadioButton ID="ProceduresRB" runat="server" Text="Procedures" AutoPostBack="true" Font-Size="22px" Font-Bold="true" Width="190px" Height="30px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 40px; width: 209px;">
                                                <asp:RadioButton ID="WorkInstructionRB" runat="server" Text="Work Instructions" AutoPostBack="true" Font-Size="21px" Font-Bold="true" Width="190px" Height="30px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 40px; width: 209px;">
                                                <asp:RadioButton ID="MiscLabelsRB" runat="server" Text="Misc Labels" AutoPostBack="True" Font-Size="22px" Font-Bold="true" Width="190px" Height="30px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 40px; width: 209px;">
                                                <asp:RadioButton ID="RecipesRB" runat="server" Text="Recipes" AutoPostBack="True" Font-Size="22px" Font-Bold="true" Width="190px" Height="30px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 40px; width: 209px;">
                                                <asp:RadioButton ID="ProLogsRB" runat="server" Text="Pro Logs" AutoPostBack="true" Font-Size="22px" Font-Bold="true" Width="190px" Height="30px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 40px;">
                                                <asp:Button ID="DownloadButton" runat="server" Text="&#10004; Open" AutoPostBack="true" Font-Size="15px" Font-Bold="true" Width="96%" Height="50px" ForeColor="#0acc00" BackColor="#363636" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                            <td style="background-color: white">
                                <asp:Panel ID="DirectoryTitlePanel" runat="server" Style="width: auto">
                                    <table style="background-color: #CCFFFF; width: 100%">
                                        <tr>
                                            <td style="width: 20px">
                                                <asp:Label ID="ImageLabel" runat="server" Font-Bold="true" Text=" "></asp:Label></td>
                                            <td style="width: 395px">
                                                <asp:Label ID="NameLabel" runat="server" Font-Bold="true" Text="Name"></asp:Label></td>
                                            <td style="width: 95px">
                                                <asp:Label ID="SizeLabel" runat="server" Font-Bold="true" Text="Size"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="LastModLabel" runat="server" Font-Bold="true" Text="Last Modified"></asp:Label></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="FileDirectoryPanel" runat="server" Visible="true" Style="width: auto" Height="450px" BackColor="White" ScrollBars="Vertical">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="GridDirList" runat="server" AutoGenerateColumns="False" GridLines="None" CellPadding="0" CellSpacing="1" DataKeyNames="FullName" Style="width: 100%" Font-Size="12px">
                                                    <HeaderStyle Font-Size="1px" ForeColor="White"></HeaderStyle>
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <img src="../color/folder.jpg" alt="" />

                                                            </ItemTemplate>
                                                            <HeaderStyle Width="20px" />
                                                        </asp:TemplateField>
                                                        <asp:ButtonField DataTextField="Name" CommandName="Select" HeaderText="Name">
                                                            <HeaderStyle Width="400px" />
                                                        </asp:ButtonField>
                                                        <asp:BoundField HeaderText="Size">
                                                            <HeaderStyle Width="100px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="LastWriteTime" HeaderText="Last Modified" />
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:GridView ID="GridFileList" runat="server" AutoGenerateColumns="False" GridLines="None" CellPadding="0" CellSpacing="1" DataKeyNames="FullName" Style="width: 100%" Font-Size="12px">
                                                    <SelectedRowStyle BackColor="#C0FFFF"></SelectedRowStyle>
                                                    <HeaderStyle Font-Size="1px" ForeColor="White"></HeaderStyle>
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderStyle Width="20px"></HeaderStyle>
                                                            <ItemTemplate>
                                                                <img src="../color/file.jpg" alt="" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:ButtonField DataTextField="Name" CommandName="Select" HeaderText="Name">
                                                            <HeaderStyle Width="400px"></HeaderStyle>
                                                        </asp:ButtonField>
                                                        <asp:BoundField DataField="Length" HeaderText="Size">
                                                            <HeaderStyle Width="100px"></HeaderStyle>
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="LastWriteTime" HeaderText="Last Modified"></asp:BoundField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                    </div>
                                </asp:Panel>
                            </td>
                            <td style="width: 40px; background-color: white"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="BottomPanel" runat="server" Height="25px" CssClass="ContentAutoScaler"></asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
