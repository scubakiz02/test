<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="OpenTRQuickView.aspx.vb" Inherits="MR_OpenMRQuickView" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 100%">
                <tr>
                    <td>&nbsp;<asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="IT Request Quick View"></asp:Label></td>
                    <td style="text-align: right"><asp:Button ID="Button2" runat="server" Text="Grid View Status Board" BackColor="#FFFF99" PostBackUrl="~/TR/OpenTicketStatusBoard.aspx" />&nbsp;</td>
                </tr>
            </table>           
            <br />
            <asp:Panel ID="Panel1" runat="server">
                               
                <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    CellPadding="4" DataKeyNames="Ticket#" DataSourceID="TicketsSqlDataSource" ForeColor="#333333"
                    GridLines="None" Style="border-right: thin solid;
                    border-top: thin solid; border-left: thin solid; border-bottom: thin solid">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:BoundField DataField="Ticket#" HeaderText="Ticket#" InsertVisible="False" ReadOnly="True"
                            SortExpression="Ticket#" />
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
                    SelectCommand="SELECT dbo.T_ITR_Tickets.MR_Key AS Ticket#, dbo.T_IT_Tools.Tool, dbo.T_ITR_Tickets.Status, dbo.T_ITR_TicketNotes.Note, dbo.T_ITR_Tickets.IssueDate FROM dbo.T_ITR_Tickets INNER JOIN dbo.T_IT_Tools ON dbo.T_ITR_Tickets.Tool = dbo.T_IT_Tools.[Key] INNER JOIN dbo.T_ITR_TicketNotes ON dbo.T_ITR_Tickets.MR_Key = dbo.T_ITR_TicketNotes.MR_Key WHERE (dbo.T_ITR_Tickets.CloseDate IS NULL) AND (dbo.T_ITR_TicketNotes.NoteType = 'Org')">
                </asp:SqlDataSource>
                
               
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

