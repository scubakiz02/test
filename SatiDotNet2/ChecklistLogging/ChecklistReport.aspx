<%@ Page Title="" Language="VB" MaintainScrollPositionOnPostback="true" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ChecklistReport.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="../scripts/WebComponents/Spinner.js"></script>
    <script src="../scripts/common.js"></script>
    <script src="../scripts/chart.umd.js"></script>
    <script defer type="text/javascript">
        let StartDate_Textbox;
        let EndDate_Textbox;
        let WebpageSpinner;
        let _inputLineChartCanvas;

        window.addEventListener("visibilitychange", function () {
            // user has returned to the tab after viewing hyperlink in 'View File' column of ReportGridView
            // induce a postback using javascript
            if (!document.hidden) {
                __doPostBack('<%= ReportGridView.ClientID %>', '');
            }
        });

        window.addEventListener("load", function () {
            const ReportGridView = document.getElementById('<%= ReportGridView.ClientID %>');
            const CheckAllCbx = document.getElementById('<%= CheckAll_CheckBox.ClientID %>');
            const LabelCbxList = document.getElementById('<%= LabelCbxList.ClientID %>');
            const CheckAllChecklists_CheckBox = document.getElementById('<%= CheckAllChecklists_CheckBox.ClientID %>');
            const AreaCheckBoxList = document.getElementById('<%= AreaCheckBoxList.ClientID %>');
            const openModalButtons = document.querySelectorAll('[data-modal-target]')
            const closeModalButtons = document.querySelectorAll('[data-close-button]')
            const gridPager = document.querySelector(".grid-pager");

            if (gridPager) gridPager.style.width = ReportGridView.offsetWidth + "px";

            //set height of ReportGridView programmatically
            if (ReportGridView) {
                let LargestReportGridViewHeight = sessionStorage.getItem("ReportGridView_Height") ? parseFloat(sessionStorage.getItem("ReportGridView_Height")) : 0;

                if (ReportGridView.offsetHeight > LargestReportGridViewHeight) {
                    LargestReportGridViewHeight = ReportGridView.offsetHeight;
                    sessionStorage.setItem("ReportGridView_Height", LargestReportGridViewHeight);
                }

                document.querySelector(".grid-container").style.height = LargestReportGridViewHeight + (ReportGridView.querySelector("tr").offsetHeight * 2) + "px"; //add height of 1 of the rows of ReportGridView to total, to ensure pager does not lay over bottom most row 
                ReportGridView.style.visibility = "visible";
            }

            WebpageSpinner = document.getElementById("WebpageSpinner");
            document.body.appendChild(WebpageSpinner);

            CheckAllFunctionality.call(CheckAllCbx, LabelCbxList); //check all functionality for label modal
            CheckAllFunctionality.call(CheckAllChecklists_CheckBox, AreaCheckBoxList); //check all functionality for checklist modal

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

            DateTbxChange.call(StartDate_Textbox);
            DateTbxChange.call(EndDate_Textbox);

            if (ReportGridView) {
                SetSpinAnimation.call(ReportGridView);
            }

            const exportButtonContainer = document.getElementById("export-button-container");
            const exportButton = document.getElementById('<%= ExportButton.ClientID %>');
            redirectClickTo(exportButtonContainer, exportButton);

            _inputLineChartCanvas = document.getElementById('input-line-chart');
            configureChartCopy(_inputLineChartCanvas);
            configureChartDownload(_inputLineChartCanvas);
        })

        function configureHyperlinkChart(config) {
            buildLineChart(config).then(function (chartInstance) {
                WebpageSpinner.displaySpin();
                configureChartClose(_inputLineChartCanvas, chartInstance);

                setTimeout(function () {
                    //fit width and height to line chart, hide spinner, and open modal
                    const lineChartModal = document.getElementById("line-chart-modal");
                    lineChartModal.classList.add("active");

                    //display line chart
                    WebpageSpinner.hideSpin();
                    openModal(lineChartModal);
                }, 1000);

                return false;
            })
        }

        const _transparentGreen = 'rgba(144,238,144,0.35)';

        function buildLineChartControlLimit(xAxisLabels, controlLimit, isLowerControlLimit) {
            let fill;
            if (isLowerControlLimit) {
                //range is >?
                fill = {
                    target: 'end', //go to min value on y-axis
                    below: _transparentGreen, //why below rather than above? I don't know :'(
                };
            }
            else {
                //range is <?
                fill = {
                    target: 'start', //go to max value on y-axis
                    above: _transparentGreen, //why above rather than below? I don't know :'(
                };
            }

            return {
                label: '',
                data: Array(xAxisLabels.length).fill(controlLimit), //Ex: [30, 30, 30, 30, 30]
                borderColor: 'red',
                borderDash: [5, 5],
                pointRadius: 0,
                fill: fill
            };
        }

        function buildLineChartDatasets(config) {
            const { xAxisLabels, data, lowerBound, upperBound, graphTitle } = config;
            let datasets = [];

            if (upperBound && lowerBound) {
                //range is ? - ?
                //not calling buildLineChartControlLimit() here
                //these control limit datasets use an index or boolean for fill rather than 'start' or 'end'

                datasets.push({
                    label: '',
                    data: Array(xAxisLabels.length).fill(upperBound), //Ex: [5, 5, 5, 5, 5, 5, 5]
                    borderColor: 'red',
                    borderDash: [5, 5],
                    pointRadius: 0,
                    fill: {
                        target: 1, //lower bound dataset
                        above: _transparentGreen, //only add fill for upperBound. otherwise, the transparent blue will be darker
                    },
                });

                datasets.push({
                    label: '',
                    data: Array(xAxisLabels.length).fill(lowerBound), //Ex: [30, 30, 30, 30, 30]
                    borderColor: 'red',
                    borderDash: [5, 5],
                    pointRadius: 0,
                    fill: false
                });
            }
            else if (!upperBound && lowerBound) {
                //range is >?
                const lclDataset = buildLineChartControlLimit(xAxisLabels, lowerBound, isLowerControlLimit = true);
                datasets.push(lclDataset);
            }
            else if (upperBound && !lowerBound) {
                //range is <?
                const uclDataset = buildLineChartControlLimit(xAxisLabels, upperBound, isLowerControlLimit = false);
                datasets.push(uclDataset);
            }
            else if (!upperBound && !lowerBound) {
                //db range is null, so fill chart above and below single dataset
                datasets.push({
                    label: '',
                    data: data,
                    borderColor: 'blue',
                    pointRadius: 0,
                    spanGaps: true,
                    fill: {
                        target: 'start', //go to max value on y-axis
                        above: _transparentGreen, //why above rather than below? I don't know :'(
                    }
                });

                datasets.push({
                    label: '',
                    data: data,
                    borderColor: 'blue',
                    pointRadius: 0,
                    spanGaps: true,
                    fill: {
                        target: 'end', //go to min value on y-axis
                        below: _transparentGreen, //why below rather than above? I don't know :'(
                    }
                });
            }

            datasets.push({
                label: graphTitle,
                data: data,
                borderColor: 'blue',
                fill: false
            });

            return datasets;
        }

        function buildLineChartTitles(config) {
            const { xAxisTitle, yAxisTitle } = config;

            if (xAxisTitle && yAxisTitle) {
                return {
                    scales: {
                        x: {
                            display: true,
                            title: {
                                display: true,
                                text: xAxisTitle
                            }
                        },
                        y: {
                            display: true,
                            title: {
                                display: true,
                                text: yAxisTitle
                            }
                        }
                    }
                }
            };

            return {};
        }

        async function buildLineChart(config) {
            const { xAxisLabels } = config;
            const ctx = _inputLineChartCanvas.getContext('2d');

            let chartConfig = {
                type: 'line',
                data: {
                    labels: xAxisLabels,
                    datasets: buildLineChartDatasets(config)
                },
                options: {
                    plugins: {
                        legend: {
                            align: 'start', //left-align line chart title
                            labels: {
                                // filter out upper and lower bounds (datasets with empty label)
                                filter: function (item, chart) {
                                    return item.text !== '';
                                },
                                //dataset title border color display
                                boxHeight: 2,
                                boxWidth: 7.5,
                                padding: 10,
                                textAlign: 'center'
                            }
                        }
                    },
                }
            };
            chartConfig.options = { ...chartConfig.options, ...buildLineChartTitles(config) }

            return new Chart(ctx, chartConfig);
        }

        function configureChartClose(canvas, chartInstance) {
            const buildLineChartCloseButton = document.getElementById("line-chart-chart-close-button");
            buildLineChartCloseButton.addEventListener("click", function () {
                const lineChartModal = document.getElementById("line-chart-modal");
                closeModal(lineChartModal);
                chartInstance.destroy();
                canvas.classList.remove("active");
                return false;
            })
        }

        function configureChartCopy(canvas) {
            const copyButton = document.getElementById("line-chart-modal-copy-button");
            copyButton.addEventListener("click", async function () {
                try {
                    const blob = await new Promise(resolve => canvas.toBlob(resolve, "image/png"));
                    await navigator.clipboard.write([
                        new ClipboardItem({ "image/png": blob })
                    ]);

                    this.classList.remove("failure");
                    this.classList.add("success");
                } catch (err) {
                    this.classList.remove("success");
                    this.classList.add("failure");
                }

                setTimeout(() => {
                    // Reset icon after 1.5s
                    this.classList.remove("success", "failure");
                }, 1500);
            })
        }

        function configureChartDownload(canvas) {
            const downloadButton = document.getElementById("line-chart-modal-download-button");
            downloadButton.addEventListener("click", function () {
                const link = document.createElement('a');
                link.href = canvas.toDataURL('image/png');
                link.download = 'chart.png';
                link.click();
            })
        }

        function CheckAllFunctionality(CheckBoxList) {
            const targetCtrl = CheckBoxList;

            this.addEventListener("click", function () {
                let checked = this.checked;

                for (const row of targetCtrl.rows) {
                    for (const cell of row.cells) {
                        const checkbox = cell.querySelector('input[type="checkbox"]')
                        checkbox.checked = checked;
                    }
                }
            })
        }

        function DateTbxChange(ErrorLabel) {
            //asp ReadOnly attribute removes calendar functionality
            //to simulate readonly effect, hinder keypress event listener
            this.addEventListener("keypress", function (e) {
                e.preventDefault();
            })

            this.addEventListener("change", function () {
                WebpageSpinner.displaySpin();
            });
        }

        function ColWidths(json) {
            const ReportGridView = document.getElementById('<%= ReportGridView.ClientID %>');
            let colgroup = document.createElement("colgroup"); //to group and style column(s)
            let ColumnOrder = ["Area", "Label", "Value", "InputDate", "InputOperator"];;
            let TableColWidths = [];
            let row;
            let cell;
            let cellText;

            if (!ReportGridView) return;
            else ReportGridView.appendChild(colgroup);

            row = ReportGridView.rows[1];
            cell = row.children[0];
            cellText = cell.innerHTML;

            //get the most narrow cell
            for (const child of row.children) {
                if (child.offsetWidth < cell.offsetWidth) {
                    cell = child
                    cellText = cell.innerHTML;
                }
            }

            //get largest width for each column based off of values in 'json' arg
            for (const Col of ColumnOrder) {
                cell.innerHTML = json[Col];
                TableColWidths.push(cell.offsetWidth);
            }

            cell.innerHTML = cellText; //return cell to its original text

            //create rules for columns using html colgroup/col elements & TableColWidths data structure
            for (let i = 0; i < TableColWidths.length - 1; i++) {
                const width = TableColWidths[i];
                let col;

                col = document.createElement("col");
                colgroup.appendChild(col);
                col.style.width = width + 5 + "px"; //add extra 5px for cushion to prevent text-wrapping
            }
        }

        function SetSpinAnimation() {
            let buttons = this.querySelectorAll("tbody a");
            buttons.forEach(button => {
                button.addEventListener("click", WebpageSpinner.displaySpin);
            });
        }

        function iterateChildren(callback, elem) { //traverse through all child elements and invoke callback function on them
            callback.call(elem);
            for (const child of elem.children) iterateChildren(callback, child);
        }

        function getAspControl(id) {
            return document.querySelector('[id$="' + id + '"]');
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

        function spinOnExport() {
            WebpageSpinner.displaySpin();

            setTimeout(function () {
                WebpageSpinner.hideSpin();
            }, 3000);
        }
    </script>
    <style>
        :root {
            --UWhitespace: 0.5em;
            --UFontSize: (calc(var(--UWhitespace) * 2));
            --Width: 300px;
        }

        /*=================== misc ================*/
        .Width {
            width: var(--Width);
        }

        .EditPreviewPanel {
            display: flex;
            gap: var(--UWhitespace);
            overflow-y: auto;
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

        /*============ ReportGridView ============*/
        .ReportGridView td, .GridViewColumn {
            text-wrap: nowrap;
        }

            .ReportGridView td span, .ReportGridView a { /*pagination elements are html span elements*/
                padding: var(--UWhitespace);
            }

        .grid-container {
            position: relative;
            display: flex;
            flex-direction: column;
        }

        .grid-pager {
            position: absolute;
            bottom: 0;
            display: flex;
            justify-content: center;
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

        .modal-footer {
            padding: var(--UWhitespace);
            display: flex;
            justify-content: space-between;
            border-top: 1px solid black;
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

        /*============== line-chart-modal ==========*/
        #line-chart-modal {
            display: flex;
            flex-direction: column;
            width: 75vw;
            height: 50vh;
        }

            #line-chart-modal.active {
                width: fit-content;
                height: fit-content;
            }

            #line-chart-modal.modal {
                border-radius: 0px;
            }

        #input-line-chart {
            box-shadow: 0 4px 16px -2px rgba(0,0,0,0.55); /* subtle shadow below canvas */
            display: block; /* removes inline gap if needed */
            margin-bottom: 0; /* ensure no extra margin */
        }

        #line-chart-modal-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
        }

        #line-chart-modal-header-copy-and-download-buttons-container {
            display: flex;
            align-items: center;
            gap: var(--UWhitespace);
        }

        #line-chart-chart-close-button, #line-chart-modal-copy-button, #line-chart-modal-download-button {
            cursor: pointer;
        }

        #line-chart-modal-copy-button {
            width: 32px;
            height: 32px;
            background-size: contain;
            background-repeat: no-repeat;
            background-position: center;
            background-image: url('../Color/icons/copy-simple-bold.svg');
        }

            #line-chart-modal-copy-button.success {
                background-image: url('../Color/icons/check-bold.svg');
            }

            #line-chart-modal-copy-button.failure {
                background-image: url('../Color/icons/warning-bold.svg');
            }

        /*============== label-modal =============*/
        #label-modal-order-by-functionality {
            display: flex;
            align-items: center;
        }

        /* =========== export-button-container =============== */
        #export-button-container {
            background: #80BEFD;
            display: flex;
            align-items: center;
            padding: 5px;
            border-radius: 5px;
            cursor: pointer;
        }

        .export-button {
            background: #80BEFD;
            border: none;
            color: #0000FF;
            font-size: 15px;
            cursor: pointer;
        }

        #export-button-icon {
            width: 21px;
        }

        /*============== pm-report-body ===============*/
        #pm-report-body {
            display: flex;
            gap: var(--UWhitespace);
            flex-direction: column;
        }

        #pm-report-body-header {
            display: flex;
            align-items: center;
            gap: var(--UWhitespace);
        }

        .pm-report-body-header-reset-button {
            width: 25px;
            cursor: pointer;
        }

        #pm-report-body-header-title {
            margin: 0;
        }
    </style>

    <sati-spinner id="WebpageSpinner"></sati-spinner>
    <div id="overlay"></div>

    <section id="pm-report-body">
        <div id="pm-report-body-header">
            <asp:ImageButton ID="ResetGridButton" CssClass="pm-report-body-header-reset-button" OnClientClick="WebpageSpinner.displaySpin();" ImageUrl="~/Color/icons/refresh.svg" runat="server" />
            <h3 id="pm-report-body-header-title">Checklist & PM Reporting</h3>
        </div>

        <div style="display: flex; gap: var(--UWhitespace);">
            <div style="display: flex; gap: var(--UWhitespace); padding: var(--UWhitespace); background-color: #FFA07A; text-wrap: nowrap;">

                <div style="display: flex; align-items: center;">
                    <asp:Label Text="Select Group:" runat="server" />
                    <asp:DropDownList ID="GroupDropDownList" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                        DataSourceID="GroupDropDownList_SqlDataSource" DataTextField="Group"
                        DataValueField="Key"
                        OnSelectedIndexChanged="GroupDropDownList_SelectedIndexChanged"
                        CssClass="Width"
                        onchange="WebpageSpinner.displaySpin();">
                        <asp:ListItem Selected="True" Value="Nothing">Select Group...</asp:ListItem>
                        <%--                        <asp:ListItem Value="0">All</asp:ListItem>--%>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="GroupDropDownList_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                        SelectCommand="SELECT G.[Group], G.[Key] FROM [ALTS].[dbo].[T_LogGroup] G ORDER BY G.[Group]"></asp:SqlDataSource>
                </div>
            </div>
        </div>


        <asp:Panel runat="server" Enabled="False" ID="FilterAndDateRangePanel" Style="display: flex; gap: var(--UWhitespace);">
            <div style="display: flex; gap: var(--UWhitespace); padding: var(--UWhitespace); background-color: #90EE90; text-wrap: nowrap;">

                <asp:Button Enabled="False" ID="FilterChecklists_Button" Text="Filter PMs/Checklists" data-modal-target="#checklistModal" runat="server" OnClientClick="return false;" />
                <asp:Button Enabled="False" ID="FilterLabels_Button" Text="Filter Inputs" data-modal-target="#label-modal" runat="server" OnClientClick="return false;" />

                <section id="modal-container">
                    <%--these modals are not visible by default--%>
                    <%--they become visible after clicking the button with a data-modal-target attribute value matching the modal id attribute value--%>
                    <div class="modal" id="checklistModal">
                        <div class="modal-header">
                            <div style="display: flex; align-items: center; gap: var(--UWhitespace);">
                                <span>Checklists To <span style="font-weight: bold;">Include</span>:</span>
                            </div>
                        </div>
                        <div class="modal-body">
                            <asp:CheckBoxList ID="AreaCheckBoxList" runat="server" RepeatColumns="2" TextAlign="Right">
                            </asp:CheckBoxList>
                        </div>

                        <div id="checklist-modal-footer" class="modal-footer">
                            <div>
                                <%--placing asp Checkbox control in a div to avoid gap between html input & span--%>
                                <asp:CheckBox ID="CheckAllChecklists_CheckBox" Text="Check All" runat="server" />
                            </div>

                            <div id="checklist-footer-buttons-container">
                                <button data-close-button class="HeaderPanelButtons">Cancel</button>
                                <asp:Button OnClientClick="WebpageSpinner.displaySpin();" ID="UpdateChecklistsButton" OnClick="UpdateChecklistsButton_OnClick" Text="Update" runat="server" CssClass="HeaderPanelButtons" BackColor="#80BEFD" />
                            </div>
                        </div>
                    </div>


                    <div class="modal" id="label-modal">
                        <div class="modal-header">
                            <span>Inputs To <span style="font-weight: bold;">Include</span>:</span>
                        </div>
                        <div class="modal-body">
                            <asp:CheckBoxList ID="LabelCbxList" runat="server" RepeatColumns="2" TextAlign="Right">
                            </asp:CheckBoxList>

                            <div style="padding: var(--UWhitespace) 0; display: flex; gap: var(--UWhitespace); justify-content: right;">
                                <button data-close-button class="HeaderPanelButtons">Cancel</button>
                                <asp:Button OnClientClick="WebpageSpinner.displaySpin();" ID="UpdateLabelsButton" OnClick="UpdateLabelsButton_OnClick" Text="Update" runat="server" CssClass="HeaderPanelButtons" BackColor="#80BEFD" />
                            </div>
                        </div>
                        <div id="label-modal-footer" class="modal-footer">
                            <div>
                                <%--placing asp Checkbox control in a div to avoid gap between html input & span--%>
                                <asp:CheckBox ID="CheckAll_CheckBox" Text="Check All" runat="server" />
                            </div>
                            <div id="label-modal-order-by-functionality">
                                <asp:Label Text="Order By: " runat="server" />
                                <asp:RadioButton ID="OrderByInputRB" GroupName="OrderByGroup" Text="Input" runat="server" />
                                <asp:RadioButton ID="OrderByDateRB" GroupName="OrderByGroup" Text="Date" runat="server" />
                            </div>
                        </div>
                    </div>
                </section>


                <div style="display: flex; align-items: center;">
                    <asp:Label Text="Start Date:" runat="server" />
                    <asp:TextBox ID="StartDate_TextBox" OnTextChanged="DatepickTextBox_OnTextChanged" AutoPostBack="True" TextMode="Date" runat="server" Style="width: calc(100% - var(--UWhitespace))" />
                </div>

                <div style="display: flex; align-items: center;">
                    <asp:Label Text="End Date:" runat="server" />
                    <asp:TextBox ID="EndDate_TextBox" OnTextChanged="DatepickTextBox_OnTextChanged" AutoPostBack="True" TextMode="Date" runat="server" Style="width: calc(100% - var(--UWhitespace))" />
                </div>

            </div>

            <div id="export-button-container">
                <asp:Button ID="ExportButton" CssClass="export-button" Enabled="False" OnClientClick="spinOnExport();" Text="Export" runat="server" />
                <img id="export-button-icon" src="../Color/icons/download-simple-bold.svg" alt="Export" />
            </div>
        </asp:Panel>

        <div class="grid-container">
            <asp:GridView ID="ReportGridView" CssClass="ReportGridView" runat="server" AllowPaging="true" PageSize="15"
                AllowSorting="True" AutoGenerateColumns="False"
                BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical"
                OnRowCommand="ReportGridView_RowCommand"
                PagerStyle-CssClass="grid-pager"
                Style="table-layout: fixed; visibility: hidden;">
                <AlternatingRowStyle BackColor="#CCCCCC" />
                <Columns>
                    <asp:TemplateField HeaderText="Checklist">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Area") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="LabelKey" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="ReportLabelKey_Label" runat="server" Text='<%# Eval("LabelKey") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Input">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Label") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Value">
                        <ItemTemplate>
                            <asp:Label ID="ReportValue_Label" runat="server" Text='<%# Eval("Value") %>'></asp:Label>
                            <asp:CheckBox Visible="False" ID="ReportValue_CheckBox" runat="server" Style="pointer-events: none;"></asp:CheckBox>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:TextBox ID="ReportValue_TextBox" runat="server" Text='<%# Bind("Value") %>' />

                            <asp:Panel ID="Checkbox_Panel" Visible="False" runat="server">
                                <asp:CheckBox ID="ReportValue_CheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel ID="DP_Panel" Visible="False" runat="server">
                                <asp:CheckBox ID="ReportValue_DpCbx1" runat="server"></asp:CheckBox>
                                <span>/</span>
                                <asp:CheckBox ID="ReportValue_DpCbx2" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel ID="HOA_Panel" Visible="False" HOA="False" runat="server">
                                <asp:DropDownList ID="ReportValue_DropDownList" runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Start Date">
                        <ItemTemplate>
                            <asp:Label ID="StartDate_Label" runat="server" Text='<%# Eval("StartDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Input Date">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("InputDate") %>'></asp:Label>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <%--placed css styles for admin mode related css classes here to reduce chances of it being found accidently--%>
                            <style>
                                .input-date-admin-mode-container {
                                    display: flex;
                                    flex-direction: column;
                                }

                                .input-date-admin-mode-label {
                                    padding: 0 !important;
                                }
                            </style>
                            <div class="input-date-admin-mode-container">
                                <asp:Label Text="mm/dd/yyyy hh:mm:ss tt" CssClass="input-date-admin-mode-label" runat="server" />
                                <asp:TextBox ID="ReportDate_TextBox" Text='<%# Eval("InputDate") %>' runat="server" />
                                <asp:Label ID="InvalidReportDate_Label" CssClass="input-date-admin-mode-label" Visible="False" ForeColor="Red" Text="error: invalid date" runat="server" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Operator">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("InputOperator") %>'></asp:Label>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <%--<asp:TextBox ID="ReportOperator_TextBox" runat="server" Text='<%# Bind("InputOperator") %>' />--%>
                            <asp:DropDownList
                                DataTextField="Operator"
                                DataValueField="Operator"
                                ID="ReportOperator_DropDownList"
                                runat="server">
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="OperatorHidden" Visible="False">
                        <ItemTemplate>
                            <asp:Label ID="ReportOperatorHidden_Label" runat="server" Text='<%# Eval("InputOperator") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="View File">
                        <ItemTemplate>
                            <asp:HyperLink runat="server"
                                Text="Log.aspx"
                                NavigateUrl='<%# "Log.aspx?Key=" + Eval("DataKey").ToString() %>'
                                Target="_blank"
                                ToolTip="Opens a new tab">
                            </asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="View Graph">
                        <ItemTemplate>
                            <%--cell content is added in code-behind ReportGridView_RowDataBound event--%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:CommandField Visible="False" ShowEditButton="True" ShowCancelButton="True" />
                </Columns>
                <FooterStyle BackColor="#CCCCCC" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
            </asp:GridView>

            <div class="modal" id="line-chart-modal">
                <div id="line-chart-modal-header" class="modal-header">
                    <div id="line-chart-modal-header-copy-and-download-buttons-container">
                        <div id="line-chart-modal-copy-button"></div>
                        <img id="line-chart-modal-download-button" src="../Color/icons/download-bold.svg" alt="download" />
                    </div>
                    <img id="line-chart-chart-close-button" src="../Color/icons/x-bold.svg" alt="close" />
                </div>
                <div class="modal-body">
                    <canvas id="input-line-chart"></canvas>
                </div>
            </div>

        </div>
    </section>

</asp:Content>

