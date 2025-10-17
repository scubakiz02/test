<%@ Page Title="" Language="VB" MaintainScrollPositionOnPostback="true" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ChecklistReport.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../scripts/tabulator.min.css" rel="stylesheet">
    <script src="../scripts/tabulator.min.js"></script>
    <script src="../scripts/WebComponents/Spinner.js"></script>
    <script src="../scripts/common.js"></script>
    <script src="../scripts/chart.umd.js"></script>
    <script defer type="text/javascript">
        let StartDate_Textbox;
        let EndDate_Textbox;
        let WebpageSpinner;
        let _inputLineChartCanvas;
        let _tabulatorGrid;

        window.addEventListener("load", function () {
            const CheckAllCbx = document.getElementById('<%= CheckAll_CheckBox.ClientID %>');
            const LabelCbxList = document.getElementById('<%= LabelCbxList.ClientID %>');
            const CheckAllChecklists_CheckBox = document.getElementById('<%= CheckAllChecklists_CheckBox.ClientID %>');
            const AreaCheckBoxList = document.getElementById('<%= AreaCheckBoxList.ClientID %>');
            const openModalButtons = document.querySelectorAll('[data-modal-target]')
            const closeModalButtons = document.querySelectorAll('[data-close-button]')
            const gridPager = document.querySelector(".grid-pager");

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

            const exportButtonContainer = document.getElementById("export-button-container");
            const exportButton = document.getElementById('<%= ExportButton.ClientID %>');
            redirectClickTo(exportButtonContainer, exportButton);

            _inputLineChartCanvas = document.getElementById('input-line-chart');
            //configureChartCopy(_inputLineChartCanvas);
            configureChartDownload(_inputLineChartCanvas);
        })

        window.addEventListener("DOMContentLoaded", function () {

            // =============== tabulator =================

            //create Tabulator on DOM element with id "tabulator-grid"
            _tabulatorGrid = new Tabulator("#tabulator-grid", {
                ajaxURL: "/api/pm-report/tabulator-data.ashx",
                ajaxParams: function () {
                    const groupDdl = document.getElementById('<%= GroupDropDownList.ClientID %>');
                    const groupkey = groupDdl.value === "Nothing" ? null : groupDdl.value;

                    const pmModal = document.getElementById("checklistModal");
                    const pmCbxList = document.getElementById('<%= AreaCheckBoxList.ClientID %>');
                    const pmKeys = getDbKeys(pmCbxList, pmModal);

                    const inputModal = document.getElementById("label-modal");
                    const inputCbxList = document.getElementById('<%= LabelCbxList.ClientID %>');
                    const inputKeys = getDbKeys(inputCbxList, inputModal);

                    const startDateTbx = document.getElementById('<%= StartDate_TextBox.ClientID %>');
                    const startDateAt = getDate(startDateTbx);

                    const endDateTbx = document.getElementById('<%= EndDate_TextBox.ClientID %>');
                    const endDateAt = getDate(endDateTbx);

                    return {
                        groupkey: groupkey,
                        pmKeys: pmKeys,
                        inputKeys: inputKeys,
                        startDateAt: startDateAt,
                        endDateAt: endDateAt,
                    };
                },
                ajaxResponse: function (url, params, response) {
                    //after http response is received but before tabulator records are created
                    //create tabulator tabs and add click functionality
                    const tabContainer = document.getElementById("tabulator-tab-container");

                    if (response) {
                        const pmOrChecklistNames = Object.keys(response);
                        for (let i = 0; i < pmOrChecklistNames.length; i++) {
                            const pmOrChecklistName = pmOrChecklistNames[i];

                            const tab = document.createElement("div");
                            tab.classList.add("tabulator-tab");
                            tab.innerText = pmOrChecklistName;
                            tab.setAttribute("title", pmOrChecklistName);
                            tab.addEventListener("click", function () {
                                const activeTab = tabContainer.querySelector(".active");
                                activeTab.classList.remove("active");
                                this.classList.add("active");

                                _tabulatorGrid.setData(response[pmOrChecklistName]);
                            })
                            tabContainer.appendChild(tab);

                            if (i === 0) tab.classList.add("active");
                        }

                        return response[pmOrChecklistNames[0]]; // return first dataset to Tabulator
                    }
                    else {
                        return response;
                    }
                },
                layout: "fitColumns",
                height: "100%",
                columns: [
                    { title: "Pm/Checklist", field: "checklist", headerSort: false },
                    { title: "Input", field: "input", headerSort: false },
                    { title: "Value", field: "value", headerSort: false },
                    {
                        title: "Start Date", field: "startDateAt",
                        headerSort: false
                        //    headerSort: true,
                        //    headerClick: async function (e, column) {
                        //        const table = column.getTable();
                        //        const sorters = table.getSorters();

                        //        let isSortAsc = true;
                        //        if (sorters[0].dir === "desc") {
                        //            isSortAsc = false;
                        //            table.clearSort();  // clear sort rather than sorting by desc order
                        //        }

                        //        //make http request to sort dataset in report class
                        //        WebpageSpinner.displaySpin();

                        //        setTimeout(function () {
                        //            // code to run after 3 second (simulates http request delay)
                        //            WebpageSpinner.hideSpin();
                        //        }, 3000);

                        //    }
                    },
                    { title: "Input Date", field: "inputDateAt", headerSort: false },
                    { title: "Operator", field: "operator", headerSort: false },
                    {
                        title: "View File", field: "", formatter: function (cell, formatterParams, onRendered) {
                            const data = cell.getData();
                            const querystring = "?Key=" + data.datakey;
                            return '<a href="Log.aspx' + querystring + '" target="_blank" rel="noopener">Log.aspx</a>';
                        }, headerSort: false
                    },
                    {
                        title: "View Graph", field: "", formatter: function (cell, formatterParams, onRendered) {
                            const data = cell.getData();
                            if (data.fieldtype === "Number") {
                                // Create anchor element
                                const a = document.createElement('a');
                                a.setAttribute("labelkey", data.labelkey);
                                a.className = 'tabulator-view-graph-cell';
                                a.title = 'Click to view graph';
                                a.textContent = 'View Graph';
                                a.style.cursor = 'pointer';
                                a.onclick = async function () {
                                    //const config = { "graphTitle": "Inlet Prefilter Pressure | >45 psi (09/01/2025 - 09/02/2025)", "xAxisTitle": "Input Date", "yAxisTitle": "psi", "lowerBound": "45", "upperBound": null, "xAxisLabels": ["09/01/2025", "09/02/2025"], "data": ["52", "51"] }; //for troubleshooting/debugging
                                    const config = await httpGet("/api/pm-report/tabulator-line-chart-config.ashx", { labelkey: this.getAttribute("labelkey") });
                                    configureHyperlinkChart(config);
                                };
                                return a; // Return the DOM element
                            }
                            return "";
                        }, headerSort: false
                    },
                ],
            });
        })

        function getDate(dateTbx) {
            let dateAt = null;
            if (dateTbx.value !== "") dateAt = dateTbx.value;
            return dateAt;
        }

        function getDbKeys(cbxList, modal) {
            let pmKeys = [];

            if (cbxList) {
                //modal needs to be opened for child elements to be visible
                openModal(modal);
                const cbxParents = cbxList.querySelectorAll(".filter-cbx");
                closeModal(modal);

                for (const cbxParent of cbxParents) {
                    const cbx = cbxParent.querySelector("input");
                    if (cbx.checked) {
                        const key = cbxParent.getAttribute("key");
                        pmKeys.push(parseInt(key));
                    }
                }
            }

            return pmKeys.length === 0 ? null : JSON.stringify(pmKeys);
        }

        function configureHyperlinkChart(config) {
            buildLineChart(config).then(function (chartInstance) {
                WebpageSpinner.displaySpin();
                configureChartClose(_inputLineChartCanvas, chartInstance);
                configureChartZoom(chartInstance);

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

            const chartInstance = new Chart(ctx, chartConfig);
            prepChartYAxis(chartInstance);
            return chartInstance;
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

        function prepChartYAxis(chart) {
            // Get all data points
            const allValues = chart.data.datasets.flatMap(ds => ds.data.filter(v => v !== null && v !== undefined));
            const minValue = Math.min(...allValues);
            const maxValue = Math.max(...allValues);

            // Add padding to make it look nicer
            const padding = (maxValue - minValue) * 0.1;
            const yAxisMin = minValue - padding;
            const yAxisMax = maxValue + padding;
            _ogYAxisMin = yAxisMin;
            _ogYAxisMax = yAxisMax;
            chart.options.scales.y.min = yAxisMin;
            chart.options.scales.y.max = yAxisMax;

            // Re-render chart
            chart.update();
        }

        function zoomChart(chart, zoomFactor) {
            const yScale = chart.options.scales.y;
            const currentMin = yScale.min;
            const currentMax = yScale.max;
            const range = currentMax - currentMin;
            const center = (currentMax + currentMin) / 2;

            const newRange = range * zoomFactor;
            yScale.min = center - newRange / 2;
            yScale.max = center + newRange / 2;

            chart.update();
        }

        let _ogYAxisMin;
        let _ogYAxisMax;
        function configureChartZoom(chartInstance) {
            const zoomInButton = document.getElementById("line-chart-modal-zoom-in-button");
            const zoomOutButton = document.getElementById("line-chart-modal-zoom-out-button");
            const resetZoomButton = document.getElementById("line-chart-modal-reset-zoom-button");

            zoomInButton.addEventListener("click", function () {
                zoomChart(chartInstance, 0.8);
            });

            zoomOutButton.addEventListener("click", function () {
                zoomChart(chartInstance, 1.2);
            });

            resetZoomButton.addEventListener("click", function () {
                chartInstance.options.scales.y.min = _ogYAxisMin;
                chartInstance.options.scales.y.max = _ogYAxisMax;
                chartInstance.update();
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

        /*============ tabulator-grid ===============*/

        #tabulator-grid-section {
            position: relative;
            display: flex;
            flex-direction: column;
            height: 400px;
        }

        #tabulator-tab-container {
            display: flex;
            gap: 2.5px;
            text-wrap: nowrap;
            overflow: auto;
        }

        .tabulator-tab {
            padding: var(--UWhitespace);
            border-radius: 5px 5px 0 0;
            cursor: pointer;
            border: 1px solid #ccc;
            border-bottom: none;
            background-color: #f1f1f1;
            flex: 1 1 0;
            min-width: 1%;
            max-width: 25%;
            overflow: hidden;
            text-overflow: ellipsis;
        }

            .tabulator-tab.active {
                background-color: #80BEFD;
            }

        .tabulator-view-graph-cell {
            color: blue;
            text-decoration: underline;
            cursor: pointer;
        }

        /*=========== modal classes =============*/
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

            #line-chart-modal .modal-footer {
                align-items: center;
                justify-content: normal;
                gap: var(--UWhitespace);
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

        #line-chart-chart-close-button, #line-chart-modal-copy-button, #line-chart-modal-download-button, #line-chart-modal-zoom-in-button, #line-chart-modal-zoom-out-button, #line-chart-modal-reset-zoom-button {
            cursor: pointer;
        }

        #line-chart-modal-zoom-in-button, #line-chart-modal-zoom-out-button {
            width: 24px;
            height: 24px;
        }

        #line-chart-modal-reset-zoom-button {
            display: flex;
            align-items: center;
            width: fit-content;
            padding: var(--UWhitespace);
            border: 1px solid black;
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

        <section id="tabulator-grid-section">
            <div id="tabulator-tab-container">
            </div>
            <div id="tabulator-grid"></div>

            <%--1 modal is used for spc line charts--%>
            <div class="modal" id="line-chart-modal">
                <div id="line-chart-modal-header" class="modal-header">
                    <div id="line-chart-modal-header-copy-and-download-buttons-container">
                        <%--<div id="line-chart-modal-copy-button"></div>--%>
                        <img id="line-chart-modal-download-button" src="../Color/icons/download-bold.svg" alt="download" />
                    </div>
                    <img id="line-chart-chart-close-button" src="../Color/icons/x-bold.svg" alt="close" />
                </div>
                <div class="modal-body">
                    <canvas id="input-line-chart"></canvas>
                </div>
                <div class="modal-footer">
                    <img id="line-chart-modal-zoom-out-button" src="../Color/icons/magnifying-glass-minus.svg" alt="zoom out" />
                    <img id="line-chart-modal-zoom-in-button" src="../Color/icons/magnifying-glass-plus.svg" alt="zoom in" />
                    <div id="line-chart-modal-reset-zoom-button">Reset Zoom</div>
                </div>
            </div>
        </section>
    </section>

</asp:Content>

