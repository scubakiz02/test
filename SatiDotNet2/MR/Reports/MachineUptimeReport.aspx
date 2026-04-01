
<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MachineUptimeReport.aspx.vb" Inherits="MR_Reports_MachineUptimeReport" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Style="clear: none; position: static" Width="915px">
                            
                <asp:Label ID="Label1" runat="server" Text="Machine Uptime Report" Font-Size="X-Large"></asp:Label><br />                
                <br />
                Select Your Date Range: <br /> <br />                          
                <table>
                    <tr>
                        <td>&nbsp;From:&nbsp;&nbsp;<asp:Label ID="LabelFromDate" runat="server" Text="" BackColor="#FFFF99"></asp:Label></td>
                        <td>&nbsp;To:&nbsp;&nbsp;<asp:Label ID="LabelToDate" runat="server" Text="" BackColor="#FFFF99"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>&nbsp;<asp:Calendar ID="CalendarFrom" runat="server"></asp:Calendar></td>
                        <td>&nbsp;<asp:Calendar ID="CalendarTo" runat="server"></asp:Calendar></td>
                    </tr>
                </table><br />
                <asp:Button ID="ButtonRun" runat="server" Text="Run Report" Visible="False" />
                &nbsp;<br />
                <br />
            </asp:Panel>
            <asp:Panel ID="PanelRecords" runat="server">
                <br />
                <br />
                <asp:GridView ID="GridView1" runat="server" DataKeyNames="MR_Key" 
                    DataSourceID="SqlDataSource_MR_By_Date_Range" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" AutoGenerateColumns="False">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    
                    <Columns>
                        
                        <asp:ButtonField ButtonType="Button" Text="View" CommandName="ViewMR" />
                       
                        <asp:BoundField DataField="Department" HeaderText="Department" 
                            SortExpression="Department" />
                        
                        <asp:BoundField DataField="Tool" HeaderText="Tool" 
                            SortExpression="Tool" />
                                                
                    
                         <asp:BoundField DataField="MR_Key" HeaderText="MR#"  />
                        
                        <asp:BoundField DataField="IssueDate" HeaderText="IssueDate" 
                            SortExpression="IssueDate" />
                        
                        <asp:BoundField DataField="CloseDate" HeaderText="CloseDate" 
                            SortExpression="CloseDate" />
                        
                        <asp:BoundField DataField="MinutesDown" HeaderText="Minutes" 
                            SortExpression="MinutesDown" ReadOnly="True" />
                        
                        <asp:BoundField DataField="HoursDown" HeaderText="Hours" 
                            SortExpression="HoursDown" ReadOnly="True" />
                        
                        <asp:BoundField DataField="DaysDown" HeaderText="Days" ReadOnly="True" 
                            SortExpression="DaysDown" />
                        <asp:BoundField DataField="Status" HeaderText="Status" 
                            SortExpression="Status" />
                        
                        <asp:CheckBoxField DataField="ReportOK" HeaderText="Report" 
                            SortExpression="ReportOK" />
                   
                       
                    </Columns>
                    
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    
                </asp:GridView>
                <asp:Button runat="server" Text="Export" ID="ButonExport" />
                
                <asp:Label ID="Labelsql" runat="server" Text="Label" Visible="False"></asp:Label>
                
                <asp:SqlDataSource ID="SqlDataSource_MR_By_Date_Range" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                    
                    
                    SelectCommand="SELECT TOP 100 PERCENT dbo.T_Tools.Department, dbo.T_Tools.Tool, dbo.T_MR_Tickets.MR_Key, dbo.T_MR_Tickets.IssueDate, dbo.T_MR_Tickets.CloseDate, ISNULL(DATEDIFF(mi, CASE WHEN dbo.T_MR_Tickets.IssueDate &lt; CONVERT (DATETIME , '2015-03-01 00:00:00' , 102) THEN CONVERT (DATETIME , '2000-03-01 00:00:00' , 102) ELSE dbo.T_MR_Tickets.IssueDate END, dbo.T_MR_Tickets.CloseDate), DATEDIFF(mi, CASE WHEN dbo.T_MR_Tickets.IssueDate &lt; CONVERT (DATETIME , '2015-03-01 00:00:00' , 102) THEN CONVERT (DATETIME , '2015-03-01 00:00:00' , 102) ELSE dbo.T_MR_Tickets.IssueDate END, { fn NOW() })) AS MinutesDown, ISNULL(DATEDIFF(hh, CASE WHEN dbo.T_MR_Tickets.IssueDate &lt; CONVERT (DATETIME , '2000-03-01 00:00:00' , 102) THEN CONVERT (DATETIME , '2015-03-01 00:00:00' , 102) ELSE dbo.T_MR_Tickets.IssueDate END, dbo.T_MR_Tickets.CloseDate), DATEDIFF(hh, CASE WHEN dbo.T_MR_Tickets.IssueDate &lt; CONVERT (DATETIME , '2015-03-01 00:00:00' , 102) THEN CONVERT (DATETIME , '2015-03-01 00:00:00' , 102) ELSE dbo.T_MR_Tickets.IssueDate END, { fn NOW() })) AS HoursDown, ISNULL(DATEDIFF(DD, CASE WHEN dbo.T_MR_Tickets.IssueDate &lt; CONVERT (DATETIME , '2000-03-01 00:00:00' , 102) THEN CONVERT (DATETIME , '2015-03-01 00:00:00' , 102) ELSE dbo.T_MR_Tickets.IssueDate END, dbo.T_MR_Tickets.CloseDate), DATEDIFF(DD, CASE WHEN dbo.T_MR_Tickets.IssueDate &lt; CONVERT (DATETIME , '2015-03-01 00:00:00' , 102) THEN CONVERT (DATETIME , '2015-03-01 00:00:00' , 102) ELSE dbo.T_MR_Tickets.IssueDate END, { fn NOW() })) AS DaysDown, dbo.T_MR_Tickets.Status, dbo.T_MR_Tickets.ReportOK FROM dbo.T_MR_GroupLists INNER JOIN dbo.T_Tools ON dbo.T_MR_GroupLists.ToolKey = dbo.T_Tools.[Key] INNER JOIN dbo.T_MR_Tickets ON dbo.T_Tools.[Key] = dbo.T_MR_Tickets.Tool WHERE (dbo.T_MR_GroupLists.ListName = 'TerryUpTimeeport') AND (dbo.T_MR_Tickets.IssueDate &gt; CONVERT (DATETIME, '2000-03-01 00:00:00', 102)) AND (dbo.T_MR_Tickets.CloseDate &lt; CONVERT (DATETIME, '2000-04-01 00:00:00', 102) OR dbo.T_MR_Tickets.CloseDate IS NULL) OR (dbo.T_MR_Tickets.IssueDate &lt; CONVERT (DATETIME, '2000-03-01 00:00:00', 102)) AND (dbo.T_MR_Tickets.CloseDate &gt; CONVERT (DATETIME, '2000-03-01 00:00:00', 102)) ORDER BY dbo.T_Tools.Department, dbo.T_Tools.Tool, dbo.T_MR_Tickets.IssueDate" 
                    
                    UpdateCommand="Update_MR_Ticket" UpdateCommandType="StoredProcedure" 
                    OnUpdating="SqlDataSource_MR_By_Date_Range_Updating" 
                    ProviderName="<%$ ConnectionStrings:ALTSConnectionString.ProviderName %>">
                    
                    <UpdateParameters>
                        <asp:Parameter Name="MKey" Type="Int32" />
                        <asp:Parameter Name="C_Date" Type="DateTime" />
                        <asp:Parameter Name="R_OK" Type="Boolean" />
                    </UpdateParameters>
                </asp:SqlDataSource>
                
                &nbsp;</asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

