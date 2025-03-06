<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MR_View.aspx.vb" Inherits="MR_MR_View" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Maintenance Request Viewer"></asp:Label><br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="912px">
               <br />
                <table class="style1">
                    <tr>
                        <td>
                             Ticket Type:&nbsp;
                <asp:RadioButton ID="OpenTicketsRadioButton" runat="server" Checked="True" GroupName="Tickets"
                    Text="Open Tickets" AutoPostBack="True" OnCheckedChanged="OpenTicketsRadioButton_CheckedChanged" />&nbsp;<asp:RadioButton ID="ClosedRadioButton" runat="server"
                        GroupName="Tickets" Text="Closed Tickets" AutoPostBack="True" OnCheckedChanged="ClosedRadioButton_CheckedChanged" />&nbsp;&nbsp;&nbsp;
                            StartDate:&nbsp;<asp:TextBox ID="TextBoxStartDate" runat="server" Width="79px"></asp:TextBox>&nbsp;&nbsp; 
                            End Date:&nbsp;<asp:TextBox ID="TextBoxEndDate" runat="server" Width="79px"></asp:TextBox>&nbsp;&nbsp;
                            
                            <asp:Button ID="ButtonRefresh" runat="server" Text="Refresh Data" />
                        </td>
                        <td>
                            &nbsp;
                            </td>
                    </tr>
                    <tr>
                        <td>
                            Ticket Status:&nbsp;
                <asp:CheckBox ID="DownCheckBox" runat="server" Checked="True" Text="Tools Down" AutoPostBack="True" OnCheckedChanged="DownCheckBox_CheckedChanged" />&nbsp;
                <asp:CheckBox ID="StandardCheckBox" runat="server" Checked="True" Text="Standard Request" AutoPostBack="True" OnCheckedChanged="StandardCheckBox_CheckedChanged" />&nbsp;
                <asp:CheckBox ID="ScheduledCheckBox" runat="server" Checked="True" Text="Scheduled Maintenance" AutoPostBack="True" OnCheckedChanged="ScheduledCheckBox_CheckedChanged" />&nbsp;
                &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="CheckBoxSort" runat="server" text=" Sort Issue Date (Decending)" Checked="True" AutoPostBack="True" />
                &nbsp;&nbsp;<br />
                
                
                <asp:DropDownList ID="DropDownListTools" runat="server" AutoPostBack="True" 
                    DataSourceID="SqlDataSource1" DataTextField="Tool" DataValueField="Tool">
                    
                </asp:DropDownList>
                <asp:CheckBox ID="CheckBoxToolOnly" runat="server" Text="View single tool" AutoPostBack="True" />           
                
                </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <br />
                               
                
                
                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    CellPadding="4" DataKeyNames="Ticket#" DataSourceID="TicketsSqlDataSource" ForeColor="#333333"
                    GridLines="None" Style="border-right: thin solid;
                    border-top: thin solid; border-left: thin solid; border-bottom: thin solid">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" />
                        <asp:BoundField DataField="Ticket#" HeaderText="Ticket#" InsertVisible="False" ReadOnly="True"
                            SortExpression="Ticket#" />
                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                        <asp:BoundField DataField="Tool" HeaderText="Tool" SortExpression="Tool" />
                        <asp:BoundField DataField="IssueDate" HeaderText="IssueDate" SortExpression="IssueDate" />
                        <asp:TemplateField HeaderText="Note" SortExpression="Note">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Note") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Text='<%# Bind("Note") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="LightBlue" />
                </asp:GridView>
                <br />
                <br />
                <asp:SqlDataSource ID="TicketsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT dbo.T_MR_Tickets.MR_Key AS Ticket#, dbo.T_Tools.Tool, dbo.T_MR_Tickets.Status, dbo.T_MR_TicketNotes.Note, dbo.T_MR_Tickets.IssueDate FROM dbo.T_MR_Tickets INNER JOIN dbo.T_Tools ON dbo.T_MR_Tickets.Tool = dbo.T_Tools.[Key] INNER JOIN dbo.T_MR_TicketNotes ON dbo.T_MR_Tickets.MR_Key = dbo.T_MR_TicketNotes.MR_Key WHERE (dbo.T_MR_Tickets.CloseDate IS NULL) AND (dbo.T_MR_TicketNotes.NoteType = 'Org')">
                </asp:SqlDataSource>
                <br />
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                    SelectCommand="SELECT Tool FROM dbo.T_Tools GROUP BY Tool ORDER BY Tool">
                </asp:SqlDataSource>
                <br />
                </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

