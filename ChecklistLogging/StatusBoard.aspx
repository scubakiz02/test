<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="StatusBoard.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Timer runat="server" OnTick="PageRefresh_OnTick" Interval="60000"></asp:Timer>
    <%--<asp:Timer runat="server" OnTick="PageRefresh_OnTick" Interval="10000"></asp:Timer>--%>
    <asp:UpdatePanel ID="UpdatePane" runat="server">

        <ContentTemplate>
            <script type="text/javascript">
                function redirect(url) {
                    window.location.href = url + this.id
                }
            </script>

            <style>
                :root {
                    --UWhitespace: 0.5em;
                    --UFontSize: calc(var(--UWhitespace) * 1.5);
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
                    height: 50px;
                    text-overflow: ellipsis;
                }

                .ColorCodingMessages {
                    display: flex;
                    width: 100%;
                    font-size: var(--UFontSize);
                    text-wrap: nowrap;
                    align-items: baseline;
                    justify-content: space-between;
                    flex-direction: column
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

                @media (min-width: 601px) {
                    .ColorCodingMessages {
                        font-size: calc(var(--UFontSize)* 1.25);
                    }

                    .DepartmentMenu {
                        font-size: calc(var(--UFontSize) * 1.5);
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
                        justify-content: space-between;
                    }

                    .ItalicizeLabel {
                        font-size: calc(var(--UFontSize) * 1.5);
                    }

                    .ChecklistButton {
                        max-width: calc(100vw / 5); /*5 so there is room for the 'special' (> monthly) checklists column*/
                        text-overflow: ellipsis;
                    }

                    .DepAndViewMenus {
                        flex-direction: row;
                    }

                    .ColorCodingMessages {
                        justify-content: normal;
                        flex-direction: row;
                    }
                }

                @media (min-width: 1920px) {
                    :root {
                        --UFontSize: calc(var(--UWhitespace)* 2);
                    }

                    .ChecklistButton {
                        font-size: var(--UFontSize);
                        height: auto;
                        padding: calc(var(--UWhitespace)* 2);
                    }


                    .TimeTravelCalendar td {
                        padding: .25em;
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

            <%--style="display: flex; justify-content: space-between;"--%>
            <div style="display: flex; flex-direction: column-reverse;">

                <asp:Panel ID="AdminPanel" runat="server" Visible="False" Style="display: flex; flex-direction: column; gap: var(--UWhitespace);">
                    <asp:Label runat="server" CssClass="SectionLabel" Text="Past Issues"></asp:Label>

                    <div>
                        <asp:Panel runat="server" Style="float: left; margin-right: var(--UWhitespace);" Visible="False">
                            <asp:Label ID="TimeTravelLabel" CssClass="" runat="server" Text=""></asp:Label>
                            <asp:Calendar runat="server" ID="TimeTravelCalendar" CssClass="TimeTravelCalendar" OnDayRender="TimeTravelCalendar_OnDayRender" OnSelectionChanged="TimeTravelCalendar_OnSelectionChanged"></asp:Calendar>
                        </asp:Panel>
                        <asp:Panel runat="server">
                            <%--Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); border: 2px solid black;"--%>
                            <asp:Panel ID="PastIssuesPanel" runat="server" Style="">
                            </asp:Panel>
                        </asp:Panel>
                    </div>
                </asp:Panel>

                <asp:Panel runat="server">
                    <div style="display: flex; justify-content: space-between">
                        <asp:Panel ID="ColorCodingMessages" CssClass="ColorCodingMessages" runat="server" Style="">
                            <div style="display: flex; align-items: center; justify-content: center;">
                                <div style="display: flex; align-items: center; justify-content: center;">
                                    <svg class="ColoredSquares" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1 1">
                                        <path d="m0,0h1v1H0" fill="#FFC0CB" />
                                    </svg>
                                    <p style="">= NOT STARTED</p>
                                </div>
                                <div style="display: flex; align-items: center; justify-content: center; margin: 0 10px;">
                                    <svg class="ColoredSquares" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1 1">
                                        <path d="m0,0h1v1H0" fill="#FF0000" />
                                    </svg>
                                    <p style="">= NEEDS COMPLETION</p>
                                </div>
                            </div>
                            <div style="display: flex; align-items: center; justify-content: center;">
                                <div style="display: flex; align-items: center; justify-content: center;">
                                    <div class="ColoredSquares" style="padding: 0; margin: 0 5px; background: repeating-linear-gradient(60deg, #33cc33, #33cc33 10px, #ADD8E6, #ADD8E6 20px);">
                                    </div>
                                    <p style="">= COMPLETE & NEEDS STAMP</p>
                                </div>
                                <div style="display: flex; align-items: center; justify-content: center; margin: 0 10px;">
                                    <svg class="ColoredSquares" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1 1">
                                        <path d="m0,0h1v1H0" fill="#33CC33" />
                                    </svg>
                                    <p style="">= COMPLETE</p>
                                </div>
                            </div>
                        </asp:Panel>

                        <asp:Label ID="WhereLabel" Style="padding: var(--UWhitespace);" runat="server" />
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
                        <asp:Panel Style="margin: var(--UWhitespace) 0" ID="MonthlyLogsPanel" runat="server">
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

                        <asp:Panel Style="margin: var(--UWhitespace) 0" ID="SpecialLogsPanel" runat="server" Visible="False">
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

