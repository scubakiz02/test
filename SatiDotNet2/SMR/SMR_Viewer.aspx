<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SMR_Viewer.aspx.vb" Inherits="MR_MR_Viewer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        document.addEventListener("visibilitychange", function () {
            if (!document.hidden) { // The page is visible (user has returned to the tab)
                __doPostBack('<%= GridView1.ClientID %>', ''); //refresh 'GridView1' GridView control
            }
        });

        window.addEventListener("load", function () {
        })

        function openModal(modalId, iframeSrc) {
            const modal = document.getElementById(modalId);
            let iframe;

            if (modal == null) return
            modal.classList.add('active')
            overlay.classList.add('active')

            iframe = modal.querySelector("iframe");
            iframe.src = iframeSrc;

            document.body.style.overflow = "hidden"; //to prevent scrolling outside of modal
        }

        function closeModal(modalId) {
            const modal = document.getElementById(modalId);

            if (modal == null) return
            modal.classList.remove('active')
            overlay.classList.remove('active')

            document.body.style.overflow = "visible"; //default property
        }

        function hoverModal(modalID, iframeSrc) {
            const modal = document.getElementById(modalID);

            //invocate openModal if the modal has NOT been opened yet
            if (!modal.classList.contains("active")) {
                openModal(modalID, iframeSrc);
            }
        }
    </script>
    <style>
        :root {
            --view-parts-modal-width: 750px;
            --view-parts-modal-height: calc(var(--view-parts-modal-width) * .75);
        }

        /*======== GridView1 =========*/
        #smr-note-section {
            max-width: 200px;
            overflow: hidden;
            text-overflow: ellipsis;
            text-wrap: nowrap;
        }

        #order-parts-and-modal-section {
            position: relative;
        }

        #order-parts-section {
            display: flex;
            gap: 5px;
            align-items: center;
        }

        .order-parts-icon {
            height: 18px;
        }

        .center-align-column {
            text-align: center;
        }

        .edit-est-hrs-textbox {
            width: 30px;
        }

        /* ========== order-parts-modal & view-parts-modal ========= */
        .modal {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%) scale(0);
            border: 1px solid black;
            border-radius: 10px;
            z-index: 10;
            background-color: white;
            font-size: calc(var(--UFontSize));
            text-wrap: nowrap;
            visibility: hidden; /* Keeps the box “out of flow” for clicks */
            opacity: 0; /* Fully transparent */
            transition: opacity 0.3s ease, /* Fade in/out over 0.3s */
            visibility 0s 0.3s; /* Delay hiding until after opacity transition */
        }

            .modal.active {
                transform: translate(-50%, -50%) scale(1);
                visibility: visible; /* Make it “there” immediately */
                opacity: 1; /* Fade to fully opaque */
                transition: opacity 0.3s ease, /* Fade in over 0.3s */
                visibility 0s 0s; /* No delay when showing—visibility becomes visible right away */
            }

        #view-parts-modal.modal.active {
            transform: unset;
            top: unset;
            left: unset;
            position: absolute;
            right: calc(100% + 10px); /*right: 100% aligns child element rightside edge along leftside edge of relative parent*/
            transform: translateY(-50%);
        }

        .modal-header {
            padding: var(--UWhitespace);
            display: flex;
            justify-content: right;
            align-items: center;
            border-bottom: 1px solid black;
        }

            .modal-header .close-button {
                cursor: pointer;
                border: none;
                outline: none;
                background: none;
                font-weight: bold;
                font-size: 30px;
            }

        .modal-body {
            margin: var(--UWhitespace);
            padding: var(--UWhitespace);
            border: none;
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
                z-index: 1;
            }

        #order-parts-iframe, #view-parts-iframe {
            border: none;
        }

        #order-parts-iframe {
            width: 1000px;
            height: 500px;
        }

        #view-parts-iframe {
            width: var(--view-parts-modal-width);
            height: var(--view-parts-modal-height);
        }
    </style>

    <%--for modal in GridView 'Order Parts?' column--%>
    <div id="overlay"></div>

    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Scheduled MR Viewer"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Button2" Enabled="False" runat="server" Text="Grid View Status Board" BackColor="#FFFF99" PostBackUrl="~/MR/OpenTicketStatusBoard.aspx" /><br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">

                <table class="style1">
                    <tr>
                        <td valign="top" width="200">
                            <asp:Panel ID="Panel2" runat="server" Width="200px">
                                Ticket Status:<br />
                                <asp:RadioButton
                                    ID="OpenTicketsRadioButton"
                                    runat="server"
                                    Checked="True"
                                    GroupName="Tickets"
                                    Text="Open"
                                    AutoPostBack="True"
                                    OnCheckedChanged="OpenTicketsRadioButton_CheckedChanged" />
                                &nbsp;
                                
                                <asp:RadioButton
                                    ID="ClosedRadioButton"
                                    runat="server"
                                    GroupName="Tickets"
                                    Text="Closed"
                                    AutoPostBack="True"
                                    OnCheckedChanged="ClosedRadioButton_CheckedChanged" />

                            </asp:Panel>
                            <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                                <ProgressTemplate>
                                    &nbsp;<img src="../Color/Animated_LoadingBigger.gif" />Loading...
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                        <td valign="top" width="200">
                            <asp:Panel ID="Panel3" runat="server" Width="200px">
                                StartDate:&nbsp;<asp:TextBox ID="TextBoxStartDate" runat="server" Width="79px"></asp:TextBox><br />
                                End Date:&nbsp;<asp:TextBox ID="TextBoxEndDate" runat="server" Width="79px"></asp:TextBox>
                                <asp:Button ID="ButtonRefresh" runat="server" Text="Refresh Data" /><br />
                            </asp:Panel>

                        </td>
                        <td valign="top">
                            <asp:Panel ID="Panel4" runat="server">
                                <asp:CheckBox ID="CheckBoxToolOnly" runat="server" Text="View single tool" AutoPostBack="True" /><br />
                                <asp:DropDownList ID="DropDownListTools" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="Tool" DataValueField="Tool" Width="225px"></asp:DropDownList><br />
                                <asp:Panel ID="PanelSGT" runat="server" BackColor="#66CCFF" Visible="False">
                                    <asp:CheckBoxList
                                        ID="CheckBoxList_SGL"
                                        runat="server"
                                        DataSourceID="SqlDataSource_SGN"
                                        DataTextField="SG_Name"
                                        DataValueField="SB_Tag" RepeatLayout="Flow" AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    CellPadding="4" DataKeyNames="TicketID,TicketNoteID" DataSourceID="TicketsSqlDataSource"
                    ForeColor="Black"
                    Style="border-right: thin solid; border-top: thin solid; border-left: thin solid; border-bottom: thin solid"
                    BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellSpacing="2">
                    <FooterStyle BackColor="#CCCCCC" />
                    <RowStyle BackColor="White" />
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" />
                        <asp:BoundField DataField="TicketID" HeaderText="TicketID" InsertVisible="False" ReadOnly="True"
                            SortExpression="TicketID" />
                        <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" ReadOnly="True" />
                        <asp:BoundField DataField="Tool" HeaderText="Tool" SortExpression="Tool" ReadOnly="True" />
                        <asp:BoundField DataField="IssueDate" HeaderText="Issue Date" SortExpression="Issue Date" ReadOnly="True" />
                        <asp:TemplateField HeaderText="Note" SortExpression="Note">
                            <EditItemTemplate>
                                <asp:TextBox ID="Note_TextBox" runat="server" Text='<%# Bind("Note") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <section id="smr-note-section">
                                    <asp:Label ID="Note_Label" runat="server" Font-Bold="True" Text='<%# Eval("Note") %>' ToolTip='<%# Eval("Note") %>'></asp:Label>
                                </section>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Est Hrs" SortExpression="EstimatedHrs">
                            <EditItemTemplate>
                                <asp:TextBox
                                    ID="EstHrs_TextBox"
                                    CssClass="edit-est-hrs-textbox"
                                    Text='<%# Bind("EstimatedHrs") %>'
                                    runat="server">
                                </asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <section id="est-hrs-column-section" class="center-align-column">
                                    <asp:Label ID="EstHrs_Label" runat="server" Font-Bold="True" Text='<%# Eval("EstimatedHrs") %>'></asp:Label>
                                </section>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Earliest Start Time" SortExpression="EarliestStartTime">
                            <EditItemTemplate>
                                <asp:TextBox ID="EarliestStartTime_TextBox" runat="server" TextMode="Date" Text='<%# Bind("EarliestStartTime") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <section id="earliest-start-time-column-section" class="center-align-column">
                                    <asp:Label ID="EarliestStartTime_Label" runat="server" Font-Bold="True" Text='<%# Eval("EarliestStartTime") %>'></asp:Label>
                                </section>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Scheduled Start Time" SortExpression="ScheduledStartTime">
                            <EditItemTemplate>
                                <asp:TextBox ID="ScheduledStartTime_TextBox" runat="server" TextMode="Date" Text='<%# Bind("ScheduledStartTime") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <section id="scheduled-start-time-column-section" class="center-align-column">
                                    <asp:Label ID="ScheduledStartTime_Label" runat="server" Font-Bold="True" Text='<%# Eval("ScheduledStartTime") %>'></asp:Label>
                                </section>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Order Parts?">
                            <ItemTemplate>
                                <section id="order-parts-and-modal-section">
                                    <%--display GridView on order-parts-section IF OrderParts is true--%>
                                    <section
                                        id="order-parts-section"
                                        onmouseenter='<%# "if(""" & LCase(Eval("OrderParts").ToString()) & """ === ""true"") hoverModal(""view-parts-modal"", ""SMR_Parts.aspx?Hover=True&TicketID=" & Eval("TicketID") & """); return false;" %>'
                                        onmouseleave="closeModal('view-parts-modal');">

                                        <asp:Label ID="OrderParts_Label" runat="server" Font-Bold="True" Text='<%# Eval("OrderParts") %>'></asp:Label>
                                        <%--call closeModal() function on view-parts-modal, in case user is hovering over cell then click modal icon--%>
                                        <asp:ImageButton ID="OrderParts_ImageButton"
                                            ToolTip="Order Parts"
                                            CssClass="order-parts-icon"
                                            ImageUrl="~/Color/icons/arrow-square-out.svg"
                                            Visible="False"
                                            runat="server"
                                            OnClientClick='<%# "openModal(""order-parts-modal"", ""SMR_Parts.aspx?TicketID=" & Eval("TicketID") & """); return false;" %>' />

                                    </section>

                                    <div class="modal" id="view-parts-modal">
                                        <div class="modal-body">
                                            <iframe id="view-parts-iframe"></iframe>
                                        </div>
                                    </div>

                                    <div class="modal" id="order-parts-modal">
                                        <div class="modal-header">
                                            <button class="close-button" onclick="closeModal('order-parts-modal');">x</button>
                                        </div>
                                        <div class="modal-body">
                                            <iframe id="order-parts-iframe"></iframe>
                                        </div>
                                    </div>

                                </section>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList runat="server" ID="OrderParts_DropDownList" SelectedValue='<%# Bind("OrderParts") %>'>
                                    <asp:ListItem Text="True" Value="True" />
                                    <asp:ListItem Text="False" Value="False" />
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:CommandField ShowEditButton="True" />
                    </Columns>
                    <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#808080" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#383838" />
                </asp:GridView>
                <br />
                <br />
                <br />
                <br />
                <asp:SqlDataSource ID="TicketsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    UpdateCommand="UPDATE T_SMR_TicketNotes SET Note=@Note WHERE [Key]=@TicketNoteID; UPDATE T_SMR_Tickets SET EstimatedHrs=@EstimatedHrs, EarliestStartTime=@EarliestStartTime, ScheduledStartTime=@ScheduledStartTime, OrderParts=@OrderParts WHERE SMR_Key=@TicketID">

                    <UpdateParameters>
                        <asp:Parameter Name="Note" Type="String" />
                        <asp:Parameter Name="EstimatedHrs" Type="Decimal" />
                        <asp:Parameter Name="EarliestStartTime" Type="DateTime" />
                        <asp:Parameter Name="ScheduledStartTime" Type="DateTime" />
                        <asp:Parameter Name="OrderParts" Type="Boolean" />
                    </UpdateParameters>
                </asp:SqlDataSource>

                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                    ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT Tool FROM dbo.T_Tools GROUP BY Tool ORDER BY Tool"></asp:SqlDataSource>

                <asp:SqlDataSource ID="SqlDataSource_SGN" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT T_Tool_SubGroup_Tag_Names.SG_Name, T_Tool_SubGroup_Tag_Names.SB_Tag FROM T_Tools INNER JOIN T_Tool_SubGroup_Tag_Names ON T_Tools.[Key] = T_Tool_SubGroup_Tag_Names.Tool_Key WHERE (T_Tools.Tool = 'CMP 1')"></asp:SqlDataSource>

            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

