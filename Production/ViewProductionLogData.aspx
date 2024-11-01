<%@ Page Title="Production Log Viewer" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ViewProductionLogData.aspx.vb" Inherits="Production_ViewProductionLogData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="ProLogView" runat="server">
        <ContentTemplate>
            <asp:Panel ID="ProductionLogPanel" runat="server" Width="1000px">
                <table>
                    <tr>
                        <td style="align-content: center; height: 80px; width: 1006px;">
                            <asp:Panel ID="ProLogTitlePanel" runat="server" Width="1000px" HorizontalAlign="Center">
                                <asp:Label ID="Label1" runat="server" Text="24/7 Production Log Viewer" Font-Bold="True" Font-Size="XX-Large"></asp:Label>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 1000px">
                            <asp:Panel ID="ProLogoptionPanel" runat="server" BackColor="LightGray">
                                <table style="width: 1000px; height: 500px">
                                    <tr style="align-content: center">
                                        <td style="width: 400px; padding-left: 30px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Panel ID="DatePanel" runat="server">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="StartDate1" runat="server" Text="SELECT A" Font-Bold="true" Font-Size="Larger"></asp:Label>
                                                                        <asp:Label ID="StartDate2" runat="server" Text=" START " Font-Bold="true" Font-Size="Larger" ForeColor="#507CD1"></asp:Label>
                                                                        <asp:Label ID="StartDate3" runat="server" Text="DATE TO VIEW" Font-Bold="true" Font-Size="Larger"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Calendar ID="DateTextBox" runat="server" BackColor="White" BorderColor="Black" DayNameFormat="Shortest" Font-Names="Times New Roman" Font-Size="10pt" ForeColor="Black" Height="200px" NextPrevFormat="FullMonth" TitleFormat="Month" Width="400px" OnSelectionChanged="DateTextBox_SelectionChanged">
                                                                            <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" ForeColor="#333333" Height="10pt" />
                                                                            <DayStyle Width="14%" />
                                                                            <NextPrevStyle Font-Size="8pt" ForeColor="White" />
                                                                            <OtherMonthDayStyle ForeColor="#999999" />
                                                                            <SelectedDayStyle BackColor="#507CD1" ForeColor="White" />
                                                                            <SelectorStyle BackColor="#CCCCCC" Font-Bold="True" Font-Names="Verdana" Font-Size="8pt" ForeColor="#333333" Width="1%" />
                                                                            <TitleStyle BackColor="Black" Font-Bold="True" Font-Size="13pt" ForeColor="White" Height="14pt" />
                                                                            <TodayDayStyle BackColor="#EFF3FB" />
                                                                        </asp:Calendar>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="EndDate1" runat="server" Text="SELECT A" Font-Bold="true" Font-Size="Larger"></asp:Label>
                                                                        <asp:Label ID="EndDate2" runat="server" Text=" END " Font-Bold="true" Font-Size="Larger" ForeColor="#507CD1"></asp:Label>
                                                                        <asp:Label ID="EndDate3" runat="server" Text="DATE TO VIEW" Font-Bold="true" Font-Size="Larger"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Calendar ID="DateTextBox2" runat="server" BackColor="White" BorderColor="Black" DayNameFormat="Shortest" Font-Names="Times New Roman" Font-Size="10pt" ForeColor="Black" Height="200px" NextPrevFormat="FullMonth" TitleFormat="Month" Width="400px" OnSelectionChanged="DateTextBox_SelectionChanged">
                                                                            <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" ForeColor="#333333" Height="10pt" />
                                                                            <DayStyle Width="14%" />
                                                                            <NextPrevStyle Font-Size="8pt" ForeColor="White" />
                                                                            <OtherMonthDayStyle ForeColor="#999999" />
                                                                            <SelectedDayStyle BackColor="#507CD1" ForeColor="White" />
                                                                            <SelectorStyle BackColor="#CCCCCC" Font-Bold="True" Font-Names="Verdana" Font-Size="8pt" ForeColor="#333333" Width="1%" />
                                                                            <TitleStyle BackColor="Black" Font-Bold="True" Font-Size="13pt" ForeColor="White" Height="14pt" />
                                                                            <TodayDayStyle BackColor="#EFF3FB" />
                                                                        </asp:Calendar>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <br />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 150px">
                                            <table style="height: 200px">
                                                <tr>
                                                    <td style="height: 100px">
                                                        <asp:Panel ID="ShiftPanel" runat="server" align-content="baseline">
                                                            <asp:Label ID="Shift" runat="server" Text="SHIFT:" Font-Size="Larger" Font-Bold="true"></asp:Label>
                                                            <asp:CheckBoxList ID="ShiftCheckList" runat="server" AutoPostBack="True" DataSourceID="SqlDataSourceShiftCheckList" DataTextField="Expr1" DataValueField="Expr1" Width="50px"></asp:CheckBoxList>
                                                            &nbsp;&nbsp; &nbsp;&nbsp;
                                                            <asp:SqlDataSource ID="SqlDataSourceShiftCheckList" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT DISTINCT SUBSTRING(Shift, 2, 1) AS Expr1 FROM T_ProLogLots WHERE (SUBSTRING(Shift, 2, 1) = '1' OR SUBSTRING(Shift, 2, 1) = '2') AND (ReportDate BETWEEN @StartDate AND @EndDate) ORDER BY Expr1">
                                                                <SelectParameters>
                                                                    <asp:ControlParameter ControlID="DateTextBox" Name="StartDate" PropertyName="SelectedDate" />
                                                                    <asp:ControlParameter ControlID="DateTextBox2" Name="EndDate" PropertyName="SelectedDate" />
                                                                </SelectParameters>
                                                            </asp:SqlDataSource>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 100px">
                                                        <asp:Panel ID="DTLabelPanel" runat="server" Height="50px">
                                                            <asp:Label ID="DTLabel" runat="server" Text="Display Down Time? (Y/N)" Width="130px" Height="25px" Font-Bold="true" Font-Size="Larger"></asp:Label>
                                                            &nbsp;&nbsp; &nbsp;&nbsp;
                                                            <asp:Panel ID="DTCBPanel" runat="server">
                                                                <asp:CheckBox ID="DTCheckBox" runat="server" Width="16px" Height="16px" AutoPostBack="true" />
                                                            </asp:Panel>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 250px; padding-left: 20px">
                                            <asp:Label ID="Stage" runat="server" Text="OPERATION:" Font-Bold="true" Font-Size="Larger"></asp:Label>
                                            <asp:CheckBoxList ID="DeparmentCheckList" runat="server" DataSourceID="SqlDataSourceDepartmentCheckLisr" DataTextField="Department" DataValueField="Department" AutoPostBack="True" Height="30px" Width="300px"></asp:CheckBoxList>
                                            <asp:SqlDataSource ID="SqlDataSourceDepartmentCheckLisr" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT DISTINCT Department FROM T_ProLogLots WHERE (ReportDate BETWEEN @StartDate AND @EndDate) ORDER BY Department">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="DateTextBox" Name="StartDate" PropertyName="SelectedDate" />
                                                    <asp:ControlParameter ControlID="DateTextBox2" Name="EndDate" PropertyName="SelectedDate" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="LogPanel" runat="server">
                                <asp:GridView ID="LogViewer" runat="server" AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSourceLogViewer" ForeColor="#333333" GridLines="None" Width="1000px">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="ReportDate" HeaderText="ReportDate" SortExpression="ReportDate" DataFormatString="{0:d}">
                                            <HeaderStyle Width="70px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Shift" HeaderText="Shift" SortExpression="Shift">
                                            <HeaderStyle Width="30px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department">
                                            <HeaderStyle Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="LotNumber" HeaderText="LotNumber" SortExpression="LotNumber">
                                            <HeaderStyle Width="125px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="QtyCompleat" HeaderText="QtyCompleat" SortExpression="QtyCompleat">
                                            <HeaderStyle Width="125px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="QtyPass" HeaderText="QtyPass" SortExpression="QtyPass">
                                            <HeaderStyle Width="100px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="StartTime" HeaderText="StartTime" SortExpression="StartTime" DataFormatString="{0:g}">
                                            <HeaderStyle Width="160px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EndTime" HeaderText="EndTime" SortExpression="EndTime" DataFormatString="{0:g}">
                                            <HeaderStyle Width="160px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Hours" HeaderText="Hours" ReadOnly="True" SortExpression="Hours" DataFormatString="{0:0.0}">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Op" HeaderText="Op" SortExpression="Op">
                                            <HeaderStyle Width="110px" />
                                        </asp:BoundField>
                                    </Columns>
                                    <EditRowStyle BackColor="#2461BF" />
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSourceLogViewer" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT ReportDate, Shift, Department, LotNumber, QtyCompleat, QtyPass, StartTime, EndTime, Op, (DATEDIFF(ss, StartTime, EndTime) * 1.0) / (60 * 60) AS Hours FROM T_ProLogLots WHERE (SUBSTRING(Shift, 2, 1) IN (@SCL)) AND (ReportDate BETWEEN @StartDate AND @EndDate) AND (Department IN (@Department)) ORDER BY ReportDate, Shift, Department">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ShiftCheckList" Name="SCL" PropertyName="SelectedValue" />
                                        <asp:ControlParameter ControlID="DateTextBox" Name="StartDate" PropertyName="SelectedDate" />
                                        <asp:ControlParameter ControlID="DateTextBox2" Name="EndDate" PropertyName="SelectedDate" />
                                        <asp:ControlParameter ControlID="DeparmentCheckList" Name="Department" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </asp:Panel>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="EventPanel" runat="server">
                                <asp:GridView ID="DTViewer" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" DataSourceID="SqlDataSourceDTViewer" ForeColor="Black" GridLines="none" Width="1000px">
                                    <AlternatingRowStyle BackColor="#CCCCCC" />
                                    <Columns>
                                        <asp:BoundField DataField="ReportDate" HeaderText="ReportDate" SortExpression="ReportDate" DataFormatString="{0:d}">
                                            <HeaderStyle Width="70px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Shift" HeaderText="Shift" SortExpression="Shift">
                                            <HeaderStyle Width="30px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department">
                                            <HeaderStyle Width="110px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Event" HeaderText="Event" SortExpression="Event" ItemStyle-HorizontalAlign="Left">
                                            <HeaderStyle Width="375px" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="StartTime" HeaderText="StartTime" SortExpression="StartTime" DataFormatString="{0:g}">
                                            <HeaderStyle Width="160px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="EndTime" HeaderText="EndTime" SortExpression="EndTime" DataFormatString="{0:g}">
                                            <HeaderStyle Width="160px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Hours" HeaderText="Hours" SortExpression="Hours" DataFormatString="{0:0.0}"/><asp:BoundField />
                                        <asp:BoundField DataField="Op" HeaderText="Op" SortExpression="Op">
                                            <HeaderStyle Width="110px" />
                                        </asp:BoundField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" />
                                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#808080" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#383838" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSourceDTViewer" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT ReportDate, Shift, Department, Event, StartTime, EndTime, Op, (DATEDIFF(ss, StartTime, EndTime) * 1.0) / (60 * 60) AS Hours FROM T_ProLogDT WHERE (ReportDate BETWEEN @StartDate AND @EndDate) AND (SUBSTRING(Shift, 2, 1) IN (@Shifts)) AND (Department IN (@Departments)) ORDER BY ReportDate, Shift, Department">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="DateTextBox" Name="StartDate" PropertyName="SelectedDate" />
                                        <asp:ControlParameter ControlID="DateTextBox2" Name="EndDate" PropertyName="SelectedDate" />
                                        <asp:ControlParameter ControlID="ShiftCheckList" Name="Shifts" PropertyName="SelectedValue" />
                                        <asp:ControlParameter ControlID="DeparmentCheckList" Name="Departments" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

