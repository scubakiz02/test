<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="LabelPhase.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <script type="text/javascript">
                window.addEventListener("load", function () {
                    document.getElementById("ctl00_MasterPagePanelTop").style.display = "none"; //hide header
                    document.getElementById("ctl00_MasterPagePanelBottom").style.display = "none"; //hide footer
                    document.getElementById("ctl00_MasterPagePanel").style.minWidth = "unset"; //prevent min-width on div with id of 'ctl00_MasterPagePanel'

                    //modify styles placed on html body
                    document.body.style.background = "none";
                    document.body.style.margin = "0";
                })

                function redirect(url) {
                    window.location.href = url + this.id
                }

                function disableIframe() {
                    window.parent.iframeEnabled(false);
                }
            </script>

            <style>
                :root {
                    --UWhitespace: 0.5em;
                    --UFontSize: (calc(var(--UWhitespace) * 2))
                }
            </style>


            <asp:Panel runat="server" ID="PhaseInterfacePanel" CssClass="InterfacePanel" Style="display: flex; flex-direction: column; align-items: baseline; gap: var(--UWhitespace);">
                <div>
                    <asp:Label runat="server" Text="Configurate Phases For"></asp:Label>
                    <asp:Label ID="AreaPhase" Text="" Style="color: blue" runat="server" />
                </div>

                <div style="display: flex; flex-direction: column; width: 500px;">
                    <asp:Label runat="server" Text="Select Phase:"></asp:Label>

                    <asp:ListBox
                        ID="PhaseListBox"
                        runat="server"
                        DataSourceID="PhaseListBox_SqlDataSource"
                        DataTextField="Phase"
                        DataValueField="Key"
                        CssClass="Width"
                        Height="200px"
                        AutoPostBack="True"
                        OnSelectedIndexChanged="PhaseListBox_SelectedIndexChanged" />

                    <asp:SqlDataSource ID="PhaseListBox_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"></asp:SqlDataSource>

                    <div style="display: flex; align-items: center; justify-content: space-between;">
                        <asp:FormView ID="PhaseFormView" Style="width: calc(var(--Width) - 100px); margin: var(--UWhitespace) 0;" runat="server" DataKeyNames="Key" DataSourceID="PhaseFormView_SqlDataSource" CellPadding="4" ForeColor="#333333">
                            <EmptyDataTemplate>
                                <asp:Panel runat="server" BackColor="#F7F6F3" ForeColor="#333333">
                                    Phase: No data loaded yet...
                                    <br />
                                    <asp:LinkButton runat="server" Enabled="False" CausesValidation="False" CommandName="Delete" Text="Delete" />
                                    &nbsp;<asp:LinkButton runat="server" Enabled="False" CausesValidation="False" CommandName="Edit" Text="Edit" />
                                    &nbsp;<asp:LinkButton ID="EmptyPhaseNewButton" OnClick="NewButton_OnClick" runat="server" CausesValidation="False" CommandName="New" Text="New" />
                                </asp:Panel>
                            </EmptyDataTemplate>

                            <EditItemTemplate>
                                Phase:
                    <asp:TextBox Style="width: 400px" ID="PhaseTextBox" runat="server" Text='<%# Bind("Phase") %>' />
                                <br />
                                <asp:LinkButton ID="PhaseUpdateButton" OnClick="UpdateButton_onClick" runat="server" CausesValidation="True" CommandName="Update" Text="Update" />
                                &nbsp;<asp:LinkButton ID="PhaseUpdateCancelButton" OnClick="UpdateCancelButton_OnClick" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                            </EditItemTemplate>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <InsertItemTemplate>
                                Phase:
                    <asp:TextBox Style="width: 400px" ID="PhaseTextBox" runat="server" Text='<%# Bind("Phase") %>' />
                                <br />
                                <asp:LinkButton ID="PhaseInsertButton" OnClick="InsertButton_onClick" runat="server" CausesValidation="True" CommandName="Insert" Text="Insert" />
                                &nbsp;<asp:LinkButton ID="PhaseInsertCancelButton" OnClick="InsertCancelButton_onClick" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                            </InsertItemTemplate>
                            <ItemTemplate>
                                Phase:
                    <asp:Label ID="PhaseLabel" runat="server" Text='<%# Bind("Phase") %>' />
                                <br />
                                <asp:LinkButton ID="PhaseDeleteButton" OnClick="DeleteButton_OnClick" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" />
                                &nbsp;<asp:LinkButton ID="PhaseEditButton" OnClick="EditButton_OnClick" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" />
                                &nbsp;<asp:LinkButton ID="PhaseNewButton" OnClick="NewButton_OnClick" runat="server" CausesValidation="False" CommandName="New" Text="New" />
                            </ItemTemplate>
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        </asp:FormView>

                        <%--InsertCommand value is a select query, because it's a workaround on the asp.net architecture to prevent empty TextBox values from creating a record in DB--%>
                        <asp:SqlDataSource ID="PhaseFormView_SqlDataSource" runat="server" ConflictDetection="OverwriteChanges" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                            DeleteCommand="DELETE FROM [T_LogPhase] WHERE [Key] = @original_Key AND [Phase] = @original_Phase"
                            InsertCommand="SELECT * FROM T_LogPhase"
                            SelectCommand=""
                            UpdateCommand="UPDATE [T_LogPhase] SET [Phase] = @Phase WHERE [Key] = @original_Key AND [Phase] = @original_Phase">
                            <DeleteParameters>
                                <asp:Parameter Name="original_Key" Type="Int32" />
                                <asp:Parameter Name="original_Phase" Type="String" />
                            </DeleteParameters>
                            <InsertParameters>
                                <asp:Parameter Name="Phase" Type="String" />
                            </InsertParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="Phase" Type="String" />
                            </UpdateParameters>
                        </asp:SqlDataSource>

                        <asp:Panel ID="PhaseOrderInterfacePanel" runat="server" Style="display: flex; flex-direction: column; align-items: normal; gap: var(--UWhitespace);">
                            <asp:Button ID="UpInOrderPhaseButton" OnClick="PhaseOrderInterface_onClick" Text="up" runat="server" />
                            <asp:Button ID="DownInOrderPhaseButton" OnClick="PhaseOrderInterface_onClick" Text="down" runat="server" />
                        </asp:Panel>
                    </div>
                </div>
            </asp:Panel>
            <asp:Button OnClientClick="disableIframe();" Text="Exit" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
