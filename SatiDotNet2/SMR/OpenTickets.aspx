<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="OpenTickets.aspx.vb" Inherits="SMR_OpenTickets" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="NotePanel" runat="server" Width="1009px" Visible="True">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Maintenance Request Open Ticket"></asp:Label>&nbsp;                                               
                &nbsp;<asp:Button ID="CloseButton" runat="server" Text="Close Ticket" Visible="False" Style="height: 26px"  />
                &nbsp;
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>
                        &nbsp;<img src="../Color/Animated_LoadingBigger.gif" />Loading...
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:Label ID="infoLabel" runat="server"></asp:Label>
                <br />
                <table cellspacing="0" style="vertical-align: top; text-align: left;">
                    <tr>
                        <td style="text-align: left;">&nbsp;SMR#&nbsp;<asp:Label ID="LabelSMRNumber" runat="server" Text="Label"></asp:Label>&nbsp;</td>
                        <td style="text-align: left;">&nbsp;Tool:&nbsp;<asp:Label ID="LabelTool" runat="server" Text="Label"></asp:Label>&nbsp;</td>
<%--                        <td style="text-align: center; font-weight: bold;">&nbsp;Ticket Type:&nbsp;
                            <asp:DropDownList ID="DropDownListStatus" runat="server" AutoPostBack="True">
                                <asp:ListItem Value="Standard">Standard Ticket</asp:ListItem>
                                <asp:ListItem Value="Down">Down Tool</asp:ListItem>
                            </asp:DropDownList>
                           
                            <asp:Label ID="LabelStatus" runat="server" Text="Label" Visible="False"></asp:Label>&nbsp; 

                        </td>--%>
                    </tr>
                    <tr>
                        <td style="text-align: left;">&nbsp;Date:&nbsp;<asp:Label ID="LabelIssueDate" runat="server" Text="Label"></asp:Label>&nbsp;</td>

                        <td style="text-align: left;">&nbsp;Close Date:&nbsp;<asp:Label ID="LabelClosedDate" runat="server" Text="Label"></asp:Label>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">&nbsp;User:&nbsp;<asp:Label ID="LabelIssueUser" runat="server" Text="Label"></asp:Label>&nbsp;</td>
                        <td style="text-align: left;">&nbsp;Tech User:&nbsp;<asp:Label ID="LabelCloseUser" runat="server" Text="Label"></asp:Label>&nbsp;</td>
                        <td>&nbsp;Ok For Report?&nbsp;<asp:CheckBox ID="CheckBoxReport" runat="server"
                            AutoPostBack="True" /></td>
                    </tr>
                    <tr>
                        <td style="text-align: left; vertical-align: top; font-weight: bold;">&nbsp;User Comment:&nbsp;<br />
                            &nbsp;<asp:TextBox ID="TextBoxUserNote"
                                runat="server" BackColor="LightBlue" Height="125px" TextMode="MultiLine"
                                Width="225px"></asp:TextBox>
                            &nbsp; </td>

                        <td style="text-align: left" colspan="2">
                            <asp:GridView ID="GridView1" runat="server"
                                AutoGenerateColumns="False" CellPadding="4" DataSourceID="NotesSqlDataSource"
                                ForeColor="#333333" GridLines="None" Width="750px"
                                Style="margin-right: 66px">
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:BoundField DataField="NoteDate" HeaderText="NoteDate"
                                        SortExpression="NoteDate">
                                        <ItemStyle Font-Size="Small" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SatiUser" HeaderText="SatiUser"
                                        SortExpression="SatiUser">
                                        <ItemStyle Font-Size="Small" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Note" SortExpression="Note">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Rows="2" Text='<%# Bind("Note") %>'
                                                TextMode="MultiLine" Width="500px"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="LightBlue" />
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="Panel2" runat="server" Width="888px">
                    Add Note:<br />
                    <asp:TextBox ID="NewNoteTextBox" runat="server" Height="40px" TextMode="MultiLine" Width="506px"></asp:TextBox>
                    &nbsp;<asp:Button ID="SubmitButton" runat="server" OnClick="SubmitButton_Click" Text="Submit" />
                    <asp:Label ID="SubmitLabel" runat="server" BackColor="Salmon"></asp:Label>
                </asp:Panel>

                <asp:SqlDataSource ID="NotesSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT NoteDate, SatiUser, Note FROM dbo.T_SMR_TicketNotes WHERE (SMR_Key = 10) AND (NoteType = 'Tech')"></asp:SqlDataSource>

                <asp:SqlDataSource ID="SqlDataSourceReport" runat="server"
                    ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    DeleteCommand="DELETE FROM [T_SMR_Tickets] WHERE [SMR_Key] = @SMR_Key"
                    InsertCommand="INSERT INTO [T_SMR_Tickets] ([ReportOK]) VALUES (@ReportOK)"
                    SelectCommand="SELECT SMR_Key, ReportOK FROM dbo.T_SMR_Tickets WHERE (SMR_Key = 10)"
                    UpdateCommand="UPDATE [T_SMR_Tickets] SET [ReportOK] = @ReportOK WHERE [SMR_Key] = @SMR_Key">
                    <DeleteParameters>
                        <asp:Parameter Name="SMR_Key" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="ReportOK" Type="Boolean" />
                        <asp:Parameter Name="SMR_Key" Type="Int32" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="ReportOK" Type="Boolean" />
                    </InsertParameters>
                </asp:SqlDataSource>

            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

