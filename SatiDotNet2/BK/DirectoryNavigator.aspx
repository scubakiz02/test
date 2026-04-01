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
                            <td style="width: 30px; background-color: white"></td>
                            <td style="width: calc(100% - 85%); min-width: 200px; background-color: #e0dede; padding-left: 20px">
                                <asp:UpdateProgress ID="UpdateProgressLoading" runat="server" Style="width: 50%; min-width: 100px;">
                                    <ProgressTemplate>
                                        &nbsp;<img src="../Color/Animated_LoadingBigger.gif" />
                                        Loading...
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <asp:Button ID="ScaleDown" runat="server" Text="Font -" Width="60px" Height="20px" />
                                <asp:Button ID="ScaleUp" runat="server" Text="Font +" Width="60px" Height="20px" />
                                &nbsp;
                                <asp:Label ID="FontSizeLabel" runat="server" Text="Font: 12px" ForeColor="DarkRed"></asp:Label>
                            </td>
                            <td style="background-color: #e0dede">
                                <table>
                                    <tr>
                                        <td style="height: 27px">
                                            <asp:Label ID="SearchFileName" runat="server">Search: </asp:Label>
                                        </td>
                                        <td style="height: 27px">
                                            <asp:Panel ID="ButtonClickerPanel" runat="server" DefaultButton="SearchButton">
                                                <asp:TextBox ID="SearchKey" runat="server" Font-Italic="true" placeholder="Search Current Directory" Width="200px"></asp:TextBox>
                                            </asp:Panel>
                                        </td>
                                        <td style="height: 27px">
                                            <asp:Button ID="SearchButton" runat="server" Width="81px" Text="Find" Height="23px" />
                                        </td>
                                        <td style="height: 27px">
                                            <asp:Label ID="WrittenPathLabel" runat="server" Font-Size="15px" Font-Italic="True">Currently showing </asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 40px; background-color: white"></td>
                        </tr>
                        <tr>
                            <td style="width: 40px; background-color: white"></td>
                            <td style="width: calc(100% - 75%); min-width: 200px; background-color: #e0dede">
                                <asp:Panel ID="OptionsPanel" runat="server" Style="width: 100%; min-width: 200px; padding-left: 20px" HorizontalAlign="left">
                                    <table style="height: 470px; width: 100%">
                                        <tr>
                                            <td>
                                                <asp:Panel ID="FormFileDetailsPanel" runat="server" Style="width: calc(100% - 20px); min-width: 200px" ScrollBars="Vertical" Height="120px">
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
                                            <td style="height: 45px">
                                                <asp:RadioButton ID="FormsRB" runat="server" Text="Forms" AutoPostBack="true" Font-Size="22px" Font-Bold="true" Width="190px" Height="30px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 45px">
                                                <asp:RadioButton ID="ProceduresRB" runat="server" Text="Procedures" AutoPostBack="true" Font-Size="22px" Font-Bold="true" Width="190px" Height="30px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 45px">
                                                <asp:RadioButton ID="WorkInstructionRB" runat="server" Text="Work Instructions" AutoPostBack="true" Font-Size="21px" Font-Bold="true" Width="190px" Height="30px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 45px">
                                                <asp:RadioButton ID="MiscLabelsRB" runat="server" Text="Misc Labels" AutoPostBack="True" Font-Size="22px" Font-Bold="true" Width="190px" Height="30px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 45px">
                                                <asp:RadioButton ID="RecipesRB" runat="server" Text="Recipes" AutoPostBack="True" Font-Size="22px" Font-Bold="true" Width="190px" Height="30px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 45px">
                                                <asp:RadioButton ID="ProLogsRB" runat="server" Text="Pro Logs" AutoPostBack="true" Font-Size="22px" Font-Bold="true" Width="190px" Height="30px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 45px">
                                                <asp:Button ID="BackPageButton" runat="server" Text="&#x21e6; Back" AutoPostBack="true" Font-Size="15px" Font-Bold="true" Width="100px" Height="35px" ForeColor="#cc3300" />
                                                <asp:Button ID="DownloadButton" runat="server" Text="&#x21e9; Download" AutoPostBack="true" Font-Size="15px" Font-Bold="true" Width="100px" Height="35px" ForeColor="#0acc00" />
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
                                <asp:Panel ID="FileDirectoryPanel" runat="server" Visible="true" Style="width: auto" Height="475px" BackColor="White" ScrollBars="Vertical">
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
