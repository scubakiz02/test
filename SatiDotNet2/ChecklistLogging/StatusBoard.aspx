<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="StatusBoard.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePane" runat="server">

        <ContentTemplate>
            <script src="../scripts/WebComponents/Spinner.js"></script>
            <script type="text/javascript">
                let satiSpinner;
                let threeDotSpinner;
                let cancelBuildOfLogs = false;
                let builtFirst50Logs = false;
                let dataChunkingDone = false;
                let checklistsConfig = {};
                let BuildMoreLogs_Hyperlink;
                let pageReloadInterval = 60000; //1 minute
                let pageReloadTimer;

                function startTimer() {
                    pageReloadTimer = setInterval(() => {
                        window.location.reload();
                    }, pageReloadInterval);
                }

                window.addEventListener("load", function () {
                    satiSpinner = document.body.querySelector("sati-spinner");
                    threeDotSpinner = document.body.querySelector(".dots-spinner");
                    BuildMoreLogs_Hyperlink = document.getElementById('<%= BuildMoreLogs_Hyperlink.ClientID %>');
                    startTimer();
                })

                window.addEventListener("DOMContentLoaded", async function () {
                    //build controls for PastIssuesPanel after html has rendered, to reduce long render times for initial load of page
                    //waiting to receive server side event to build controls for Past Issues
                    fetch('PastIssues.ashx')
                        .then(response => {
                            const reader = response.body.getReader();
                            const decoder = new TextDecoder();

                            function buildFirst50logs() {
                                build50Logs();
                                builtFirst50Logs = true; //this must be after invocation of build50Logs()
                                threeDotSpinner.style.display = "none";
                            }

                            function readChunk() {
                                reader.read().then(({ done, value }) => {
                                    let chunk = decoder.decode(value);

                                    if (done) {
                                        console.log('Stream finished');
                                        dataChunkingDone = true;
                                        if (!builtFirst50Logs) buildFirst50logs();
                                        return;
                                    }

                                    for (const checklistChunk of chunk.split("\n")) {
                                        let checklistChunkConfig = {};
                                        let checklist;

                                        try {
                                            checklistChunkConfig = JSON.parse(checklistChunk);
                                            checklist = checklistChunkConfig.Value.Checklist;

                                            if (!checklistsConfig.hasOwnProperty(checklist)) {
                                                checklistsConfig[[checklist]] = {};
                                            }

                                            checklistsConfig[checklist][checklistChunkConfig.Key] = checklistChunkConfig.Value;

                                            console.log("successful parsing of JSON: \n" + checklistChunk)
                                        }
                                        catch {
                                            console.log("error when parsing JSON: \n" + checklistChunk);
                                        }
                                    }

                                    //must collect all data chunks from http response, but ALSO build exactly the 50 first received logs
                                    if (configCount(checklistsConfig) >= 50 && !builtFirst50Logs) buildFirst50logs();

                                    readChunk(); // Read the next chunk
                                });
                            }

                            readChunk(); // Start reading chunks

                        })
                        .catch(error => {
                            console.error('Error fetching data:', error);
                        });
                })

                function build50Logs() {
                    let checklistsToBuildConfig = {};

                    for (const checklist of Object.keys(checklistsConfig)) {
                        for (const areaKey of Object.keys(checklistsConfig[checklist])) {
                            let drillDownPath = checklistsConfig[checklist];
                            let logConfig = drillDownPath[areaKey];

                            if (!checklistsToBuildConfig[checklist]) checklistsToBuildConfig[checklist] = {};

                            checklistsToBuildConfig[checklist][areaKey] = logConfig;
                            delete drillDownPath[areaKey];

                            if (configCount(checklistsToBuildConfig) === 50 || configCount(checklistsConfig) === 0) {
                                //calling the function below asynchronously
                                //this is in case user invocates 'redirect' js function while BuildLogs() function is still executing
                                //instead of having to wait for BuildLogs() to execute, redirect() is called right away, b/c BuildLogs() is executing asynchronously
                                BuildLogs(checklistsToBuildConfig);
                                threeDotSpinner.style.display = "none";

                                //programmatically scroll to bottom of page IF user has clicked 'More...' hyperlink
                                //'More...' hyperlink onclick event invocates build50Logs AND passes itself as this pointer
                                if (this.id) {
                                    let newInterval = 300000; //5 minutes

                                    //set timer control interval property to 5 minutes upon interaction with 'More...' hyperlink
                                    if (pageReloadInterval !== newInterval) {
                                        clearInterval(pageReloadTimer);
                                        pageReloadInterval = newInterval;
                                        startTimer();
                                    }

                                    setTimeout(() => { // give DOM time to layout fully
                                        window.scrollTo({ top: document.documentElement.scrollHeight, behavior: 'smooth' });
                                    }, 100);
                                }

                                if (dataChunkingDone && configCount(checklistsConfig) === 0) BuildMoreLogs_Hyperlink.style.display = "none";
                                else BuildMoreLogs_Hyperlink.style.display = "";

                                return;
                            }
                        }
                    }
                }

                function configCount(config) {
                    let count = 0;

                    for (const checklist of Object.keys(config)) {
                        for (const areaKey of Object.keys(config[checklist])) {
                            count++;
                        }
                    }

                    return count;
                }

                function BuildLogs(partitionedChecklistsConfig) {
                    if (partitionedChecklistsConfig) {
                        const checklistsArr = Object.keys(partitionedChecklistsConfig);
                        const PastIssuesPanel = document.getElementById('<%= PastIssuesPanel.ClientID %>');

                        for (const checklist of checklistsArr) {
                            const checklistConfig = partitionedChecklistsConfig[checklist];
                            const areaKeyArr = Object.keys(checklistConfig);

                            if (cancelBuildOfLogs) return;

                            for (const areaKey of areaKeyArr) {
                                const areaKeyConfig = checklistConfig[areaKey];
                                const Panel = document.createElement("div");
                                const SubPanel = document.createElement("div");
                                const IconPanel = document.createElement("div");
                                const LogButton = document.createElement("input");
                                const LogStatus = areaKeyConfig.LogStatus;
                                const StripeColor = areaKeyConfig.StripeColor
                                let CssStripedBackground = "background: repeating-linear-gradient(60deg, " + LogStatus + ", " + LogStatus + " 10px, " + StripeColor + ", " + StripeColor + " 20px);"

                                Panel.setAttribute("style", "display: inline-block; border: 2px solid black; " + CssStripedBackground)
                                SubPanel.setAttribute("style", "display: flex")
                                LogButton.setAttribute("style", "width: 100%; border: none; cursor: pointer; " + CssStripedBackground)
                                IconPanel.setAttribute("style", "display: flex; align-items: center; cursor: pointer;")

                                IconPanel.id = "IconPanel_" + areaKey

                                if (LogStatus !== "red" && LogStatus !== "pink")  //log has to be complete to receive icons
                                {
                                    const iconsList = JSON.parse(areaKeyConfig.iconsConfig);

                                    iconsList.forEach(function (iconSrc) {
                                        const icon = document.createElement("img");

                                        icon.setAttribute("src", iconSrc);
                                        icon.setAttribute("class", "ChecklistButtonIcon");
                                        icon.addEventListener("click", function () {
                                            redirect("Log.aspx?Key=" + areaKey);
                                            satiSpinner.displaySpin();
                                            cancelBuildOfLogs = true;
                                        })

                                        IconPanel.appendChild(icon);
                                    });
                                }

                                LogButton.setAttribute("type", "button")
                                LogButton.setAttribute("class", "ChecklistButton");
                                LogButton.id = areaKey;
                                LogButton.value = checklist;
                                LogButton.addEventListener("click", function () {
                                    redirect("Log.aspx?Key=" + areaKey);
                                    satiSpinner.displaySpin();
                                    cancelBuildOfLogs = true;
                                })

                                PastIssuesPanel.appendChild(Panel);

                                Panel.appendChild(SubPanel);

                                SubPanel.appendChild(LogButton);
                                SubPanel.appendChild(IconPanel);
                            }

                        }

                        //end loading effects for user to see past issues have been built

                    }
                }


                function redirect(url) {
                    window.location.href = url;
                }

            </script>

            <style>
                :root {
                    --UWhitespace: 0.5em;
                    --UFontSize: calc(var(--UWhitespace) * 1.5);
                    --ChecklistButtonWidth: calc(100vw / 5); /*5 so there is room for the 'special' (> monthly) checklists column*/
                    --ChecklistButtonHeight: 50px;
                }

                .SectionPanel {
                    margin: var(--UWhitespace) 0;
                    display: flex;
                    flex-direction: column;
                    gap: var(--UWhitespace);
                }

                .SubSection {
                    display: grid;
                    grid-template-columns: 1fr;
                }

                .SectionLabel {
                    font-size: calc(var(--UFontSize) * 2);
                    font-weight: bold;
                }

                .SubSectionLabel {
                    font-size: calc(var(--UFontSize) * 1.75);
                }

                .ItalicizeLabel {
                    font-style: italic;
                    color: gray;
                    font-size: calc(var(--UFontSize)* 2);
                }

                #ctl00_MasterPagePanelTop {
                    display: none;
                }

                #ctl00_MasterPagePanelBottom {
                    display: none;
                }

                #ctl00_MasterPagePanel {
                    min-width: unset;
                }

                .MasterMainBackground {
                    background: none;
                    margin: 0;
                }

                #ctl00_MasterPagePanelMain {
                }

                .ChecklistButton {
                    height: var(--ChecklistButtonHeight);
                    text-overflow: ellipsis;
                }

                .ChecklistButtonIcon {
                    height: calc(var(--ChecklistButtonHeight) * .5);
                }

                .ColorCodingMessages {
                    display: flex;
                    width: 100%;
                    font-size: var(--UFontSize);
                    text-wrap: nowrap;
                    align-items: baseline;
                    justify-content: space-between;
                    flex-direction: column;
                    gap: var(--UWhitespace);
                }

                .ColoredSquares {
                    width: 25px;
                    height: 25px;
                    padding: 0 var(--UWhitespace);
                }

                .DepAndViewMenus {
                    display: flex;
                    flex-direction: column;
                    gap: var(--UFontSize);
                }

                .TimeTravelCalendar td {
                    padding: .5em .75em;
                }

                /*================== 3 dot spinner =======================*/

                .dots-spinner {
                    display: flex;
                    align-items: center;
                    gap: 6px;
                    height: 40px;
                }

                    .dots-spinner span {
                        width: 10px;
                        height: 10px;
                        background-color: #333;
                        border-radius: 50%;
                        animation: bounce 0.6s infinite ease-in-out;
                    }

                        .dots-spinner span:nth-child(2) {
                            animation-delay: 0.2s;
                        }

                        .dots-spinner span:nth-child(3) {
                            animation-delay: 0.4s;
                        }

                @keyframes bounce {
                    0%, 80%, 100% {
                        transform: scale(0.6);
                        opacity: 0.3;
                    }

                    40% {
                        transform: scale(1);
                        opacity: 1;
                    }
                }

                /*================== 3 dot spinner =======================*/


                @media (min-width: 601px) {
                    .ColorCodingMessages {
                        font-size: calc(var(--UFontSize)* 1.25);
                    }

                    .DepartmentMenu {
                        font-size: calc(var(--UFontSize) * 1.5);
                    }

                    .StampIndicators {
                        display: flex;
                        gap: var(--UWhitespace);
                    }

                    .PageHeader {
                        display: flex;
                        flex-direction: row-reverse;
                        justify-content: space-between;
                    }
                }

                @media (min-width: 601px) and (orientation: portrait) { /*tablets in portrait mode*/
                    .SubSection {
                        grid-template-columns: 1fr 1fr;
                    }
                }

                @media (min-width: 601px) and (orientation: landscape) { /*tablets in landscape mode*/
                    .CurrentLogsPanel {
                        display: flex;
                        justify-content: space-around;
                    }

                    .ItalicizeLabel {
                        font-size: calc(var(--UFontSize) * 1.5);
                    }

                    .ChecklistButton {
                        max-width: var(--ChecklistButtonWidth);
                        text-overflow: ellipsis;
                        padding: var(--UWhitespace);
                        height: auto;
                    }

                    .DepAndViewMenus {
                        flex-direction: row;
                    }

                    .ColorCodingMessages {
                        justify-content: normal;
                        flex-direction: row;
                        gap: unset;
                    }

                    .PastIssuesHeader {
                        display: flex;
                        align-items: baseline;
                        gap: var(--UWhitespace);
                    }

                    .PageHeader {
                        align-items: center;
                    }
                }

                @media (min-width: 1280px) {
                    :root {
                        --UFontSize: calc(var(--UWhitespace)* 2);
                    }

                    .ChecklistButton {
                        font-size: var(--UFontSize);
                    }

                    .TimeTravelCalendar td {
                        padding: .25em;
                    }
                }

                @media (min-width: 1920px) {
                    .MonthlyLogsPanel .SubSection {
                        grid-template-columns: 1fr 1fr;
                    }

                    .MonthlyLogsPanel .ChecklistButton {
                        max-width: calc(var(--ChecklistButtonWidth) / 1.5); /*trim max-width compared to smaller devices*/
                    }
                }

                @media (min-width: 2560px) {
                    :root {
                        --UFontSize: calc(var(--UWhitespace)* 3);
                    }

                    .ColoredSquares {
                        width: 50px;
                        height: 50px;
                    }
                }

                @media (min-width: 3840px) {
                    :root {
                        --UFontSize: calc(var(--UWhitespace)* 4);
                    }
                }
            </style>

            <sati-spinner></sati-spinner>

            <%--style="display: flex; justify-content: space-between;"--%>
            <div style="display: flex; flex-direction: column-reverse;">

                <asp:Panel ID="AdminPanel" runat="server" Visible="False" Style="display: flex; flex-direction: column; gap: var(--UWhitespace);">
                    <div class="PastIssuesHeader">
                        <asp:Label runat="server" CssClass="SectionLabel" Text="Past Issues"></asp:Label>
                        <asp:Panel runat="server" ID="StampIndicatorLabelsPanel" CssClass="StampIndicators">
                            <div style="display: flex; align-items: center;">
                                <span>F&amp;M Manager =</span>
                                <img src="../Color/wrench-fill.png" style="width: 20px; height: 20px;" />
                            </div>
                            <div style="display: flex; align-items: center;">
                                <span>Q/SHE Manager =</span>
                                <img src="../Color/list-checks-fill.png" style="width: 20px; height: 20px;" />
                            </div>
                            <div style="display: flex; align-items: center;">
                                <span>F&amp;M Manager =</span>
                                <img src="../Color/factory-fill.png" style="width: 20px; height: 20px;" />
                            </div>
                            <div style="display: flex; align-items: center;">
                                <span>Maint Sup =</span>
                                <img src="../Color/pipe-wrench-fill.png" style="width: 20px; height: 20px;" />
                            </div>
                        </asp:Panel>
                    </div>

                    <asp:Panel ID="PastIssuesPanel" runat="server" Style="">
                    </asp:Panel>

                    <div style="display: flex; align-items: center;">
                        <asp:LinkButton ID="BuildMoreLogs_Hyperlink" OnClientClick="build50Logs.call(this); return false;" Style="display: none;" Text="More..." runat="server" />
                        <div class="dots-spinner">
                            <span></span>
                            <span></span>
                            <span></span>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel runat="server">
                    <div class="PageHeader">
                        <asp:Label ID="WhereLabel" CssClass="SubSectionLabel" runat="server" />

                        <asp:Panel ID="ColorCodingMessages" CssClass="ColorCodingMessages" runat="server" Style="">
                            <div style="display: flex; align-items: center; justify-content: center;">
                                <div style="display: flex; align-items: center; justify-content: center;">
                                    <svg class="ColoredSquares" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1 1">
                                        <path d="m0,0h1v1H0" fill="#FFC0CB" />
                                    </svg>
                                    <p style="margin: 0">= NOT STARTED</p>
                                </div>
                                <div style="display: flex; align-items: center; justify-content: center; margin: 0 10px;">
                                    <svg class="ColoredSquares" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1 1">
                                        <path d="m0,0h1v1H0" fill="#FF0000" />
                                    </svg>
                                    <p style="margin: 0">= NEEDS COMPLETION</p>
                                </div>
                            </div>
                            <div style="display: flex; align-items: center; justify-content: center;">
                                <div style="display: flex; align-items: center; justify-content: center;">
                                    <div class="ColoredSquares" style="padding: 0; margin: 0 var(--UWhitespace); background: repeating-linear-gradient(60deg, #33cc33, #33cc33 10px, #ADD8E6, #ADD8E6 20px);">
                                    </div>
                                    <p style="margin: 0">= COMPLETE & NEEDS STAMP</p>
                                </div>
                                <div style="display: flex; align-items: center; justify-content: center; margin: 0 10px;">
                                    <svg class="ColoredSquares" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1 1">
                                        <path d="m0,0h1v1H0" fill="#33CC33" />
                                    </svg>
                                    <p style="margin: 0">= COMPLETE</p>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>

                    <asp:Panel runat="server" ID="CurrentLogsPanel" CssClass="CurrentLogsPanel">
                        <asp:Panel CssClass="SectionPanel" ID="OneTimeLogsPanel" runat="server">
                            <asp:Label runat="server" Text="One Time Logs" CssClass="SectionLabel"></asp:Label>

                            <div>
                                <asp:Label runat="server" Text="Users" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="OneTimeUsersPanel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="OneTimeUsersNoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="D1" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="OneTimeD1Panel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="OneTimeD1NoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="N1" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="OneTimeN1Panel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="OneTimeN1NoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="D2" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="OneTimeD2Panel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="OneTimeD2NoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="N2" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="OneTimeN2Panel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="OneTimeN2NoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="Days (M-F)" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="OneTimeMFShiftPanel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="OneTimeMFShiftNoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                        </asp:Panel>
                        <asp:Panel CssClass="SectionPanel" ID="DailyLogsPanel" runat="server">
                            <asp:Label runat="server" Text="Daily Logs" CssClass="SectionLabel"></asp:Label>
                            <div>
                                <asp:Label runat="server" Text="Day Shift" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="DailyDayShiftPanel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="DailyDayShiftNoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="Night Shift" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="DailyNightShiftPanel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="DailyNightShiftNoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="Days (M-F)" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="DailyMFShiftPanel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="DailyMFShiftNoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                        </asp:Panel>
                        <asp:Panel Style="margin: var(--UWhitespace) 0" ID="WeeklyLogsPanel" runat="server">
                            <asp:Label runat="server" Text="Weekly Logs" CssClass="SectionLabel"></asp:Label>

                            <div>
                                <asp:Label runat="server" Text="Users" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="WeeklyUsersPanel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="WeeklyUsersNoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="D1" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="WeeklyD1Panel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="WeeklyD1NoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="N1" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="WeeklyN1Panel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="WeeklyN1NoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="D2" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="WeeklyD2Panel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="WeeklyD2NoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="N2" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="WeeklyN2Panel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="WeeklyN2NoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="Days (M-F)" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="WeeklyMFShiftPanel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="WeeklyMFShiftNoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                        </asp:Panel>
                        <asp:Panel Style="margin: var(--UWhitespace) 0" ID="MonthlyLogsPanel" CssClass="MonthlyLogsPanel" runat="server">
                            <asp:Label runat="server" Text="Monthly Logs" CssClass="SectionLabel"></asp:Label>

                            <div>
                                <asp:Label runat="server" Text="Users" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="MonthlyUsersPanel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="MonthlyUsersNoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="D1" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="MonthlyD1Panel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="MonthlyD1NoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="N1" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="MonthlyN1Panel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="MonthlyN1NoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="D2" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="MonthlyD2Panel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="MonthlyD2NoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="N2" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="MonthlyN2Panel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="MonthlyN2NoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="Days (M-F)" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="MonthlyMFShiftPanel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="MonthlyMFShiftNoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                        </asp:Panel>

                        <%--<asp:Panel Style="margin: var(--UWhitespace) 0" ID="SpecialLogsPanel" runat="server" Visible="False">--%>
                        <asp:Panel Style="margin: var(--UWhitespace) 0" ID="SpecialLogsPanel" runat="server">
                            <asp:Label runat="server" Text="Special Logs" CssClass="SectionLabel"></asp:Label>

                            <div>
                                <asp:Label runat="server" Text="Quarterly" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="QuarterlyPanel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="QuarterlyNoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="Bi-Annual" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="BiAnnualPanel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="BiAnnualNoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="1 Year" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="OneYearPanel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="OneYearNoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="2 Year" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="TwoYearPanel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="TwoYearNoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="3 Year" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="ThreeYearPanel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="ThreeYearNoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="4 Year" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="FourYearPanel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="FourYearNoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="5 Year" CssClass="SubSectionLabel"></asp:Label>
                                <asp:Panel runat="server" ID="FiveYearPanel" CssClass="SubSection">
                                    <asp:Label runat="server" ID="FiveYearNoneLabel" Text="NONE AT THIS TIME" CssClass="ItalicizeLabel"></asp:Label>
                                </asp:Panel>
                            </div>

                        </asp:Panel>

                    </asp:Panel>

                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

