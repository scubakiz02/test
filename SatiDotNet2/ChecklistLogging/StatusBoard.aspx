<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="StatusBoard.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePane" runat="server">

        <ContentTemplate>
            <script src="../scripts/WebComponents/Spinner.js"></script>
            <script src="../scripts/WebComponents/sati-full-screen.js"></script>
            <script src="../scripts/common.js"></script>
            <script src="../scripts/jquery-3.6.0.min.js"></script>
            <script src="../scripts/jquery.signalR-2.4.3.min.js"></script>
            <script src="/signalr/hubs"></script>
            <script type="text/javascript">
                let _overdueCache = new OverdueLogsCache();

                window.addEventListener("load", async function () {
                    const department = '<%= Session("DepartmentFromQueryString") %>';
                    const statusBoardDateAt = '<%= Session("WhereFromQueryString") %>';
                    const startDateCutoffAt = '<%= Session("StartDateCutoffAt") %>';
                    const view = '<%= Session("ViewFromQueryString") %>';

                    const response = await httpGet("/api/pm-logs-past-issues.ashx", {
                        department: department,
                        statusBoardDateAt: statusBoardDateAt,
                        startDateCutoffAt: startDateCutoffAt,
                        view: view
                    });
                    const datakeys = Object.keys(response);

                    if (datakeys.length > 0) {
                        //logic related to overdue log functionalities
                        for (const datakey of datakeys) {
                            const data = response[datakey];
                            let log;

                            try {
                                log = document.getElementById("log-" + datakey);
                                if (!log) throw error;
                            }
                            catch (err) {
                                //logs built in asp code-behind
                                const logId = "log-" + datakey
                                log = getAspControl(logId);
                            }

                            if (!log) {
                                //log needs to be created on status board
                                if (data.logParentId === "PastIssuesPanel") {
                                    //overdue log
                                    _overdueCache.set(datakey, data);
                                    continue;
                                }
                            }
                        }

                        build50OverdueLogs();

                        const horizontalSpinner = document.body.querySelector(".dots-spinner");
                        horizontalSpinner.style.display = "none";

                        const buildMoreHyperlink = document.getElementById("overdue-logs-build-more-hyperlink");
                        buildMoreHyperlink.addEventListener("click", function () {
                            build50OverdueLogs();

                            setTimeout(() => {
                                // give DOM time to layout fully, then programmatically scroll to bottom
                                window.scrollTo({ top: document.documentElement.scrollHeight, behavior: 'smooth' });
                            }, 100);

                        });
                    }
                })

                async function keepAspCookiesAlive() {
                    //send http request once every 10 minutes to ensure the cookies stay alive
                    const now = new Date();
                    const min = now.getMinutes();
                    const seconds = now.getSeconds();
                    if (min % 10 === 0 && seconds < 30) {
                        try {
                            const response = await fetch('/api/pm-status-board/http-session-refresh.ashx', {
                                method: 'GET',
                                credentials: 'same-origin',  // credentials: 'same-origin' ensures the ASP.NET_SessionId cookie is sent
                                cache: 'no-store'
                            });

                            if (!response.ok) {
                                console.warn('session refresh failed:', response.status);
                            }
                        } catch (err) {
                            console.warn('session refresh error:', err);
                        }
                    }
                }

                function ssePingReaction(signal, data) {
                    switch (signal) {
                        case "change":
                            const response = JSON.parse(data);
                            //const response = fakeApiCacheData(); //for debugging/troubleshooting
                            const datakeys = Object.keys(response);
                            for (const datakey of datakeys) {
                                const config = response[datakey];
                                const log = findLog(datakey, config);
                                changeLogState(log, datakey, config);
                            }
                            break;

                        case "refresh":
                            // reload status board with today's date as where qs param value
                            const url = new URL(window.location.href);
                            const today = new Date();
                            const mm = String(today.getMonth() + 1).padStart(2, '0');
                            const dd = String(today.getDate()).padStart(2, '0');
                            const yyyy = today.getFullYear();
                            const mmddyyyy = `${mm}/${dd}/${yyyy}`;
                            url.searchParams.set('WHERE', mmddyyyy);
                            window.location.href = url.toString();
                            break;

                        case "ping":
                            console.log('last ping: ' + data);
                            break;

                        default:
                            console.error("status board server side event failure");
                    }
                }

                $(function () {
                    const connection = $.connection.sseStatusBoardHub;
                    const satiSpinner = document.getElementById("status-board-spinner");

                    connection.client.statusBoardPing = async function (signal, data) {
                        ssePingReaction(signal, data);
                        await keepAspCookiesAlive();
                    };


                    //disconnect server side event connection every 10 seconds (for troubleshooting/debugging callbacks below)
                    //setInterval(function () {
                    //    $.connection.hub.stop();
                    //}, 10000);

                    $.connection.hub.connectionSlow(function () {
                        console.warn("SignalR: Connection is slow.");
                    });

                    $.connection.hub.reconnecting(function () {
                        satiSpinner.displaySpin();
                        console.warn("SignalR: Attempting to reconnect...");
                    });

                    $.connection.hub.reconnected(function () {
                        satiSpinner.hideSpin();
                        console.info("SignalR: Reconnected.");
                    });

                    $.connection.hub.disconnected(function () {
                        console.error("SignalR: Disconnected. Attempting to reconnect in 5 seconds...");
                        setTimeout(startSignalR(), 5000);
                    });

                    async function findCurrentLogs() {
                        //if logs exist, modify them if needed
                        //if logs do not exist, create them
                        const response = await httpGet("/api/pm-status-board/current-logs.ashx", {
                            statusBoardDateAt: '<%= Session("WhereFromQueryString") %>',
                            department: '<%= Session("DepartmentFromQueryString") %>'
                        });

                        for (const kvp of response) {
                            const datakey = kvp.Key;
                            const config = kvp.Value;
                            try {
                                const log = findLog(datakey, config);

                                //calling this in case log state has changed before server side event connection is established
                                //this edgecase can occur during publishes or hourly programmatic changes
                                changeLogState(log, datakey, config);
                            }
                            catch (err) {
                                continue;
                            }
                        }
                    }

                    function startSignalR() {
                        $.connection.hub.start({ transport: 'serverSentEvents' }).done(async function () {
                            console.info("SignalR: Connected.");

                            satiSpinner.displaySpin();
                            await findCurrentLogs();
                            satiSpinner.hideSpin();
                        }).fail(function () {
                            console.error("SignalR: Connection failed. Retrying in 5 seconds...");
                            setTimeout(startSignalR, 5000);
                        });
                    }

                    startSignalR();
                });


                function checkForShiftChange(datakey, config) {
                    //a shift would be a subsection within an interval (Ex: Weekly Logs D1 Shift, Weekly Logs D2 Shift, etc.)
                    let log = document.getElementById("log-" + datakey);
                    const { logParentId, logState } = config;
                    const newParentCtrl = getAspControl(logParentId);
                    const oldParentCtrl = log ? log.parentElement : null;
                    if (log && (oldParentCtrl !== newParentCtrl || logState === "delete")) {
                        //log needs to be moved or deleted
                        //in either case, delete the current log
                        oldParentCtrl.removeChild(log);
                    }

                    const hasLogsClass = "has-logs";
                    if (oldParentCtrl) {
                        if (oldParentCtrl.children.length === 1) {
                            //the only element within oldParentCtrl is the no logs message
                            oldParentCtrl.classList.remove(hasLogsClass)
                        }
                    }
                    if (newParentCtrl) newParentCtrl.classList.add(hasLogsClass);

                    return newParentCtrl;
                }

                function findLog(datakey, config) {
                    //if log does not exist, create the log
                    //if log exists, return the log
                    const newParentCtrl = checkForShiftChange(datakey, config);

                    let log = document.getElementById("log-" + datakey);
                    if (!log) {
                        log = buildLog({
                            datakey: datakey,
                            pmName: config.pmName,
                            iconsConfig: [], //the case statement below creates the icons
                            logState: config.logState
                        }, newParentCtrl)
                    }

                    return log;
                }

                function changeLogState(log, datakey, config) {
                    const { logState, removeStamps, addStamps } = config;

                    switch (logState) {
                        case "virgin":
                        case "incomplete":
                            removeStampCtrlsFrom(log);
                            break;
                        case "submitted":
                            // remove stamps
                            for (const stampRole of removeStamps) {
                                const stampCtrlClass = getStampCssClass(stampRole);
                                const stampCtrl = log.querySelector("." + stampCtrlClass);

                                if (stampCtrl) {
                                    stampCtrl.parentElement.removeChild(stampCtrl);
                                }
                            }

                            //add stamps
                            for (const stampRole of addStamps) {
                                const stampCtrlClass = getStampCssClass(stampRole);
                                const stampCtrl = log.querySelector("." + stampCtrlClass);

                                if (!stampCtrl) {
                                    const iconPanel = log.querySelector(".icon-panel");
                                    createStamp(stampCtrlClass, datakey, iconPanel)
                                }
                            }

                            break;
                        case "completed":
                            //log is complete. It has been submitted, received all its stamps, and is staying up on the status board
                            removeStampCtrlsFrom(log);
                            break;
                        case "error":
                            //error has occured. Display the message from http response on log
                            const logButton = log.querySelector(".ChecklistButton");

                            removeStampCtrlsFrom(log);
                            applyBackcolorClass(log, "error");
                            break;
                    }

                    //no matter the log state, apply the log state backcolor css classes
                    applyBackcolorClass(log, logState);
                    iterateChildren(function () {
                        applyBackcolorClass(this, logState);
                    }, log);

                }

                function removeStampCtrlsFrom(ctrl) {
                    const stampCtrls = ctrl.querySelectorAll(".stamp-icon");

                    for (const stampCtrl of stampCtrls) {
                        stampCtrl.parentElement.removeChild(stampCtrl);
                    }
                }

                function getStampCssClass(managerRole) {
                    let cssClass;

                    switch (managerRole) {
                        case "F&M Manager":
                            cssClass = "icon-fm-manager";
                            break;
                        case "Q/SHE Manager":
                            cssClass = "icon-qshe-manager";
                            break;
                        case "Prod Sup":
                            cssClass = "icon-prod-sup";
                            break;
                        case "Maint Sup":
                            cssClass = "icon-maint-sup";
                            break;
                    }

                    return cssClass;
                }

                function build50OverdueLogs() {
                    const cacheStorage = _overdueCache.getAll()
                    const datakeys = Object.keys(cacheStorage);
                    const buildMoreHyperlink = document.getElementById("overdue-logs-build-more-hyperlink");
                    const cacheTotal = _overdueCache.getCount();

                    for (let i = 0; i < 50 && i < cacheTotal; i++) {
                        const datakey = datakeys[i];
                        const parentCtrl = document.getElementById('<%= PastIssuesPanel.ClientID %>');
                        const data = cacheStorage[datakey];
                        const stampClasses = data.addStamps ? data.addStamps.map(stampRole => getStampCssClass(stampRole)) : [];

                        buildLog({
                            datakey: datakey,
                            pmName: data.pmName,
                            iconsConfig: stampClasses,
                            logState: data.logState
                        }, parentCtrl)

                        _overdueCache.remove(datakey);
                    }


                    if (_overdueCache.getCount() > 0) {
                        buildMoreHyperlink.style.display = "";
                    } else {
                        buildMoreHyperlink.style.display = "none";
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

                function applyBackcolorClass(elem, cssClass) {
                    const logStates = ["error", "virgin", "incomplete", "submitted", "completed"];

                    if (!elem) return;

                    for (const css_class of elem.classList) {
                        //remove currently applied log states
                        if (logStates.includes(css_class)) {
                            elem.classList.remove(css_class);
                        }
                    }

                    //add new css class
                    elem.classList.add(cssClass);

                    if (cssClass === "error") {
                        try {
                            pushErrorToDom(elem);
                            elem.setAttribute("title", elem.value);
                        }
                        catch (err) {
                            return;
                        }
                    }
                }

                function createStamp(stampIconClass, datakey, parentCtrl) {
                    const stampCtrl = document.createElement("div");

                    stampCtrl.classList.add("stamp-icon");
                    stampCtrl.classList.add(stampIconClass);

                    stampCtrl.addEventListener("click", function () {
                        newTab("Log.aspx?Key=" + datakey);
                    })

                    parentCtrl.appendChild(stampCtrl);
                }

                function buildLog(config, parentCtrl) {
                    let { datakey, pmName, iconsConfig, logState } = config;
                    const Panel = document.createElement("div");
                    const SubPanel = document.createElement("div");
                    const IconPanel = document.createElement("div");
                    const LogButton = document.createElement("input");

                    Panel.classList.add("button-and-stamps-container");
                    Panel.id = "log-" + datakey;
                    applyBackcolorClass(Panel, logState);

                    SubPanel.setAttribute("style", "display: flex")

                    LogButton.setAttribute("style", "width: 100%; border: none; cursor: pointer;")
                    LogButton.setAttribute("type", "button")
                    LogButton.classList.add("ChecklistButton");
                    LogButton.id = datakey;
                    LogButton.addEventListener("click", function () {
                        newTab("Log.aspx?Key=" + datakey);
                    })
                    LogButton.value = pmName;
                    applyBackcolorClass(LogButton, logState);

                    IconPanel.classList.add("icon-panel")
                    IconPanel.id = "IconPanel_" + datakey

                    iconsConfig.forEach(function (iconClass) {
                        createStamp(iconClass, datakey, IconPanel);
                    });

                    parentCtrl.appendChild(Panel);

                    Panel.appendChild(SubPanel);

                    SubPanel.appendChild(LogButton);
                    SubPanel.appendChild(IconPanel);

                    return Panel;
                }

                function pushErrorToDom(elem) {
                    const elemValue = elem.value;

                    if (!elemValue.includes("error")) {
                        elem.value = "error: '" + elemValue + "' duplication";
                    }
                }

                function newTab(url) {
                    window.open(url, '_blank');
                }


                function fakeApiCacheData() {
                    return {
                        10000000: {
                            logParentId: "DailyNightShiftPanel",
                            pmName: "some random checklist again",
                            logState: "error",
                        },
                        10000001: {
                            logParentId: "DailyDayShiftPanel",
                            pmName: "AWN Daily",
                            logState: "incomplete",
                        },
                        10000002: {
                            logParentId: "DailyNightShiftPanel",
                            pmName: "some random checklist",
                            logState: "virgin",
                        },
                        10000003: {
                            logParentId: "DailyDayShiftPanel",
                            pmName: "your mom",
                            logState: "submitted",
                            removeStamps: ["F&M Manager"],
                            addStamps: ["Prod Sup", "Maint Sup"],
                        },
                        "100000001": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 1",
                            "logState": "error"
                        },
                        "100000002": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 2",
                            "logState": "virgin"
                        },
                        "100000003": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 3",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000004": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 4",
                            "logState": "error"
                        },
                        "100000005": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 5",
                            "logState": "incomplete"
                        },
                        "100000006": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 6",
                            "logState": "virgin"
                        },
                        "100000007": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 7",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000008": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 8",
                            "logState": "error"
                        },
                        "100000009": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 9",
                            "logState": "incomplete"
                        },
                        "100000010": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 10",
                            "logState": "virgin"
                        },
                        "100000011": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 11",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000012": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 12",
                            "logState": "error"
                        },
                        "100000013": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 13",
                            "logState": "incomplete"
                        },
                        "100000014": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 14",
                            "logState": "virgin"
                        },
                        "100000015": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 15",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000016": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 16",
                            "logState": "error"
                        },
                        "100000017": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 17",
                            "logState": "incomplete"
                        },
                        "100000018": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 18",
                            "logState": "virgin"
                        },
                        "100000019": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 19",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000020": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 20",
                            "logState": "error"
                        },
                        "100000021": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 21",
                            "logState": "incomplete"
                        },
                        "100000022": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 22",
                            "logState": "virgin"
                        },
                        "100000023": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 23",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000024": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 24",
                            "logState": "error"
                        },
                        "100000025": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 25",
                            "logState": "incomplete"
                        },
                        "100000026": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 26",
                            "logState": "virgin"
                        },
                        "100000027": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 27",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000028": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 28",
                            "logState": "error"
                        },
                        "100000029": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 29",
                            "logState": "incomplete"
                        },
                        "100000030": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 30",
                            "logState": "virgin"
                        },
                        "100000031": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 31",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000032": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 32",
                            "logState": "error"
                        },
                        "100000033": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 33",
                            "logState": "incomplete"
                        },
                        "100000034": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 34",
                            "logState": "virgin"
                        },
                        "100000035": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 35",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000036": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 36",
                            "logState": "error"
                        },
                        "100000037": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 37",
                            "logState": "incomplete"
                        },
                        "100000038": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 38",
                            "logState": "virgin"
                        },
                        "100000039": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 39",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000040": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 40",
                            "logState": "error"
                        },
                        "100000041": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 41",
                            "logState": "incomplete"
                        },
                        "100000042": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 42",
                            "logState": "virgin"
                        },
                        "100000043": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 43",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000044": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 44",
                            "logState": "error"
                        },
                        "100000045": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 45",
                            "logState": "incomplete"
                        },
                        "100000046": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 46",
                            "logState": "virgin"
                        },
                        "100000047": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 47",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000048": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 48",
                            "logState": "error"
                        },
                        "100000049": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 49",
                            "logState": "incomplete"
                        },
                        "100000050": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 50",
                            "logState": "virgin"
                        },
                        "100000051": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 51",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000052": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 52",
                            "logState": "error"
                        },
                        "100000053": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 53",
                            "logState": "incomplete"
                        },
                        "100000054": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 54",
                            "logState": "virgin"
                        },
                        "100000055": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 55",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000056": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 56",
                            "logState": "error"
                        },
                        "100000057": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 57",
                            "logState": "incomplete"
                        },
                        "100000058": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 58",
                            "logState": "virgin"
                        },
                        "100000059": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 59",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000060": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 60",
                            "logState": "error"
                        },
                        "100000061": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 61",
                            "logState": "incomplete"
                        },
                        "100000062": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 62",
                            "logState": "virgin"
                        },
                        "100000063": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 63",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000064": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 64",
                            "logState": "error"
                        },
                        "100000065": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 65",
                            "logState": "incomplete"
                        },
                        "100000066": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 66",
                            "logState": "virgin"
                        },
                        "100000067": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 67",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000068": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 68",
                            "logState": "error"
                        },
                        "100000069": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 69",
                            "logState": "incomplete"
                        },
                        "100000070": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 70",
                            "logState": "virgin"
                        },
                        "100000071": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 71",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000072": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 72",
                            "logState": "error"
                        },
                        "100000073": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 73",
                            "logState": "incomplete"
                        },
                        "100000074": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 74",
                            "logState": "virgin"
                        },
                        "100000075": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 75",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000076": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 76",
                            "logState": "error"
                        },
                        "100000077": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 77",
                            "logState": "incomplete"
                        },
                        "100000078": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 78",
                            "logState": "virgin"
                        },
                        "100000079": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 79",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000080": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 80",
                            "logState": "error"
                        },
                        "100000081": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 81",
                            "logState": "incomplete"
                        },
                        "100000082": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 82",
                            "logState": "virgin"
                        },
                        "100000083": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 83",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000084": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 84",
                            "logState": "error"
                        },
                        "100000085": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 85",
                            "logState": "incomplete"
                        },
                        "100000086": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 86",
                            "logState": "virgin"
                        },
                        "100000087": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 87",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000088": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 88",
                            "logState": "error"
                        },
                        "100000089": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 89",
                            "logState": "incomplete"
                        },
                        "100000090": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 90",
                            "logState": "virgin"
                        },
                        "100000091": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 91",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000092": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 92",
                            "logState": "error"
                        },
                        "100000093": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 93",
                            "logState": "incomplete"
                        },
                        "100000094": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 94",
                            "logState": "virgin"
                        },
                        "100000095": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 95",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000096": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 96",
                            "logState": "error"
                        },
                        "100000097": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 97",
                            "logState": "incomplete"
                        },
                        "100000098": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 98",
                            "logState": "virgin"
                        },
                        "100000099": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 99",
                            "logState": "submitted",
                            "removeStamps": ["F&M Manager"],
                            "addStamps": ["Prod Sup", "Maint Sup"]
                        },
                        "100000100": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 100",
                            "logState": "error"
                        },
                        "100000101": {
                            "logParentId": "PastIssuesPanel",
                            "pmName": "checklist item 101",
                            "logState": "error"
                        }

                    }
                }
            </script>

            <style>
                :root {
                    --UWhitespace: 0.5em;
                    --UFontSize: calc(var(--UWhitespace) * 1.5);
                    --ChecklistButtonWidth: calc(100vw / 5); /*5 so there is room for the 'special' (> monthly) checklists column*/
                    --ChecklistButtonHeight: 50px;
                }

                /* ============= status board log classes =============== */

                .button-and-stamps-container {
                    display: inline-block;
                    border: 2px solid black;
                }

                .ChecklistButton {
                    height: var(--ChecklistButtonHeight);
                    text-overflow: ellipsis;
                }

                .error {
                    background: red;
                }

                .virgin {
                    background: pink;
                }

                .incomplete {
                    background: red;
                }

                .submitted {
                    /* green and blue candy cane design */
                    background: repeating-linear-gradient(60deg, #33CC33, #33CC33 10px, #ADD8E6, #ADD8E6 20px);
                }

                .completed {
                    /* green */
                    background: #33CC33;
                }

                .icon-panel {
                    display: flex;
                    align-items: center;
                    cursor: pointer;
                }

                .stamp-icon {
                    width: calc(var(--ChecklistButtonHeight) * .5);
                    height: calc(var(--ChecklistButtonHeight) * .5);
                    background-size: cover;
                    background-position: center;
                }

                .icon-fm-manager {
                    background-image: url(../Color/wrench-fill.png);
                }

                .icon-qshe-manager {
                    background-image: url(../Color/list-checks-fill.png);
                }

                .icon-prod-sup {
                    background-image: url(../Color/factory-fill.png);
                }

                .icon-maint-sup {
                    background-image: url(../Color/pipe-wrench-fill.png);
                }

                /* ? section */

                .SectionPanel {
                    margin: var(--UWhitespace) 0;
                    display: flex;
                    flex-direction: column;
                    gap: var(--UWhitespace);
                }

                .interval-shift-section {
                    display: grid;
                    grid-template-columns: 1fr;
                }

                    .interval-shift-section.has-logs > .interval-shift-no-logs-message {
                        display: none;
                    }

                .SectionLabel {
                    font-size: calc(var(--UFontSize) * 2);
                    font-weight: bold;
                }

                .interval-shift-section-label {
                    font-size: calc(var(--UFontSize) * 1.75);
                }

                .interval-shift-no-logs-message {
                    font-style: italic;
                    color: gray;
                    font-size: calc(var(--UFontSize)* 2);
                }

                /* ======== hide sati header and footer ============= */
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

                .hyperlink {
                    text-decoration: underline;
                    color: blue;
                    cursor: pointer;
                }

                @media (min-width: 601px) and (orientation: portrait) { /*tablets in portrait mode*/
                    .interval-shift-section {
                        grid-template-columns: 1fr 1fr;
                    }
                }

                @media (min-width: 601px) and (orientation: landscape) { /*tablets in landscape mode*/
                    .CurrentLogsPanel {
                        display: flex;
                        justify-content: space-around;
                    }

                    .interval-shift-no-logs-message {
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
                    .MonthlyLogsPanel .interval-shift-section {
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

            <sati-full-screen></sati-full-screen>
            <sati-spinner id="status-board-spinner"></sati-spinner>

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
                        <span id="overdue-logs-build-more-hyperlink" class="hyperlink">More...</span>
                        <div class="dots-spinner">
                            <span></span>
                            <span></span>
                            <span></span>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel runat="server">
                    <div class="PageHeader">
                        <asp:Label ID="WhereLabel" CssClass="interval-shift-section-label" runat="server" />

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
                                <asp:Label runat="server" Text="Users" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="OneTimeUsersPanel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="OneTimeUsersNoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="D1" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="OneTimeD1Panel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="OneTimeD1NoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="N1" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="OneTimeN1Panel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="OneTimeN1NoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="D2" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="OneTimeD2Panel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="OneTimeD2NoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="N2" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="OneTimeN2Panel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="OneTimeN2NoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="Days (M-F)" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="OneTimeMFShiftPanel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="OneTimeMFShiftNoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                        </asp:Panel>
                        <asp:Panel CssClass="SectionPanel" ID="DailyLogsPanel" runat="server">
                            <asp:Label runat="server" Text="Daily Logs" CssClass="SectionLabel"></asp:Label>
                            <div>
                                <asp:Label runat="server" Text="Day Shift" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="DailyDayShiftPanel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="DailyDayShiftNoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="Night Shift" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="DailyNightShiftPanel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="DailyNightShiftNoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="Days (M-F)" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="DailyMFShiftPanel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="DailyMFShiftNoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                        </asp:Panel>
                        <asp:Panel Style="margin: var(--UWhitespace) 0" ID="WeeklyLogsPanel" runat="server">
                            <asp:Label runat="server" Text="Weekly Logs" CssClass="SectionLabel"></asp:Label>

                            <div>
                                <asp:Label runat="server" Text="Users" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="WeeklyUsersPanel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="WeeklyUsersNoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="D1" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="WeeklyD1Panel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="WeeklyD1NoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="N1" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="WeeklyN1Panel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="WeeklyN1NoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="D2" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="WeeklyD2Panel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="WeeklyD2NoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="N2" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="WeeklyN2Panel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="WeeklyN2NoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="Days (M-F)" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="WeeklyMFShiftPanel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="WeeklyMFShiftNoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                        </asp:Panel>
                        <asp:Panel Style="margin: var(--UWhitespace) 0" ID="MonthlyLogsPanel" CssClass="MonthlyLogsPanel" runat="server">
                            <asp:Label runat="server" Text="Monthly Logs" CssClass="SectionLabel"></asp:Label>

                            <div>
                                <asp:Label runat="server" Text="Users" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="MonthlyUsersPanel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="MonthlyUsersNoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="D1" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="MonthlyD1Panel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="MonthlyD1NoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="N1" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="MonthlyN1Panel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="MonthlyN1NoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="D2" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="MonthlyD2Panel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="MonthlyD2NoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="N2" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="MonthlyN2Panel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="MonthlyN2NoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="Days (M-F)" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="MonthlyMFShiftPanel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="MonthlyMFShiftNoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                        </asp:Panel>

                        <%--<asp:Panel Style="margin: var(--UWhitespace) 0" ID="SpecialLogsPanel" runat="server" Visible="False">--%>
                        <asp:Panel Style="margin: var(--UWhitespace) 0" ID="SpecialLogsPanel" runat="server">
                            <asp:Label runat="server" Text="Special Logs" CssClass="SectionLabel"></asp:Label>

                            <div>
                                <asp:Label runat="server" Text="Quarterly" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="QuarterlyPanel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="QuarterlyNoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="Bi-Annual" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="BiAnnualPanel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="BiAnnualNoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="1 Year" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="OneYearPanel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="OneYearNoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="2 Year" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="TwoYearPanel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="TwoYearNoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="3 Year" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="ThreeYearPanel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="ThreeYearNoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="4 Year" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="FourYearPanel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="FourYearNoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                            <div>
                                <asp:Label runat="server" Text="5 Year" CssClass="interval-shift-section-label"></asp:Label>
                                <asp:Panel runat="server" ID="FiveYearPanel" CssClass="interval-shift-section">
                                    <asp:Label runat="server" ID="FiveYearNoneLabel" Text="NONE AT THIS TIME" CssClass="interval-shift-no-logs-message"></asp:Label>
                                </asp:Panel>
                            </div>

                        </asp:Panel>

                    </asp:Panel>

                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
