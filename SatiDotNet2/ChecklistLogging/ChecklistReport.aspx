<%@ Page Title="" Language="VB" MaintainScrollPositionOnPostback="true" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ChecklistReport.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script defer type="text/javascript">
        let StartDate_Textbox;
        let EndDate_Textbox;

        window.addEventListener("load", function () {
            const ReportGridView = document.getElementById('<%= ReportGridView.ClientID %>');

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
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Date") %>'></asp:Label>
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

