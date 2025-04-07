<%@ Page Title="" Language="VB" MaintainScrollPositionOnPostback="true" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ChecklistReport.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script defer type="text/javascript">
        let StartDate_Textbox;
        let EndDate_Textbox;

        window.addEventListener("load", function () {
            const ReportGridView = document.getElementById('<%= ReportGridView.ClientID %>');
            const CheckAllCbx = document.getElementById('<%= CheckAll_CheckBox.ClientID %>');
            const LabelCbxList = document.getElementById('<%= LabelCbxList.ClientID %>');
            const openModalButtons = document.querySelectorAll('[data-modal-target]')
            const closeModalButtons = document.querySelectorAll('[data-close-button]')

            CheckAllCbx.addEventListener("click", function () {
                let checked = this.checked;

                for (const row of LabelCbxList.rows) {
                    for (const cell of row.cells) {
                        const checkbox = cell.querySelector('input[type="checkbox"]')
                        checkbox.checked = checked;
                    }
                }
            })

            openModalButtons.forEach(button => {
                button.addEventListener('click', () => {
                    const modal = document.querySelector(button.dataset.modalTarget)
                    openModal(modal)
                })
            })

            closeModalButtons.forEach(button => {
                button.addEventListener('click', () => {
                    const modal = button.closest('.modal')
                    closeModal(modal)
                })
            })

            StartDate_Textbox = document.getElementById('<%= StartDate_TextBox.ClientID %>')
            EndDate_Textbox = document.getElementById('<%= EndDate_Textbox.ClientID %>')

            SetTbxInputListener.call(StartDate_Textbox, document.getElementById('<%= StartDateError_Label.ClientID %>'));
            SetTbxInputListener.call(EndDate_Textbox, document.getElementById('<%= EndDateError_Label.ClientID %>'));

            SetSpinAnimation.call(document.getElementById('<%= StartDateCalendar.ClientID %>'));
            SetSpinAnimation.call(document.getElementById('<%= EndDateCalendar.ClientID %>'));

            if (ReportGridView) {
                SetSpinAnimation.call(ReportGridView);
            }
        })

        function SetCbxStatus() { //b/c CheckAll_CheckBox & CompareFields_CheckBox do NOT use AutoPostBack, which means their Checked status is NOT managed by ASP.NET
            PageMethods.CheckAll(document.getElementById('<%= CheckAll_CheckBox.ClientID %>').checked);
            PageMethods.CompareFields(document.getElementById('<%= CompareFields_CheckBox.ClientID %>').checked);
        }

        function SetTbxInputListener(ErrorLabel) {
            const self = this;

            this.addEventListener("keypress", function (e) {
                if (e.key === "Enter") {
                    displaySpin();
                    PageMethods.SetQueryStringDates(this.value, StartDate_Textbox.value, EndDate_Textbox.value ? EndDate_Textbox.value : new Date().toLocaleDateString('en-US'), function (response) {
                        let message = response["DateInRange"];

                        ErrorLabel.innerHTML = message;
                        if (response.hasOwnProperty("Url")) window.location.replace(response["Url"]);
                        if (message !== "") hideSpin();
                    });
                }
            })
        }

        function ColWidths(json) {
            const ReportGridView = document.getElementById('<%= ReportGridView.ClientID %>');

            if (!ReportGridView) return;

            const row = ReportGridView.rows[1];
            let ColumnOrder = ["Area", "Label", "Value", "InputDate", "InputOperator"];
            let TableColWidths = [];
            let cell = row.children[0];
            let cellText = cell.innerText;
            let colgroup = document.createElement("colgroup");

            ReportGridView.appendChild(colgroup);

            for (const Col of ColumnOrder) {
                cell.innerText = json[Col];
                TableColWidths.push(cell.offsetWidth + "px");
            }

            cell.innerText = cellText;

            for (let i = 0; i < TableColWidths.length - 1; i++) {
                const width = TableColWidths[i];
                let col;

                if (ColumnOrder[i] === "InputDate") continue; //since all field values will be date only, but arg 1 'json' holds date and time, skip this one

                col = document.createElement("col");
                colgroup.appendChild(col);
                col.style.width = width;
            }
        }

        function SetSpinAnimation() {
            let buttons = this.querySelectorAll("tbody a");
            buttons.forEach(button => {
                button.addEventListener("click", displaySpin);
            });
        }

        function displaySpin() {
            document.getElementById("Overlay").style.display = "flex";
        }

        function hideSpin() {
            document.getElementById("Overlay").style.display = "none";
        }

        function iterateChildren(callback, elem) { //traverse through all child elements and invoke callback function on them
            callback.call(elem);
            for (const child of elem.children) iterateChildren(callback, child);
        }

        function getAspControl(id) {
            return document.querySelector('[id$="' + id + '"]');
        }

        function setScrollPos() {
            let scrollTo;

            if (arguments.length > 0) scrollTo = arguments[0];
            else scrollTo = this.scrollTop;

            document.getElementById("<%=EditPreviewPanel_HiddenField.ClientID%>").value = scrollTo;
        }

        function syncScrollPos(id, yPos) {
            toSyncArr.push({ "idToSync": id, "yPosToSync": yPos });
        }

        function openModal(modal) {
            if (modal == null) return
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
    </script>
    <style>
        :root {
            --UWhitespace: 0.5em;
            --UFontSize: (calc(var(--UWhitespace) * 2));
            --Width: 400px;
        }

        .Width {
            width: var(--Width);
        }

        .EditPreviewPanel {
            display: flex;
            gap: var(--UWhitespace);
            overflow-y: auto;
            height: 95%;
            overflow-x: hidden;
        }

        .InterfacePanel {
            border: 2px solid black;
            padding: var(--UWhitespace);
        }

        .overlay {
            position: absolute;
            width: 100%;
            height: 100%;
            background-color: black;
            opacity: .5;
        }

        .spinner {
            width: 50px;
            height: 50px;
            border: 6px solid #fff;
            border-top: 6px solid transparent;
            border-radius: 50%;
            animation: spin 1s linear infinite;
        }

        .ReportGridView td {
            padding: var(--UWhitespace);
        }

        .GridViewColumn {
            text-wrap: nowrap;
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
            max-width: 80%;
            font-size: calc(var(--UFontSize));
            text-wrap: nowrap;
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

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }
    </style>

    <div id="Overlay" class="overlay" style="justify-content: center; align-items: center; display: none; width: 100vw; height: 100vh; top: 0; left: 0;">
        <div class="spinner"></div>
    </div>

    <%--120px for header, 80.5px for footer (footer is actually 161px, so it's divided by 2 to reach desired effect)--%>
    <asp:Panel runat="server" Style="display: flex; justify-content: space-between; height: calc(100vh - (120px + 80.5px));">
        <asp:HiddenField ID="EditPreviewPanel_HiddenField" runat="server" Value="0" />
        <%--height is 95% to prevent weird overlap with footer--%>
        <asp:Panel ID="EditPreviewPanel" CssClass="EditPreviewPanel" onscroll="setScrollPos.call(this)" runat="server" Style="">
            <asp:Panel runat="server" ID="AreaInterfacePanel" CssClass="InterfacePanel" Style="display: flex; gap: var(--UWhitespace); flex-direction: column;">
                <div style="display: flex; flex-direction: column;">
                    <asp:Label Text="Select Group:" runat="server" />
                    <asp:DropDownList ID="GroupDropDownList" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                        DataSourceID="GroupDropDownList_SqlDataSource" DataTextField="Group"
                        DataValueField="Key"
                        OnSelectedIndexChanged="GroupDropDownList_SelectedIndexChanged"
                        CssClass="Width"
                        onchange="displaySpin();">
                        <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="GroupDropDownList_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                        SelectCommand="SELECT G.[Group], G.[Key] FROM [ALTS].[dbo].[T_LogGroup] G ORDER BY G.[Group]"></asp:SqlDataSource>
                </div>

                <div style="display: flex; flex-direction: column;">
                    <asp:Label Text="Select Checklist:" runat="server" />
                    <asp:DropDownList ID="AreaDropDownList" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                        DataSourceID="AreaDropDownList_SqlDataSource" DataTextField="Area"
                        DataValueField="Key" OnSelectedIndexChanged="AreaDropDownList_SelectedIndexChanged"
                        CssClass="Width"
                        onchange="displaySpin();">
                        <asp:ListItem Selected="True" Value="0">All</asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="AreaDropDownList_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"></asp:SqlDataSource>
                </div>

                <div style="display: flex; justify-content: space-between; gap: var(--UWhitespace);">
                    <div>
                        <div style="display: flex; gap: var(--UWhitespace);">
                            <asp:Label Text="Start Date:" runat="server" />
                            <asp:Label ID="StartDateError_Label" ForeColor="red" runat="server" />
                        </div>
                        <asp:TextBox ID="StartDate_TextBox" runat="server" />
                        <asp:Calendar ID="StartDateCalendar" runat="server" OnDayRender="DatepickCalendar_OnDayRender" OnSelectionChanged="Calendar_OnSelectionChanged"></asp:Calendar>
                    </div>
                    <div>
                        <div style="display: flex; gap: var(--UWhitespace);">
                            <asp:Label Text="End Date:" runat="server" />
                            <asp:Label ID="EndDateError_Label" ForeColor="red" runat="server" />
                        </div>
                        <asp:TextBox ID="EndDate_TextBox" runat="server" />
                        <asp:Calendar ID="EndDateCalendar" runat="server" OnDayRender="DatepickCalendar_OnDayRender" OnSelectionChanged="Calendar_OnSelectionChanged"></asp:Calendar>
                    </div>
                </div>

                <asp:Panel ID="LabelCbxList_Panel" runat="server" Style="display: flex; flex-direction: column;">
                    <asp:Button Enabled="False" ID="FilterData_Button" Text="Filter Data" data-modal-target="#modal" runat="server" OnClientClick="return false;" />
                    <div class="modal" id="modal">
                        <div class="modal-header">
                            <div style="display: flex; align-items: center; gap: var(--UWhitespace);">
                                <span>Labels To <span style="font-weight: bold;">Exclude</span>:</span>

                                <%--placing asp Checkbox control in a div to avoid gap between html input & span--%>
                                <div>
                                    <asp:CheckBox ID="CheckAll_CheckBox" Text="Check All" runat="server" />
                                </div>

                                <%--placing asp Checkbox control in a div to avoid gap between html input & span--%>
                                <div>
                                    <asp:CheckBox ID="CompareFields_CheckBox" Text="Compare Fields" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="modal-body">
                            <asp:CheckBoxList ID="LabelCbxList" runat="server" RepeatColumns="2" TextAlign="Right" >
                            </asp:CheckBoxList>

                            <div style="padding: var(--UWhitespace) 0; display: flex; gap: var(--UWhitespace); justify-content: right;">
                                <button data-close-button class="HeaderPanelButtons">Cancel</button>
                                <asp:Button ID="UpdateLabelsButton" OnClick="UpdateLabelsButton_OnClick" OnClientClick="SetCbxStatus();" Text="Update" runat="server" CssClass="HeaderPanelButtons" BackColor="#80BEFD" />
                            </div>
                        </div>
                    </div>
                    <div id="overlay"></div>
                </asp:Panel>


            </asp:Panel>

            <asp:GridView ID="ReportGridView" CssClass="ReportGridView" runat="server" AllowPaging="true" PageSize="14"
                AllowSorting="True" AutoGenerateColumns="False"
                BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical"
                Style="table-layout: fixed;">
                <AlternatingRowStyle BackColor="#CCCCCC" />

                <Columns>
                    <asp:TemplateField HeaderText="Checklist">
                        <ItemStyle CssClass="GridViewColumn" Width="100px" />
                        <HeaderStyle CssClass="GridViewColumn" Width="100px" />
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Area") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Label">
                        <ItemStyle CssClass="GridViewColumn" Width="100px" />
                        <HeaderStyle CssClass="GridViewColumn" Width="100px" />
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Label") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Value">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Start Date">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("StartDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Input Date">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("InputDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Operator">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("InputOperator") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#CCCCCC" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#808080" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#383838" />
            </asp:GridView>
        </asp:Panel>
    </asp:Panel>
</asp:Content>

