<%@ Page Title="Production Logger" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="EnterProductionLog.aspx.vb" MaintainScrollPositionOnPostback="false" Inherits="Production_EnterProductionLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="ProductionLogPanel" runat="server" Width="1000px">
                <table>
                    <tr>
                        <td style="align-content: center; height: 40px; width: 1006px;">
                            <asp:Panel ID="ProLogTitlePanel" runat="server" Width="1000px" HorizontalAlign="Center">
                                <asp:Label ID="TitleLabel" runat="server" Text="24/7 Production Log" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 1000px; height: 300px;">
                            <asp:Panel ID="ProLogoptionPanel" runat="server" BackColor="LightGray" Width="1003px" Height="297px">
                                <table border="0">
                                    <tr>
                                        <td style="width: 500px; height: 250px;">
                                            <table>
                                                <tr>
                                                    <td style="padding-top: 10px; padding-left: 50px; width: 460px; height: 60px">
                                                        <asp:Label ID="Shift" runat="server" Text="SELECT YOUR SHIFT:   " Width="240px" Font-Bold="True"></asp:Label>
                                                        &nbsp;
                                                        <asp:DropDownList ID="ShiftDropDown" runat="server" AutoPostBack="True" Style="margin-left: 0px" Width="165px">
                                                            <asp:ListItem>Select A Shift...</asp:ListItem>
                                                            <asp:ListItem>D1</asp:ListItem>
                                                            <asp:ListItem>N1</asp:ListItem>
                                                            <asp:ListItem>D2</asp:ListItem>
                                                            <asp:ListItem>N2</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 50px; width: 460px; height: 60px">
                                                        <asp:Label ID="Stage" runat="server" Text="SELECT YOUR DEPARTMENT:   " Width="240px" Font-Bold="true"></asp:Label>
                                                        &nbsp;
                                                        <asp:DropDownList ID="DepartmentDropDown" runat="server" Width="165px" AutoPostBack="True">
                                                            <asp:ListItem>Select A Department...</asp:ListItem>
                                                            <asp:ListItem>INC/INSPECT</asp:ListItem>
                                                            <asp:ListItem>T7 RREAD</asp:ListItem>
                                                            <asp:ListItem>SCRIBE Sort</asp:ListItem>
                                                            <asp:ListItem>STRIP/ETCH 1</asp:ListItem>
                                                            <asp:ListItem>STRIP/ETCH 2</asp:ListItem>
                                                            <asp:ListItem>STRIP/ETCH 3</asp:ListItem>
                                                            <asp:ListItem>STRIP/ETCH 4</asp:ListItem>
                                                            <asp:ListItem>SCRIBE</asp:ListItem>
                                                            <asp:ListItem>Grind</asp:ListItem>
                                                            <asp:ListItem>Type Sort</asp:ListItem>
                                                            <asp:ListItem>200mm Presort</asp:ListItem>
                                                            <asp:ListItem>200mm Presort Second Pass</asp:ListItem>
                                                            <asp:ListItem>300 Presort P3</asp:ListItem>
                                                            <asp:ListItem>300 Presort GIGASORT</asp:ListItem>
                                                            <asp:ListItem>300 Presort GIGASORT Second Pass</asp:ListItem>
                                                            <asp:ListItem>300 Presort GIGASORT Third Pass</asp:ListItem>
                                                            <asp:ListItem>POLISH</asp:ListItem>
                                                            <asp:ListItem>DSP</asp:ListItem>
                                                            <asp:ListItem>CMP</asp:ListItem>
                                                            <asp:ListItem>Tel Bench</asp:ListItem>
                                                            <asp:ListItem>CLEANROOM 1</asp:ListItem>
                                                            <asp:ListItem>CLEANROOM 2</asp:ListItem>
                                                            <asp:ListItem>CLEANROOM 3</asp:ListItem>                                                            
                                                            <asp:ListItem>LASER INSPECTION 1</asp:ListItem>
                                                            <asp:ListItem>LASER INSPECTION 2</asp:ListItem>
                                                            <asp:ListItem>LASER INSPECTION 3</asp:ListItem>
                                                           
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 50px; width: 460px; height: 100px">
                                                        <asp:Label ID="Key" runat="server" Text="Helpful Infomation"></asp:Label>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <table style="width: 400px; height: 100px;">
                                                                        <tr>
                                                                            <td style="width: 30px">
                                                                                <asp:Label ID="LBS" runat="server" Text="&#9632;" Font-Size="X-Large" ForeColor="#507CD1" Font-Bold="true"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="LBT" runat="server" Text="LIGHT Blue Table: Previous Shift Production Log."></asp:Label> 
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="DBS" runat="server" Text="&#9632;" Font-Size="X-Large" ForeColor="#000066" Font-Bold="true"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="DBT" runat="server" Text="DARK Blue Table : Current Shift Production Log."></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="LGS" runat="server" Text="&#9632;" Font-Size="X-Large" ForeColor="#808080" Font-Bold="true"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="LGT" runat="server" Text="LIGHT Gray Table: Previous Shift Production Log."></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="DGS" runat="server" Text="&#9632;" Font-Size="X-Large" ForeColor="Black" Font-Bold="true"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="DGT" runat="server" Text="DARK Gray Table: Current Shift Down Time Log."></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 492px; height: 250px; align-content: center">
                                            <table style="width: 488px; height: 274px;">
                                                <tr>
                                                    <td style="text-align: center; padding-left: 20px; height: 43px;">
                                                        <asp:Label ID="CalendarText" runat="server" Text="SELECT THE DAY WHICH YOUR SHIFT START ON" Font-Bold="true"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 100px">
                                                        <asp:Calendar ID="DateTextBox" runat="server" AutoPostBack="True" Width="300px" Height="195px" BackColor="White">
                                                            <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" ForeColor="#333333" Height="10pt" />
                                                            <DayStyle Width="14%" />
                                                            <NextPrevStyle Font-Size="8pt" ForeColor="White" />
                                                            <OtherMonthDayStyle ForeColor="#999999" />
                                                            <SelectedDayStyle BackColor="#507CD1" ForeColor="White" />
                                                            <SelectorStyle BackColor="White" Font-Bold="True" Font-Names="Verdana" Font-Size="8pt" ForeColor="#333333" Width="1%" />
                                                            <TitleStyle BackColor="Black" Font-Bold="True" Font-Size="13pt" ForeColor="White" Height="14pt" />
                                                            <TodayDayStyle BackColor="#EFF3FB" />
                                                        </asp:Calendar>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 1000px">
                            <asp:Panel ID="ShiftPanel" runat="server" Width="1000px" HorizontalAlign="Center">
                                <table>
                                    <tr>
                                        <td style="width: 1000px; padding-top: 10px">
                                            <asp:Panel ID="TaskPanel" runat="server" BackColor="LightGray" Width="1003px" Height="25">
                                                <table>
                                                    <tr>
                                                        <td style="width: 1000px; text-align: center">
                                                            <asp:Label ID="Label2" runat="server" Text="PREVIOUS SHIFT'S TASKS" ForeColor="#507CD1" Font-Bold="true" Font-Size="Larger"></asp:Label>
                                                            <asp:Label ID="Label5" runat="server" Text="" ForeColor="#507CD1" Font-Bold="true" Width="100px"></asp:Label>
                                                            <asp:Label ID="Label1" runat="server" Text="CURRENT SHIFT'S TASKS" ForeColor="#000066" Font-Bold="true" Font-Size="Larger"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1000px; padding-top: 10px">
                                            <asp:GridView ID="ProLogTablePre" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="Key" DataSourceID="SqlDataSourceProLogLotsPre" ForeColor="#333333" AutoPostBack="True" ShowFooter="True" Width="997px" ShowHeaderWhenEmpty="True" GridLines="None">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:BoundField DataField="Key" HeaderText="Key" SortExpression="Key" ReadOnly="True" InsertVisible="False"></asp:BoundField>
                                                    <asp:BoundField DataField="Shift" HeaderText="Shift" SortExpression="Shift"></asp:BoundField>
                                                    <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" />
                                                    <asp:BoundField DataField="LotNumber" HeaderText="LotNumber" SortExpression="LotNumber" />
                                                    <asp:BoundField DataField="QtyCompleat" HeaderText="QtyCompleat" SortExpression="QtyCompleat" />
                                                    <asp:BoundField DataField="QtyPass" HeaderText="QtyPass" SortExpression="QtyPass" />
                                                    <asp:BoundField DataField="StartTime" HeaderText="StartTime" SortExpression="StartTime" DataFormatString="{0:g}"/>
                                                    <asp:BoundField DataField="EndTime" HeaderText="EndTime" SortExpression="EndTime" DataFormatString="{0:g}"/>
                                                    <asp:BoundField DataField="Hours" DataFormatString="{0:0.0}" HeaderText="Hours" ReadOnly="True" SortExpression="Hours" />
                                                    <asp:BoundField DataField="Op" HeaderText="Op" SortExpression="Op" />
                                                </Columns>
                                                <EditRowStyle BackColor="#2461BF" />
                                                <EmptyDataTemplate>
                                                    No Data Has Been Entered for This Shift
                                                </EmptyDataTemplate>
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#EFF3FB" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                            </asp:GridView>
                                            <br />
                                            <asp:GridView ID="ProLogTableCurrent" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="Key" DataSourceID="SqlDataSourceProLogLots" ForeColor="#000066" AutoPostBack="True" Width="997px" ShowHeaderWhenEmpty="True" GridLines="None">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:BoundField DataField="Key" HeaderText="Key" SortExpression="Key" ReadOnly="True" InsertVisible="False"></asp:BoundField>
                                                    <asp:BoundField DataField="Shift" HeaderText="Shift" SortExpression="Shift" ReadOnly="True"></asp:BoundField>
                                                    <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" ReadOnly="True"/>
                                                    <asp:BoundField DataField="LotNumber" HeaderText="LotNumber" SortExpression="LotNumber" ControlStyle-Width="100px"/>
                                                    <asp:BoundField DataField="QtyCompleat" HeaderText="QtyCompleat" SortExpression="QtyCompleat" ControlStyle-Width="75px"/>
                                                    <asp:BoundField DataField="QtyPass" HeaderText="QtyPass" SortExpression="QtyPass" ControlStyle-Width="75px"/>
                                                    <asp:BoundField DataField="StartTime" HeaderText="StartTime" SortExpression="StartTime" DataFormatString="{0:g}"/>
                                                    <asp:BoundField DataField="EndTime" HeaderText="EndTime" SortExpression="EndTime" DataFormatString="{0:g}"/>
                                                    <asp:BoundField DataField="Hours" HeaderText="Hours"  DataFormatString="{0:0.0}" ReadOnly="True" SortExpression="Hours" />
                                                    <asp:BoundField DataField="Op" HeaderText="Op" SortExpression="Op" ReadOnly="True"/>
                                                    <asp:CommandField DeleteText="Del" ShowDeleteButton="True" ShowEditButton="True" />
                                                </Columns>
                                                <EditRowStyle BackColor="#7C6F57" />
                                                <EmptyDataTemplate>
                                                    No Data Has Been Entered for This Shift
                                                </EmptyDataTemplate>
                                                <FooterStyle BackColor="#000066" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#000066" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#E3EAEB" HorizontalAlign="Center" ForeColor="Black"/>
                                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                                <SortedAscendingHeaderStyle BackColor="#246B61" />
                                                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                                <SortedDescendingHeaderStyle BackColor="#15524A" />
                                            </asp:GridView>
                                            <asp:SqlDataSource ID="SqlDataSourceProLogLotsPre" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" DeleteCommand="DELETE FROM [T_ProLogLots] WHERE [Key] = @Key" InsertCommand="INSERT INTO [T_ProLogLots] ([Shift], [Department], [QtyCompleat], [LotNumber], [QtyPass], [Op], [EndTime], [StartTime]) VALUES (@Shift, @Department, @QtyCompleat, @LotNumber, @QtyPass, @Op, @EndTime, @StartTime)" SelectCommand="SELECT [Key], Shift, Department, LotNumber, QtyCompleat, QtyPass, StartTime, EndTime, Op, (DATEDIFF(ss, StartTime, EndTime) * 1.0) / (60 * 60) AS Hours FROM T_ProLogLots WHERE (ReportDate = @ReportDate) AND (Shift = @Shift) AND (Department = @Department) ORDER BY StartTime" UpdateCommand="UPDATE [T_ProLogLots] SET [Shift] = @Shift, [Department] = @Department, [QtyCompleat] = @QtyCompleat, [LotNumber] = @LotNumber, [QtyPass] = @QtyPass, [Op] = @Op, [EndTime] = @EndTime, [StartTime] = @StartTime WHERE [Key] = @Key">
                                                <DeleteParameters>
                                                    <asp:Parameter Name="Key" Type="Int32" />
                                                </DeleteParameters>
                                                <InsertParameters>
                                                    <asp:Parameter Name="Shift" Type="String" />
                                                    <asp:Parameter Name="Department" Type="String" />
                                                    <asp:Parameter Name="QtyCompleat" Type="Int32" />
                                                    <asp:Parameter Name="LotNumber" Type="String" />
                                                    <asp:Parameter Name="QtyPass" Type="Int32" />
                                                    <asp:Parameter Name="Op" Type="String" />
                                                    <asp:Parameter Name="EndTime" Type="DateTime" />
                                                    <asp:Parameter Name="StartTime" Type="DateTime" />
                                                </InsertParameters>
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="DateTextBox" DbType="Date" Name="ReportDate" PropertyName="SelectedDate" />
                                                    <asp:ControlParameter ControlID="ShiftDropDown" Name="Shift" PropertyName="SelectedValue" />
                                                    <asp:ControlParameter ControlID="DepartmentDropDown" Name="Department" PropertyName="SelectedValue" Type="String" />
                                                </SelectParameters>
                                                <UpdateParameters>
                                                    <asp:Parameter Name="Shift" Type="String" />
                                                    <asp:Parameter Name="Department" Type="String" />
                                                    <asp:Parameter Name="QtyCompleat" Type="Int32" />
                                                    <asp:Parameter Name="LotNumber" Type="String" />
                                                    <asp:Parameter Name="QtyPass" Type="Int32" />
                                                    <asp:Parameter Name="Op" Type="String" />
                                                    <asp:Parameter Name="EndTime" Type="DateTime" />
                                                    <asp:Parameter Name="StartTime" Type="DateTime" />
                                                    <asp:Parameter Name="Key" Type="Int32" />
                                                </UpdateParameters>
                                            </asp:SqlDataSource>
                                            <asp:SqlDataSource ID="SqlDataSourceProLogLots" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                                                DeleteCommand="DELETE FROM [T_ProLogLots] WHERE [Key] = @original_Key"
                                                InsertCommand="INSERT INTO [T_ProLogLots] ([Shift], [Department], [LotNumber], [QtyCompleat], [QtyPass], [StartTime], [EndTime], [ReportDate], [Op]) VALUES (@Shift, @Department, @LotNumber, @QtyCompleat, @QtyPass, @StartTime, @EndTime, @ReportDate, @Op)"
                                                SelectCommand="SELECT [Key], Shift, Department, LotNumber, QtyCompleat, QtyPass, StartTime, EndTime, Op, (DATEDIFF(ss, StartTime, EndTime) * 1.0) / (60 * 60) AS Hours FROM T_ProLogLots WHERE (ReportDate = @ReportDate) AND (Shift = @Shift) AND (Department = @Department) ORDER BY StartTime"
                                                UpdateCommand="UPDATE [T_ProLogLots] SET [LotNumber] = @LotNumber, [QtyCompleat] = @QtyCompleat, [QtyPass] = @QtyPass, [StartTime] = @StartTime, [EndTime] = @EndTime WHERE [Key] = @original_Key" OldValuesParameterFormatString="original_{0}">
                                                <DeleteParameters>
                                                    <asp:Parameter Name="original_Key" Type="Int32" />
                                                </DeleteParameters>
                                                <InsertParameters>
                                                    <asp:Parameter Name="Shift" Type="String" />
                                                    <asp:Parameter Name="Department" Type="String" />
                                                    <asp:Parameter Name="LotNumber" Type="String" />
                                                    <asp:Parameter Name="QtyCompleat" Type="Int32" />
                                                    <asp:Parameter Name="QtyPass" Type="Int32" />
                                                    <asp:Parameter Name="StartTime" Type="DateTime" />
                                                    <asp:Parameter Name="EndTime" Type="DateTime" />
                                                    <asp:Parameter Name="ReportDate" Type="DateTime" />
                                                    <asp:Parameter Name="Op" Type="String" />
                                                </InsertParameters>
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="DateTextBox" Name="ReportDate" PropertyName="SelectedDate" DbType="Date" />
                                                    <asp:ControlParameter ControlID="ShiftDropDown" Name="Shift" PropertyName="SelectedValue" />
                                                    <asp:ControlParameter ControlID="DepartmentDropDown" Name="Department" PropertyName="SelectedValue" Type="String" />
                                                </SelectParameters>
                                                <UpdateParameters>
                                                    <asp:Parameter Name="LotNumber" Type="String" />
                                                    <asp:Parameter Name="QtyCompleat" Type="Int32" />
                                                    <asp:Parameter Name="QtyPass" Type="Int32" />

                                                    <asp:Parameter Name="StartTime" Type="DateTime" />
                                                    <asp:Parameter Name="EndTime" Type="DateTime" />
                                                    <asp:Parameter Name="original_Key" Type="Int32" />
                                                </UpdateParameters>
                                            </asp:SqlDataSource>

                                            <asp:Panel ID="AddNewRecord" runat="server" Width="997px" Height="70px" BackColor="#000066">
                                                <table style="width: 997px; height: 70px; background-color: #000066;">
                                                    <tr>
                                                        <td style="height: 35px; width: 250px">
                                                            <table style="width: 250px">
                                                                <tr>
                                                                    <td style="padding-left: 2px; width:135px">
                                                                        <asp:Button ID="CreateRecordButton" runat="server" Text="&#10003;  Create" Width="110px" BackColor="#00cc00" Font-Bold="true" />&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr style="justify-content:right">
                                                                    <td style="height: 10px; width: 135px;">
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="QualLabel" runat="server" Text="Qual. Lot?" ForeColor="White"></asp:Label>
                                                                        <asp:CheckBox ID="QualCheck" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="padding-left: 1px; width: 135px;">
                                                                        <asp:Button ID="CancelRecordButton" runat="server" Text="&#10008;  Cancel" Width="110px" BackColor="#ff0000" Height="23px" Font-Bold="true" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 120px">
                                                            <asp:TextBox ID="LN" runat="server" Width="100px" placeholder="Lot Number..."></asp:TextBox>
                                                        </td>
                                                        <td style="width: 105px">
                                                            <asp:TextBox ID="QC" runat="server" Width="85px" placeholder="Qty..."></asp:TextBox>
                                                        </td>
                                                        <td style="width: 105px">
                                                            <asp:TextBox ID="QP" runat="server" Width="79px" placeholder="Qty..."></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 210px; padding-right:3px">
                                                                        <asp:DropDownList ID="LogStartMN" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>MM</asp:ListItem>
                                                                            <asp:ListItem>01</asp:ListItem>
                                                                            <asp:ListItem>02</asp:ListItem>
                                                                            <asp:ListItem>03</asp:ListItem>
                                                                            <asp:ListItem>04</asp:ListItem>
                                                                            <asp:ListItem>05</asp:ListItem>
                                                                            <asp:ListItem>06</asp:ListItem>
                                                                            <asp:ListItem>07</asp:ListItem>
                                                                            <asp:ListItem>08</asp:ListItem>
                                                                            <asp:ListItem>09</asp:ListItem>
                                                                            <asp:ListItem>10</asp:ListItem>
                                                                            <asp:ListItem>11</asp:ListItem>
                                                                            <asp:ListItem>12</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="PDL1" runat="server" Text=""></asp:Label>
                                                                        <asp:Label ID="PDL2" runat="server" Text="  &#0047;  " Font-Bold="True" Font-Size="Large" ForeColor="White"></asp:Label>
                                                                        <asp:DropDownList ID="LogStartDD" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>DD</asp:ListItem>
                                                                            <asp:ListItem>01</asp:ListItem>
                                                                            <asp:ListItem>02</asp:ListItem>
                                                                            <asp:ListItem>03</asp:ListItem>
                                                                            <asp:ListItem>04</asp:ListItem>
                                                                            <asp:ListItem>05</asp:ListItem>
                                                                            <asp:ListItem>06</asp:ListItem>
                                                                            <asp:ListItem>07</asp:ListItem>
                                                                            <asp:ListItem>08</asp:ListItem>
                                                                            <asp:ListItem>09</asp:ListItem>
                                                                            <asp:ListItem>10</asp:ListItem>
                                                                            <asp:ListItem>11</asp:ListItem>
                                                                            <asp:ListItem>12</asp:ListItem>
                                                                            <asp:ListItem>13</asp:ListItem>
                                                                            <asp:ListItem>14</asp:ListItem>
                                                                            <asp:ListItem>15</asp:ListItem>
                                                                            <asp:ListItem>16</asp:ListItem>
                                                                            <asp:ListItem>17</asp:ListItem>
                                                                            <asp:ListItem>18</asp:ListItem>
                                                                            <asp:ListItem>19</asp:ListItem>
                                                                            <asp:ListItem>20</asp:ListItem>
                                                                            <asp:ListItem>21</asp:ListItem>
                                                                            <asp:ListItem>22</asp:ListItem>
                                                                            <asp:ListItem>23</asp:ListItem>
                                                                            <asp:ListItem>24</asp:ListItem>
                                                                            <asp:ListItem>25</asp:ListItem>
                                                                            <asp:ListItem>26</asp:ListItem>
                                                                            <asp:ListItem>27</asp:ListItem>
                                                                            <asp:ListItem>28</asp:ListItem>
                                                                            <asp:ListItem>29</asp:ListItem>
                                                                            <asp:ListItem>30</asp:ListItem>
                                                                            <asp:ListItem>31</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="PDL3" runat="server" Text=""></asp:Label>
                                                                        <asp:Label ID="PDL4" runat="server" Text="  &#0047;  " Font-Bold="True" Font-Size="Large" ForeColor="White"></asp:Label>
                                                                        <asp:Label ID="PDL5" runat="server" Text='<%# Eval("Date", "{yyyy}") %>' ForeColor="White"></asp:Label>
                                                                    </td>
                                                                      <td style="width: 210px; padding-right:3px">
                                                                        <asp:DropDownList ID="LogEndMN" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>MM</asp:ListItem>
                                                                            <asp:ListItem>01</asp:ListItem>
                                                                            <asp:ListItem>02</asp:ListItem>
                                                                            <asp:ListItem>03</asp:ListItem>
                                                                            <asp:ListItem>04</asp:ListItem>
                                                                            <asp:ListItem>05</asp:ListItem>
                                                                            <asp:ListItem>06</asp:ListItem>
                                                                            <asp:ListItem>07</asp:ListItem>
                                                                            <asp:ListItem>08</asp:ListItem>
                                                                            <asp:ListItem>09</asp:ListItem>
                                                                            <asp:ListItem>10</asp:ListItem>
                                                                            <asp:ListItem>11</asp:ListItem>
                                                                            <asp:ListItem>12</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="PDL6" runat="server" Text=""></asp:Label>
                                                                        <asp:Label ID="PDL7" runat="server" Text="  &#0047;  " Font-Bold="True" Font-Size="Large" ForeColor="White"></asp:Label>
                                                                        <asp:DropDownList ID="LogEndDD" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>DD</asp:ListItem>
                                                                            <asp:ListItem>01</asp:ListItem>
                                                                            <asp:ListItem>02</asp:ListItem>
                                                                            <asp:ListItem>03</asp:ListItem>
                                                                            <asp:ListItem>04</asp:ListItem>
                                                                            <asp:ListItem>05</asp:ListItem>
                                                                            <asp:ListItem>06</asp:ListItem>
                                                                            <asp:ListItem>07</asp:ListItem>
                                                                            <asp:ListItem>08</asp:ListItem>
                                                                            <asp:ListItem>09</asp:ListItem>
                                                                            <asp:ListItem>10</asp:ListItem>
                                                                            <asp:ListItem>11</asp:ListItem>
                                                                            <asp:ListItem>12</asp:ListItem>
                                                                            <asp:ListItem>13</asp:ListItem>
                                                                            <asp:ListItem>14</asp:ListItem>
                                                                            <asp:ListItem>15</asp:ListItem>
                                                                            <asp:ListItem>16</asp:ListItem>
                                                                            <asp:ListItem>17</asp:ListItem>
                                                                            <asp:ListItem>18</asp:ListItem>
                                                                            <asp:ListItem>19</asp:ListItem>
                                                                            <asp:ListItem>20</asp:ListItem>
                                                                            <asp:ListItem>21</asp:ListItem>
                                                                            <asp:ListItem>22</asp:ListItem>
                                                                            <asp:ListItem>23</asp:ListItem>
                                                                            <asp:ListItem>24</asp:ListItem>
                                                                            <asp:ListItem>25</asp:ListItem>
                                                                            <asp:ListItem>26</asp:ListItem>
                                                                            <asp:ListItem>27</asp:ListItem>
                                                                            <asp:ListItem>28</asp:ListItem>
                                                                            <asp:ListItem>29</asp:ListItem>
                                                                            <asp:ListItem>30</asp:ListItem>
                                                                            <asp:ListItem>31</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="PDL8" runat="server" Text=""></asp:Label>
                                                                        <asp:Label ID="PDL9" runat="server" Text="  &#0047;  " Font-Bold="True" Font-Size="Large" ForeColor="White"></asp:Label>
                                                                        <asp:Label ID="PDL10" runat="server" Text='<%# Eval("Date", "{yyyy}") %>' ForeColor="White"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                  <td style="width: 210px">
                                                                        <asp:DropDownList ID="LogStartHH" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>12</asp:ListItem>
                                                                            <asp:ListItem>01</asp:ListItem>
                                                                            <asp:ListItem>02</asp:ListItem>
                                                                            <asp:ListItem>03</asp:ListItem>
                                                                            <asp:ListItem>04</asp:ListItem>
                                                                            <asp:ListItem>05</asp:ListItem>
                                                                            <asp:ListItem>06</asp:ListItem>
                                                                            <asp:ListItem>07</asp:ListItem>
                                                                            <asp:ListItem>08</asp:ListItem>
                                                                            <asp:ListItem>09</asp:ListItem>
                                                                            <asp:ListItem>10</asp:ListItem>
                                                                            <asp:ListItem>11</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="PTL1" runat="server" Text=""></asp:Label>
                                                                        <asp:Label ID="PTL2" runat="server" Text="  : " Font-Bold="True" Font-Size="Large" ForeColor="White"></asp:Label>
                                                                        <asp:DropDownList ID="LogStartMM" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>00</asp:ListItem>
                                                                            <asp:ListItem>05</asp:ListItem>
                                                                            <asp:ListItem>10</asp:ListItem>
                                                                            <asp:ListItem>15</asp:ListItem>
                                                                            <asp:ListItem>20</asp:ListItem>
                                                                            <asp:ListItem>25</asp:ListItem>
                                                                            <asp:ListItem>30</asp:ListItem>
                                                                            <asp:ListItem>35</asp:ListItem>
                                                                            <asp:ListItem>40</asp:ListItem>
                                                                            <asp:ListItem>45</asp:ListItem>
                                                                            <asp:ListItem>50</asp:ListItem>
                                                                            <asp:ListItem>55</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="PTL3" runat="server" Text=""></asp:Label>
                                                                        <asp:Label ID="PTL4" runat="server" Text="  : " Font-Bold="True" Font-Size="Large" ForeColor="White"></asp:Label>
                                                                        <asp:DropDownList ID="LogStartTZ" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>AM</asp:ListItem>
                                                                            <asp:ListItem>PM</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td style="width: 210px">
                                                                        <asp:DropDownList ID="LogEndHH" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>12</asp:ListItem>
                                                                            <asp:ListItem>01</asp:ListItem>
                                                                            <asp:ListItem>02</asp:ListItem>
                                                                            <asp:ListItem>03</asp:ListItem>
                                                                            <asp:ListItem>04</asp:ListItem>
                                                                            <asp:ListItem>05</asp:ListItem>
                                                                            <asp:ListItem>06</asp:ListItem>
                                                                            <asp:ListItem>07</asp:ListItem>
                                                                            <asp:ListItem>08</asp:ListItem>
                                                                            <asp:ListItem>09</asp:ListItem>
                                                                            <asp:ListItem>10</asp:ListItem>
                                                                            <asp:ListItem>11</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="PTL5" runat="server" Text=""></asp:Label>
                                                                        <asp:Label ID="PTL6" runat="server" Text=" : " Font-Bold="True" Font-Size="Large" ForeColor="White"></asp:Label>
                                                                        <asp:DropDownList ID="LogEndMM" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>00</asp:ListItem>
                                                                            <asp:ListItem>05</asp:ListItem>
                                                                            <asp:ListItem>10</asp:ListItem>
                                                                            <asp:ListItem>15</asp:ListItem>
                                                                            <asp:ListItem>20</asp:ListItem>
                                                                            <asp:ListItem>25</asp:ListItem>
                                                                            <asp:ListItem>30</asp:ListItem>
                                                                            <asp:ListItem>35</asp:ListItem>
                                                                            <asp:ListItem>40</asp:ListItem>
                                                                            <asp:ListItem>45</asp:ListItem>
                                                                            <asp:ListItem>50</asp:ListItem>
                                                                            <asp:ListItem>55</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="PTL7" runat="server" Text=""></asp:Label>
                                                                        <asp:Label ID="PTL8" runat="server" Text=" : " Font-Bold="True" Font-Size="Large" ForeColor="White"></asp:Label>
                                                                        <asp:DropDownList ID="LogEndTZ" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>AM</asp:ListItem>
                                                                            <asp:ListItem>PM</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr style="padding-top:10px">
                                        <td style="padding-top: 10px; width: 1000px;">
                                            <asp:Panel ID="Divider" runat="server" BackColor="LightGray" Height="25px">
                                                <table>
                                                    <tr>
                                                        <td style="width: 1000px; text-align: center">
                                                            <asp:Label ID="Label6" runat="server" Text="PREVIOUS SHIFT'S DOWN TIME" ForeColor="#808080" Font-Bold="true" Font-Size="Larger"></asp:Label> 
                                                            <asp:Label ID="Label4" runat="server" Text="" ForeColor="Black" Font-Bold="true" Width="100px"></asp:Label>
                                                            <asp:Label ID="Label3" runat="server" Text="CURRENT SHIFT'S DOWN TIME" ForeColor="Black" Font-Bold="true" Font-Size="Larger"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 1000px; padding-top: 10px">
                                            <asp:GridView ID="ProLogDTPre" runat="server" AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSourceProLogDTPre" ForeColor="#333333" Width="997px" DataKeyNames="Key" ShowFooter="True" ShowHeaderWhenEmpty="True" GridLines="None">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:BoundField DataField="Key" HeaderText="Key" SortExpression="Key" ReadOnly="true" InsertVisible="False"></asp:BoundField>
                                                    <asp:BoundField DataField="Shift" HeaderText="Shift" SortExpression="Shift"></asp:BoundField>
                                                    <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" />
                                                    <asp:BoundField DataField="Event" HeaderText="Event" SortExpression="Event" />
                                                    <asp:BoundField DataField="StartTime" HeaderText="StartTime" SortExpression="StartTime" DataFormatString="{0:g}"/>
                                                    <asp:BoundField DataField="EndTime" HeaderText="EndTime" SortExpression="EndTime" DataFormatString="{0:g}"/>
                                                    <asp:BoundField DataField="Hours" HeaderText="Hours" ReadOnly="True" SortExpression="Hours" DataFormatString="{0:0.0}"/>
                                                    <asp:BoundField DataField="Op" HeaderText="Op" SortExpression="Op" />
                                                </Columns>
                                                <EditRowStyle BackColor="#7C6F57" />
                                                <EmptyDataTemplate>
                                                    No Data Has Been Entered for This Shift
                                                </EmptyDataTemplate>
                                                <FooterStyle BackColor="#808080" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#808080" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                                <RowStyle HorizontalAlign="Center" BackColor="#E3EAEB" />
                                                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                                <SortedAscendingHeaderStyle BackColor="#246B61" />
                                                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                                <SortedDescendingHeaderStyle BackColor="#15524A" />
                                            </asp:GridView>
                                            <br />
                                            <asp:GridView ID="ProLogDTCurrent" runat="server" AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSourceProLogDT" ForeColor="#333333" Width="997px" ShowHeaderWhenEmpty="True" GridLines="None" DataKeyNames="Key">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True" SortExpression="Key" />
                                                    <asp:BoundField DataField="Shift" HeaderText="Shift" SortExpression="Shift" ReadOnly="True"/>
                                                    <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" ReadOnly="True"/>
                                                    <asp:BoundField DataField="Event" HeaderText="Event" SortExpression="Event" />
                                                    <asp:BoundField DataField="StartTime" HeaderText="StartTime" SortExpression="StartTime" DataFormatString="{0:g}"/>
                                                    <asp:BoundField DataField="EndTime" HeaderText="EndTime" SortExpression="EndTime" DataFormatString="{0:g}"/>
                                                    <asp:BoundField DataField="Hours" HeaderText="Hours" ReadOnly="True" SortExpression="Hours" DataFormatString="{0:0.0}"/>
                                                    <asp:BoundField DataField="Op" HeaderText="Op" SortExpression="Op" ReadOnly="True"/>
                                                    <asp:CommandField DeleteText="Del" ShowDeleteButton="True" ShowEditButton="True" />
                                                </Columns>
                                                <EditRowStyle BackColor="#7C6F57" />
                                                <EmptyDataTemplate>
                                                    No Data Has Been Entered for This Shift
                                                </EmptyDataTemplate>

                                                <FooterStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                                <RowStyle HorizontalAlign="Center" BackColor="#E3EAEB" />
                                                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                                <SortedAscendingHeaderStyle BackColor="#246B61" />
                                                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                                <SortedDescendingHeaderStyle BackColor="#15524A" />
                                            </asp:GridView>

                                            <asp:SqlDataSource ID="SqlDataSourceProLogDTPre" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                                                SelectCommand="SELECT [Key], Shift, Department, Event, StartTime, EndTime, Op, (DATEDIFF(ss, StartTime, EndTime) * 1.0) / (60 * 60) AS Hours FROM T_ProLogDT WHERE (Department = @Department) AND (Shift = @Shift) AND (ReportDate = @ReportDate) ORDER BY StartTime"
                                                DeleteCommand="DELETE FROM [T_ProLogDT] WHERE [Key] = @original_Key"
                                                InsertCommand="INSERT INTO [T_ProLogDT] ([Shift], [Department], [Event], [StartTime], [EndTime], [ReportDate], [Op]) VALUES (@Shift, @Department, @Event, @StartTime, @EndTime, @ReportDate, @Op)" OldValuesParameterFormatString="original_{0}"
                                                UpdateCommand="UPDATE [T_ProLogDT] SET [Event] = @Event, [StartTime] = @StartTime, [EndTime] = @EndTime WHERE [Key] = @original_Key">
                                                <DeleteParameters>
                                                    <asp:Parameter Name="original_Key" Type="Int32" />
                                                </DeleteParameters>
                                                <InsertParameters>
                                                    <asp:Parameter Name="Shift" Type="String" />
                                                    <asp:Parameter Name="Department" Type="String" />
                                                    <asp:Parameter Name="Event" Type="String" />
                                                    <asp:Parameter Name="StartTime" Type="DateTime" />
                                                    <asp:Parameter Name="EndTime" Type="DateTime" />
                                                    <asp:Parameter Name="ReportDate" Type="DateTime" />
                                                    <asp:Parameter Name="Op" Type="String" />
                                                </InsertParameters>
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="DepartmentDropDown" Name="Department" PropertyName="SelectedValue" Type="String" />
                                                    <asp:ControlParameter ControlID="ShiftDropDown" Name="Shift" PropertyName="SelectedValue" />
                                                    <asp:ControlParameter ControlID="DateTextBox" Name="ReportDate" PropertyName="SelectedDate" DbType="Date" />
                                                </SelectParameters>
                                                <UpdateParameters>
                                                    <asp:Parameter Name="Event" Type="String" />
                                                    <asp:Parameter Name="StartTime" Type="DateTime" />
                                                    <asp:Parameter Name="EndTime" Type="DateTime" />
                                                    <asp:Parameter Name="original_Key" Type="Int32" />
                                                </UpdateParameters>
                                            </asp:SqlDataSource>
                                            <asp:SqlDataSource ID="SqlDataSourceProLogDT" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                                                SelectCommand="SELECT [Key], Shift, Department, Event, StartTime, EndTime, Op, (DATEDIFF(ss, StartTime, EndTime) * 1.0) / (60 * 60) AS Hours FROM T_ProLogDT WHERE (Department = @Department) AND (Shift = @Shift) AND (ReportDate = @ReportDate) ORDER BY StartTime"
                                                DeleteCommand="DELETE FROM [T_ProLogDT] WHERE [Key] = @original_Key"
                                                InsertCommand="INSERT INTO [T_ProLogDT] ([Shift], [Department], [Event], [StartTime], [EndTime], [ReportDate], [Op]) VALUES (@Shift, @Department, @Event, @StartTime, @EndTime, @ReportDate, @Op)" OldValuesParameterFormatString="original_{0}"
                                                UpdateCommand="UPDATE [T_ProLogDT] SET [Event] = @Event, [StartTime] = @StartTime, [EndTime] = @EndTime WHERE [Key] = @original_Key">
                                                <DeleteParameters>
                                                    <asp:Parameter Name="original_Key" Type="Int32" />
                                                </DeleteParameters>
                                                <InsertParameters>
                                                    <asp:Parameter Name="Shift" Type="String" />
                                                    <asp:Parameter Name="Department" Type="String" />
                                                    <asp:Parameter Name="Event" Type="String" />
                                                    <asp:Parameter Name="StartTime" Type="DateTime" />
                                                    <asp:Parameter Name="EndTime" Type="DateTime" />
                                                    <asp:Parameter Name="ReportDate" Type="DateTime" />
                                                    <asp:Parameter Name="Op" Type="String" />
                                                </InsertParameters>
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="DepartmentDropDown" Name="Department" PropertyName="SelectedValue" Type="String" />
                                                    <asp:ControlParameter ControlID="ShiftDropDown" Name="Shift" PropertyName="SelectedValue" />
                                                    <asp:ControlParameter ControlID="DateTextBox" Name="ReportDate" PropertyName="SelectedDate" DbType="Date" />
                                                </SelectParameters>
                                                <UpdateParameters>
                                                    <asp:Parameter Name="Event" Type="String" />
                                                    <asp:Parameter Name="StartTime" Type="DateTime" />
                                                    <asp:Parameter Name="EndTime" Type="DateTime" />
                                                    <asp:Parameter Name="original_Key" Type="Int32" />
                                                </UpdateParameters>
                                            </asp:SqlDataSource>

                                            <asp:Panel ID="AddNewDTRecord" runat="server" Width="997px" Height="70" HorizontalAlign="Left">
                                                <table style="width: 997px; height: 50px; background-color: Black; padding-left: 2px">
                                                    <tr>
                                                        <td style="width: 250px">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="CreateNewDT" runat="server" Text="&#10003;  Create" Width="110px" BackColor="#00cc00" Font-Bold="true" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 10px"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="CancelADT" runat="server" Text="&#10008;  Cancel" Width="110px" BackColor="#ff0000" Height="23px" Font-Bold="true" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 350px; padding-top:5px"">
                                                            <asp:TextBox ID="AddEvent" runat="server" Width="300px" Height="50px" placeholder="Down Time Description..." TextMode="MultiLine"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 210px">
                                                                        <asp:DropDownList ID="DTStartMN" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>MM</asp:ListItem>
                                                                            <asp:ListItem>01</asp:ListItem>
                                                                            <asp:ListItem>02</asp:ListItem>
                                                                            <asp:ListItem>03</asp:ListItem>
                                                                            <asp:ListItem>04</asp:ListItem>
                                                                            <asp:ListItem>05</asp:ListItem>
                                                                            <asp:ListItem>06</asp:ListItem>
                                                                            <asp:ListItem>07</asp:ListItem>
                                                                            <asp:ListItem>08</asp:ListItem>
                                                                            <asp:ListItem>09</asp:ListItem>
                                                                            <asp:ListItem>10</asp:ListItem>
                                                                            <asp:ListItem>11</asp:ListItem>
                                                                            <asp:ListItem>12</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="DDL1" runat="server" Text=""></asp:Label>
                                                                        <asp:Label ID="DDL2" runat="server" Text="  &#0047;  " Font-Bold="True" Font-Size="Large" ForeColor="White"></asp:Label>
                                                                        <asp:DropDownList ID="DTStartDD" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>DD</asp:ListItem>
                                                                            <asp:ListItem>01</asp:ListItem>
                                                                            <asp:ListItem>02</asp:ListItem>
                                                                            <asp:ListItem>03</asp:ListItem>
                                                                            <asp:ListItem>04</asp:ListItem>
                                                                            <asp:ListItem>05</asp:ListItem>
                                                                            <asp:ListItem>06</asp:ListItem>
                                                                            <asp:ListItem>07</asp:ListItem>
                                                                            <asp:ListItem>08</asp:ListItem>
                                                                            <asp:ListItem>09</asp:ListItem>
                                                                            <asp:ListItem>10</asp:ListItem>
                                                                            <asp:ListItem>11</asp:ListItem>
                                                                            <asp:ListItem>12</asp:ListItem>
                                                                            <asp:ListItem>13</asp:ListItem>
                                                                            <asp:ListItem>14</asp:ListItem>
                                                                            <asp:ListItem>15</asp:ListItem>
                                                                            <asp:ListItem>16</asp:ListItem>
                                                                            <asp:ListItem>17</asp:ListItem>
                                                                            <asp:ListItem>18</asp:ListItem>
                                                                            <asp:ListItem>19</asp:ListItem>
                                                                            <asp:ListItem>20</asp:ListItem>
                                                                            <asp:ListItem>21</asp:ListItem>
                                                                            <asp:ListItem>22</asp:ListItem>
                                                                            <asp:ListItem>23</asp:ListItem>
                                                                            <asp:ListItem>24</asp:ListItem>
                                                                            <asp:ListItem>25</asp:ListItem>
                                                                            <asp:ListItem>26</asp:ListItem>
                                                                            <asp:ListItem>27</asp:ListItem>
                                                                            <asp:ListItem>28</asp:ListItem>
                                                                            <asp:ListItem>29</asp:ListItem>
                                                                            <asp:ListItem>30</asp:ListItem>
                                                                            <asp:ListItem>31</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="DDL3" runat="server" Text=""></asp:Label>
                                                                        <asp:Label ID="DDL4" runat="server" Text="  &#0047;  " Font-Bold="True" Font-Size="Large" ForeColor="White"></asp:Label>
                                                                        <asp:Label ID="DDL5" runat="server" Text='<%# Eval("Date", "{yyyy}") %>' ForeColor="White"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 210px">
                                                                        <asp:DropDownList ID="DTEndMN" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>MM</asp:ListItem>
                                                                            <asp:ListItem>01</asp:ListItem>
                                                                            <asp:ListItem>02</asp:ListItem>
                                                                            <asp:ListItem>03</asp:ListItem>
                                                                            <asp:ListItem>04</asp:ListItem>
                                                                            <asp:ListItem>05</asp:ListItem>
                                                                            <asp:ListItem>06</asp:ListItem>
                                                                            <asp:ListItem>07</asp:ListItem>
                                                                            <asp:ListItem>08</asp:ListItem>
                                                                            <asp:ListItem>09</asp:ListItem>
                                                                            <asp:ListItem>10</asp:ListItem>
                                                                            <asp:ListItem>11</asp:ListItem>
                                                                            <asp:ListItem>12</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="DDL6" runat="server" Text=""></asp:Label>
                                                                        <asp:Label ID="DDL7" runat="server" Text="  &#0047;  " Font-Bold="True" Font-Size="Large" ForeColor="White"></asp:Label>
                                                                        <asp:DropDownList ID="DTEndDD" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>DD</asp:ListItem>
                                                                            <asp:ListItem>01</asp:ListItem>
                                                                            <asp:ListItem>02</asp:ListItem>
                                                                            <asp:ListItem>03</asp:ListItem>
                                                                            <asp:ListItem>04</asp:ListItem>
                                                                            <asp:ListItem>05</asp:ListItem>
                                                                            <asp:ListItem>06</asp:ListItem>
                                                                            <asp:ListItem>07</asp:ListItem>
                                                                            <asp:ListItem>08</asp:ListItem>
                                                                            <asp:ListItem>09</asp:ListItem>
                                                                            <asp:ListItem>10</asp:ListItem>
                                                                            <asp:ListItem>11</asp:ListItem>
                                                                            <asp:ListItem>12</asp:ListItem>
                                                                            <asp:ListItem>13</asp:ListItem>
                                                                            <asp:ListItem>14</asp:ListItem>
                                                                            <asp:ListItem>15</asp:ListItem>
                                                                            <asp:ListItem>16</asp:ListItem>
                                                                            <asp:ListItem>17</asp:ListItem>
                                                                            <asp:ListItem>18</asp:ListItem>
                                                                            <asp:ListItem>19</asp:ListItem>
                                                                            <asp:ListItem>20</asp:ListItem>
                                                                            <asp:ListItem>21</asp:ListItem>
                                                                            <asp:ListItem>22</asp:ListItem>
                                                                            <asp:ListItem>23</asp:ListItem>
                                                                            <asp:ListItem>24</asp:ListItem>
                                                                            <asp:ListItem>25</asp:ListItem>
                                                                            <asp:ListItem>26</asp:ListItem>
                                                                            <asp:ListItem>27</asp:ListItem>
                                                                            <asp:ListItem>28</asp:ListItem>
                                                                            <asp:ListItem>29</asp:ListItem>
                                                                            <asp:ListItem>30</asp:ListItem>
                                                                            <asp:ListItem>31</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="DDL8" runat="server" Text=""></asp:Label>
                                                                        <asp:Label ID="DDL9" runat="server" Text="  &#0047;  " Font-Bold="True" Font-Size="Large" ForeColor="White"></asp:Label>
                                                                        <asp:Label ID="DDL10" runat="server" Text='<%# Eval("Date", "{yyyy}") %>' ForeColor="White"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 210px">
                                                                        <asp:DropDownList ID="DTStartHH" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>12</asp:ListItem>
                                                                            <asp:ListItem>01</asp:ListItem>
                                                                            <asp:ListItem>02</asp:ListItem>
                                                                            <asp:ListItem>03</asp:ListItem>
                                                                            <asp:ListItem>04</asp:ListItem>
                                                                            <asp:ListItem>05</asp:ListItem>
                                                                            <asp:ListItem>06</asp:ListItem>
                                                                            <asp:ListItem>07</asp:ListItem>
                                                                            <asp:ListItem>08</asp:ListItem>
                                                                            <asp:ListItem>09</asp:ListItem>
                                                                            <asp:ListItem>10</asp:ListItem>
                                                                            <asp:ListItem>11</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="DTL1" runat="server" Text=""></asp:Label>
                                                                        <asp:Label ID="DTL2" runat="server" Text=" : " Font-Bold="True" Font-Size="Large" ForeColor="White"></asp:Label>
                                                                        <asp:DropDownList ID="DTStartMM" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>00</asp:ListItem>
                                                                            <asp:ListItem>05</asp:ListItem>
                                                                            <asp:ListItem>10</asp:ListItem>
                                                                            <asp:ListItem>15</asp:ListItem>
                                                                            <asp:ListItem>20</asp:ListItem>
                                                                            <asp:ListItem>25</asp:ListItem>
                                                                            <asp:ListItem>30</asp:ListItem>
                                                                            <asp:ListItem>35</asp:ListItem>
                                                                            <asp:ListItem>40</asp:ListItem>
                                                                            <asp:ListItem>45</asp:ListItem>
                                                                            <asp:ListItem>50</asp:ListItem>
                                                                            <asp:ListItem>55</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="DTL3" runat="server" Text=""></asp:Label>
                                                                        <asp:Label ID="DTL4" runat="server" Text=" : " Font-Bold="True" Font-Size="Large" ForeColor="White"></asp:Label>
                                                                        <asp:DropDownList ID="DTStartTT" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>AM</asp:ListItem>
                                                                            <asp:ListItem>PM</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td style="width: 210px">
                                                                        <asp:DropDownList ID="DTEndHH" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>12</asp:ListItem>
                                                                            <asp:ListItem>01</asp:ListItem>
                                                                            <asp:ListItem>02</asp:ListItem>
                                                                            <asp:ListItem>03</asp:ListItem>
                                                                            <asp:ListItem>04</asp:ListItem>
                                                                            <asp:ListItem>05</asp:ListItem>
                                                                            <asp:ListItem>06</asp:ListItem>
                                                                            <asp:ListItem>07</asp:ListItem>
                                                                            <asp:ListItem>08</asp:ListItem>
                                                                            <asp:ListItem>09</asp:ListItem>
                                                                            <asp:ListItem>10</asp:ListItem>
                                                                            <asp:ListItem>11</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="DTL5" runat="server" Text=""></asp:Label>
                                                                        <asp:Label ID="DTL6" runat="server" Text=" : " Font-Bold="True" Font-Size="Large" ForeColor="White"></asp:Label>
                                                                        <asp:DropDownList ID="DTEndMM" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>00</asp:ListItem>
                                                                            <asp:ListItem>05</asp:ListItem>
                                                                            <asp:ListItem>10</asp:ListItem>
                                                                            <asp:ListItem>15</asp:ListItem>
                                                                            <asp:ListItem>20</asp:ListItem>
                                                                            <asp:ListItem>25</asp:ListItem>
                                                                            <asp:ListItem>30</asp:ListItem>
                                                                            <asp:ListItem>35</asp:ListItem>
                                                                            <asp:ListItem>40</asp:ListItem>
                                                                            <asp:ListItem>45</asp:ListItem>
                                                                            <asp:ListItem>50</asp:ListItem>
                                                                            <asp:ListItem>55</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:Label ID="DTL7" runat="server" Text=""></asp:Label>
                                                                        <asp:Label ID="DTL8" runat="server" Text=" : " Font-Bold="True" Font-Size="Large" ForeColor="White"></asp:Label>
                                                                        <asp:DropDownList ID="DTEndTT" runat="server" AutoPostBack="True">
                                                                            <asp:ListItem>AM</asp:ListItem>
                                                                            <asp:ListItem>PM</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 1000px; padding-top: 10px">
                            <asp:Panel ID="ErrorMessagePanel" runat="server" HorizontalAlign="Center" BackColor="LightGray" Height="30px">
                                <table style="width: 1000px; align-content: center">
                                    <tr>
                                        <td style="width: 1000px; align-content: center">
                                            <asp:Label ID="ErrorMessage" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="BottemPanel" runat="server" HorizontalAlign="Center" BackColor="LightGray" Height="30px">
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
