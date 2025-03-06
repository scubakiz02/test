<%@ Page Title="Work Instruction Viewer" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="WorkInstructionViewer.aspx.vb" Inherits="WI_WorkInstructionViewer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="MainUpdatePanel" runat="server">
        <ContentTemplate>
            <asp:Panel ID="MainContentPanel" runat="server" CssClass="ContentAutoScaler">
                <asp:Panel ID="InputPanels" runat="server" Height="100%" BackColor="lightblue" Style="width: calc(100% - 20px); padding: 10px; padding-bottom: 5px">
                    <table id="EditControlContainer" class="ContentAutoScaler">
                        <tr>
                            <td style="width: calc(100% - 985px);"></td>
                            <td style="width: 985px;">
                                <asp:Panel ID="Panel1" runat="server" BackColor="LightBlue">
                                    <div id="editorTabs" class="editorTab">
                                        <button class="editorTab active" onclick="openEditor(event, 'OpenHTMLFiles'); return false;">Open WI Files</button>
                                    </div>

                                    <div id="OpenHTMLFiles" class="editorContent" style="display: block">
                                        <contenttemplate>
                                            <table class="ContentAutoScaler" style="height: 85px; padding:5px">
                                                <tr style="width:100%">
                                                    <td style="width: 2%"></td>
                                                    <td style="width:30%">
                                                        <asp:TextBox ID="LotID" runat="server" placeholder="Lot ID Number" Width="100%" Height="22px" onkeyup="searchLotOnKeyUp(event, this.value);" AutoPostBack="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width:3%">
                                                        <asp:HiddenField ID="SearchRogue" runat="server" Value="false"></asp:HiddenField>
                                                    </td>
                                                    <td style="width:29%">
                                                        <asp:DropDownList ID="DepartDropDown" runat="server" style="width: 100%; height: 29px">
                                                            <asp:ListItem>Department</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width:2%">
                                                        <input type="text" id="RogueWI" value="" style=" width: 1px; visibility: hidden;" />
                                                        <input type="text" id="RogueFC" value="" style=" width: 1px; visibility: hidden;" />
                                                    </td>
                                                    <td style="width:30%">
                                                        <asp:Button ID="LoadWI" runat="server" Text="Load Work Instructions" Width="100%" Height="29px"></asp:Button>
                                                    </td>
                                                    <td style="width: 2%"></td>
                                                </tr>
                                            </table>
                                        </contenttemplate>
                                    </div>
                                </asp:Panel>
                            </td>
                            <td style="width: calc(100% - 985px);"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="HiddenedPanelContainer" runat="server" BackColor="LightBlue" Style="width: 100%; height: 20px;">
                    <asp:Panel ID="NotFoundPanel" runat="server" BackColor="LightBlue" Visible="False" Style="width: 100%;" HorizontalAlign="Center">
                        <asp:Label ID="FoundLabel" runat="server" Text="NOTE: THE FILES YOU ARE LOOKING FOR WERE NOT FOUND. PLEASE CHECK YOUR INPUTS" Font-Bold="true" ForeColor="DarkRed"></asp:Label>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel ID="ExcelMarkUps" runat="server" BackColor="lightblue" Style="width: 100%; height: 100%; padding-bottom: 10px;">
                    <table id="FrameContainer" class="ContentAutoScaler">
                        <tr>
                            <td style="width: calc(100% - 985px)"></td>
                            <td style="width: 985px; background-color: lightblue; border-color: lightblue;">
                                <asp:Panel ID="WorkIntructionHolder" runat="server" Height="100%" BackColor="lightgray">
                                </asp:Panel>
                                <div id="tabHolder" runat="server" class="tab">
                                </div>
                            </td>
                            <td style="width: calc(100% - 985px)"></td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <script src="../scripts/WIScripts/ExcelViewerControls.js" type="text/javascript"></script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
