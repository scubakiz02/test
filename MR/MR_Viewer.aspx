<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MR_Viewer.aspx.vb" Inherits="MR_MR_Viewer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Maintenance Request Viewer"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Button2" runat="server" Text="Grid View Status Board" BackColor="#FFFF99" PostBackUrl="~/MR/OpenTicketStatusBoard.aspx" /><br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                
                <table class="style1">
                    <tr >
                        <td valign="top" width="200">
                            <asp:Panel ID="Panel2" runat="server" Width="200px">
                                Ticket Status:<br />
                                <asp:RadioButton 
                                    ID="OpenTicketsRadioButton" 
                                    runat="server" 
                                    Checked="True" 
                                    GroupName="Tickets"
                                    Text="Open" 
                                    AutoPostBack="True" 
                                    OnCheckedChanged="OpenTicketsRadioButton_CheckedChanged" /> &nbsp;
                                
                                <asp:RadioButton 
                                    ID="ClosedRadioButton" 
                                    runat="server"
                                    GroupName="Tickets" 
                                    Text="Closed" 
                                    AutoPostBack="True" 
                                    OnCheckedChanged="ClosedRadioButton_CheckedChanged" />
                                
                            </asp:Panel>
                            <asp:UpdateProgress id="UpdateProgress2" runat="server">
                                <ProgressTemplate>
                                    &nbsp;<IMG src="../Color/Animated_LoadingBigger.gif" />Loading...
                                </ProgressTemplate>
                            </asp:UpdateProgress>  
                        </td>
                        <td valign="top"  width="200">
                            <asp:Panel ID="Panel3" runat="server"  Width="200px">
                                StartDate:&nbsp;<asp:TextBox ID="TextBoxStartDate" runat="server" Width="79px"></asp:TextBox><br />
                                End Date:&nbsp;<asp:TextBox ID="TextBoxEndDate" runat="server" Width="79px"></asp:TextBox>
                                <asp:Button ID="ButtonRefresh" runat="server" Text="Refresh Data" /><br />
                            </asp:Panel>
                            
                        </td>
                        <td  valign="top" >
                            <asp:Panel ID="Panel4" runat="server"   >
                                <asp:CheckBox ID="CheckBoxToolOnly" runat="server" Text="View single tool" AutoPostBack="True" /><br />
                                <asp:DropDownList ID="DropDownListTools" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="Tool" DataValueField="Tool" Width="225px"></asp:DropDownList><br />
                                <asp:Panel ID="PanelSGT" runat="server" BackColor="#66CCFF" Visible="False" >                                    
                                    <asp:CheckBoxList 
                                        ID="CheckBoxList_SGL" 
                                        runat="server" 
                                        DataSourceID="SqlDataSource_SGN" 
                                        DataTextField="SG_Name" 
                                        DataValueField="SB_Tag" RepeatLayout="Flow" AutoPostBack="True">
                                    </asp:CheckBoxList>
                                </asp:Panel>
                            </asp:Panel> 
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
                        <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" />
                        <asp:BoundField DataField="Status" HeaderText="Ticket Type" SortExpression="Status" />
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
                <br />
                <br />
                <asp:SqlDataSource ID="TicketsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT T_MR_Tickets.MR_Key AS Ticket#, T_Tools.Department, T_Tools.Tool, T_MR_Tickets.Status, T_MR_TicketNotes.Note, T_MR_Tickets.IssueDate FROM T_MR_Tickets INNER JOIN T_Tools ON T_MR_Tickets.Tool = T_Tools.[Key] INNER JOIN T_MR_TicketNotes ON T_MR_Tickets.MR_Key = T_MR_TicketNotes.MR_Key WHERE (T_MR_Tickets.CloseDate IS NULL) AND (T_MR_TicketNotes.NoteType = 'Org')">
                </asp:SqlDataSource>
                
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                    SelectCommand="SELECT Tool FROM dbo.T_Tools GROUP BY Tool ORDER BY Tool">
                </asp:SqlDataSource>
                
                <asp:SqlDataSource ID="SqlDataSource_SGN" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                    SelectCommand="SELECT T_Tool_SubGroup_Tag_Names.SG_Name, T_Tool_SubGroup_Tag_Names.SB_Tag FROM T_Tools INNER JOIN T_Tool_SubGroup_Tag_Names ON T_Tools.[Key] = T_Tool_SubGroup_Tag_Names.Tool_Key WHERE (T_Tools.Tool = 'CMP 1')">
                </asp:SqlDataSource>

            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

