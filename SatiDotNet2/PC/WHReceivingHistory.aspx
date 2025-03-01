<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="WHReceivingHistory.aspx.vb" Inherits="PC_WHReceivingHistory" title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>   
        <asp:Panel ID="PanelOut" runat="server">            
                <table class="style1">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" 
                                Text="Receiving History"></asp:Label>
                        </td>
                        <td style="text-align: left">
                            &nbsp;</td>
                    </tr>
                </table>
                
                <asp:Panel ID="PanelFilter" runat="server">            
                    <table class="style1">
                        <tr>
                            <td>
                                Date From
                                <asp:TextBox ID="TextBoxAfterDate" runat="server"></asp:TextBox>
                                &nbsp;<asp:CheckBox ID="CheckBoxFromDate" runat="server" 
                                    Text="Filter By This Day and Later" AutoPostBack="True" />
                                
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownListFab" runat="server" Width="138px" 
                                    DataSourceID="SqlDataSourceFabs" DataTextField="Fab" DataValueField="Fab">
                                </asp:DropDownList>
                                <asp:CheckBox ID="CheckBoxFab" runat="server" Text="Filter By Fab" 
                                    AutoPostBack="True" />
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                Date To&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:TextBox ID="TextBoxBeforeDate" runat="server"></asp:TextBox>
                                &nbsp;<asp:CheckBox ID="CheckBoxToDate" runat="server" 
                                    Text="Filter By This Day and Before" AutoPostBack="True" />
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownListID" runat="server" Width="138px" 
                                    DataSourceID="SqlDataSourceID" DataTextField="ID" DataValueField="ID">
                                </asp:DropDownList>
                                <asp:CheckBox ID="CheckBoxID" runat="server" Text="Filter By ID" 
                                    AutoPostBack="True" />
                                &nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center">
                               
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="DropDownListCustomer" runat="server" Width="138px" 
                                                                DataSourceID="SqlDataSourceCustomers" DataTextField="Customer_Name" 
                                    DataValueField="Customer_Name">
                                </asp:DropDownList>
                                <asp:CheckBox ID="CheckBoxCustomer" runat="server" Text="Filter By Customer" 
                                    AutoPostBack="True" />
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="text-align: center">
                                <asp:Button ID="ButtonFind" runat="server" Text="Search Records" />
                            </td>
                            <td>
                                <asp:DropDownList ID="DropDownListSize" runat="server" 
                                    DataSourceID="SqlDataSourceSize" DataTextField="Diameter" 
                                    DataValueField="Diameter" Width="138px">
                                </asp:DropDownList>
                                <asp:CheckBox ID="CheckBoxSize" runat="server" Text="Filter By Size" 
                                    AutoPostBack="True" />
                            </td>
                        </tr>                
                    </table>        
                </asp:Panel>
                
                <cc1:CalendarExtender runat="server" TargetControlID="TextBoxAfterDate" ID="CalendarExtenderAfterDate" PopupPosition="BottomRight" />
                <cc1:CalendarExtender runat="server" TargetControlID="TextBoxBeforeDate" ID="CalendarExtenderBeforeDate" PopupPosition="BottomRight"/>
                
                
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="#333333" 
                    GridLines="None" AllowPaging="True" PageSize="100" AllowSorting="True" EnableModelValidation="True"
                >
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:CommandField ButtonType="Button" SelectText="Edit" ShowSelectButton="True" />
                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" 
                        DataFormatString="{0:d}" />
                    <asp:BoundField DataField="Fab" HeaderText="Fab" SortExpression="Fab" />
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                    <asp:BoundField DataField="Waferlog" HeaderText="Waferlog" SortExpression="Waferlog" />
                    <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" />
                    <asp:BoundField DataField="PackingSlip" HeaderText="PackingSlip" SortExpression="PackingSlip" />
                    <asp:BoundField DataField="Carrier" HeaderText="Carrier" SortExpression="Carrier" />
                    <asp:BoundField DataField="Note" HeaderText="Note" SortExpression="Note" />
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="LightBlue" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                SelectCommand="SELECT dbo.T_WH_Invintory.EventTime AS Date, dbo.MainID.CustomerID AS Fab, dbo.T_WH_Invintory.MainID AS ID, dbo.T_WH_Invintory.Waferlog, dbo.T_WH_Invintory.Qty, dbo.T_WH_Invintory.PackingSlip, dbo.T_WH_Invintory.Carrier, dbo.T_WH_Invintory.Note FROM dbo.T_WH_Invintory INNER JOIN dbo.MainID ON dbo.T_WH_Invintory.MainID = dbo.MainID.MainID INNER JOIN dbo.Customer ON dbo.MainID.CustomerID = dbo.Customer.CustomerID WHERE (dbo.T_WH_Invintory.Action = N'StartWL') AND (dbo.T_WH_Invintory.EventTime > CONVERT (DATETIME, '2007-01-01 00:00:00', 102)) ORDER BY dbo.T_WH_Invintory.EventTime DESC">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSourceFabs" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                    SelectCommand="SELECT dbo.MainID.CustomerID AS Fab FROM dbo.T_WH_Invintory INNER JOIN dbo.MainID ON dbo.T_WH_Invintory.MainID = dbo.MainID.MainID GROUP BY dbo.MainID.CustomerID ORDER BY dbo.MainID.CustomerID">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSourceID" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                    SelectCommand="SELECT MainID AS ID FROM dbo.T_WH_Invintory GROUP BY MainID ORDER BY MainID">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSourceCustomers" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                    SelectCommand="SELECT dbo.Customer.Customer_Name FROM dbo.T_WH_Invintory INNER JOIN dbo.MainID ON dbo.T_WH_Invintory.MainID = dbo.MainID.MainID INNER JOIN dbo.Customer ON dbo.MainID.CustomerID = dbo.Customer.CustomerID GROUP BY dbo.Customer.Customer_Name ORDER BY dbo.Customer.Customer_Name">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSourceSize" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                    SelectCommand="SELECT dbo.MainID.Diameter FROM dbo.T_WH_Invintory INNER JOIN dbo.MainID ON dbo.T_WH_Invintory.MainID = dbo.MainID.MainID GROUP BY dbo.MainID.Diameter ORDER BY dbo.MainID.Diameter">
                </asp:SqlDataSource>
                 <asp:Label ID="LabelSQL" runat="server" Text=""  
                                    Width="1px"></asp:Label>
                
                    
        </asp:Panel>
        </ContentTemplate> 
    </asp:UpdatePanel>
</asp:Content>

