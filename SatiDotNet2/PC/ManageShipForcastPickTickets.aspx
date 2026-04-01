<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ManageShipForcastPickTickets.aspx.vb" Inherits="PC_ManageShipForcastPickTickets" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <p>Manage Sales Forcasted Pick Tickets&nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
            </p>
            <p>
                <asp:CheckBox ID="CheckBox1" runat="server" Text="Filter By ID"/> <br />  
                <asp:TextBox ID="TextBoxID" runat="server" Width="100"></asp:TextBox> &nbsp;
                <asp:Button ID="Button1" runat="server" Text="Add ID" />
                
            </p>
            <asp:Panel ID="SSPanel" runat="server" Width="880px">
                
                <asp:Panel ID="PanelLink" runat="server" BackColor="#00FF99" Width="250" Visible="false">
                    
                    <table style="padding-left: 10px">
                        <tr>
                            <td>
                                &nbsp;Record# &nbsp;<asp:Label ID="LabelRecordNumber" runat="server" Text="New or xxxxxxxx"></asp:Label><br />
                                &nbsp;ID: &nbsp; <asp:Label ID="LabelID" runat="server" Text="XXXX"></asp:Label><br />
                                &nbsp;SO# &nbsp;<asp:DropDownList ID="DropDownListSO" runat="server"></asp:DropDownList>&nbsp;  <br /> 
                                &nbsp;Qty: &nbsp; <asp:TextBox ID="TextBox1" runat="server"  Width="100"></asp:TextBox><br />
                                <br />
                                &nbsp;Work Week: &nbsp; <asp:Label ID="LabelWW" runat="server" Text="date"></asp:Label><br />
                                &nbsp;Day of Week: &nbsp; <asp:Label ID="LabelDOW" runat="server" Text="date"></asp:Label><br />
                                &nbsp;Dock Date: &nbsp; <asp:Label ID="LabelDate" runat="server" Text="date"></asp:Label><br />
                            </td>
                        </tr>
                        <tr>
                            <td >
                                <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="#3366CC" BorderWidth="1px" CellPadding="1" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="#003399" Height="200px" Width="220px">
                                    <DayHeaderStyle BackColor="#99CCCC" ForeColor="#336666" Height="1px" />
                                    <NextPrevStyle Font-Size="8pt" ForeColor="#CCCCFF" />
                                    <OtherMonthDayStyle ForeColor="#999999" />
                                    <SelectedDayStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                    <SelectorStyle BackColor="#99CCCC" ForeColor="#336666" />
                                    <TitleStyle BackColor="#003399" BorderColor="#3366CC" BorderWidth="1px" Font-Bold="True" Font-Size="10pt" ForeColor="#CCCCFF" Height="25px" />
                                    <TodayDayStyle BackColor="#99CCCC" ForeColor="White" />
                                    <WeekendDayStyle BackColor="#CCCCFF" />
                                </asp:Calendar> 
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center">                                                                
                                <asp:Button ID="ButtonAddEdit" runat="server" Text="Add" /> 
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" />                                 
                            </td>
                        </tr>
                    </table>
                    
                   
                    
                    
                    
                    
                    
                   
                   
                    <br />
                     
                                    
                </asp:Panel>
               
                 <cc1:RoundedCornersExtender ID="PanelLink_RoundedCornersExtender" runat="server" BehaviorID="PanelLink_RoundedCornersExtender" Radius="15" TargetControlID="PanelLink">
                </cc1:RoundedCornersExtender>
               
                 <cc1:AlwaysVisibleControlExtender ID="PanelLink_AlwaysVisibleControlExtender" 
                    runat="server" Enabled="True" HorizontalSide="Center" 
                    TargetControlID="PanelLink" VerticalSide="Middle">
                </cc1:AlwaysVisibleControlExtender>
                
                <asp:SqlDataSource ID="ScheduledShipmentsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT WorkWeek, ID, SO, FGI, Qty, DockDate, DayofWeek, Entry FROM dbo.q_SalesSchedule ORDER BY WorkWeek, ID">
                </asp:SqlDataSource>
                
                <asp:GridView ID="ScheduledShipmentsGridView" runat="server" 
                    AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
                    DataSourceID="ScheduledShipmentsSqlDataSource" ForeColor="#333333" 
                    GridLines="None">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:ButtonField ButtonType="Button" CommandName="MakePT" Text="Make PT" />
                        <asp:BoundField DataField="WorkWeek" HeaderText="WorkWeek" 
                            SortExpression="WorkWeek" />
                        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                        <asp:BoundField DataField="SO" HeaderText="SO" SortExpression="SO" />
                        <asp:BoundField DataField="FGI" HeaderText="FGI" SortExpression="FGI" />
                        <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" />
                        <asp:BoundField DataField="DockDate" DataFormatString="{0:d}" 
                            HeaderText="DockDate" SortExpression="DockDate" />
                        <asp:BoundField DataField="DayofWeek" HeaderText="DayofWeek" 
                            SortExpression="DayofWeek" />
                        <asp:BoundField DataField="Entry" HeaderText="Entry" SortExpression="Entry" />
                    </Columns>
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>

            </asp:Panel>           

        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>

