<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="T7_Check_Criteria.aspx.vb" Inherits="DBMaintenance_T7_Check_Criteria" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="PanelMain" runat="server">
        
            T7 Criterias By Stage<table class="style1">
                <tr>
                    <td>
                        Select ID:&nbsp;
                        <asp:DropDownList ID="DropDownListIDSelect" runat="server" 
                            AppendDataBoundItems="True" DataSourceID="SqlDataSource2" 
                            DataTextField="MainID" DataValueField="MainID">
                            <asp:ListItem>Select One...</asp:ListItem>
                            <asp:ListItem>Add...</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                            BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" 
                            CellPadding="4" DataKeyNames="Key" DataSourceID="SqlDataSource1">
                            <RowStyle BackColor="White" ForeColor="#003399" />
                            <Columns>
                                <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" 
                                    ReadOnly="True" SortExpression="Key" />
                                <asp:BoundField DataField="MainID" HeaderText="MainID" 
                                    SortExpression="MainID" />
                                <asp:BoundField DataField="Stage" HeaderText="Stage" SortExpression="Stage" />
                                <asp:CheckBoxField DataField="Needed" HeaderText="Needed" 
                                    SortExpression="Needed" />
                                <asp:BoundField DataField="Chr1" HeaderText="Chr1" SortExpression="Chr1" />
                                <asp:BoundField DataField="Chr2" HeaderText="Chr2" SortExpression="Chr2" />
                                <asp:BoundField DataField="Chr3" HeaderText="Chr3" SortExpression="Chr3" />
                                <asp:BoundField DataField="Chr4" HeaderText="Chr4" SortExpression="Chr4" />
                                <asp:BoundField DataField="Chr5" HeaderText="Chr5" SortExpression="Chr5" />
                                <asp:BoundField DataField="Chr6" HeaderText="Chr6" SortExpression="Chr6" />
                                <asp:BoundField DataField="Chr7" HeaderText="Chr7" SortExpression="Chr7" />
                                <asp:BoundField DataField="Chr8" HeaderText="Chr8" SortExpression="Chr8" />
                                <asp:BoundField DataField="Chr9" HeaderText="Chr9" SortExpression="Chr9" />
                                <asp:BoundField DataField="Chr10" HeaderText="Chr10" SortExpression="Chr10" />
                                <asp:BoundField DataField="Message" HeaderText="Message" 
                                    SortExpression="Message" />
                                <asp:BoundField DataField="DateStamp" HeaderText="DateStamp" 
                                    SortExpression="DateStamp" />
                            </Columns>
                            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
            <br />
            <br />
            <br />
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                SelectCommand="SELECT MainID FROM dbo.T_T7_Check_Criteria GROUP BY MainID">
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                DeleteCommand="DELETE FROM [T_T7_Check_Criteria] WHERE [Key] = @Key" 
                InsertCommand="INSERT INTO [T_T7_Check_Criteria] ([MainID], [Stage], [Needed], [Chr1], [Chr2], [Chr3], [Chr4], [Chr5], [Chr6], [Chr7], [Chr8], [Chr9], [Chr10], [Message], [DateStamp]) VALUES (@MainID, @Stage, @Needed, @Chr1, @Chr2, @Chr3, @Chr4, @Chr5, @Chr6, @Chr7, @Chr8, @Chr9, @Chr10, @Message, @DateStamp)" 
                SelectCommand="SELECT [Key], [MainID], [Stage], [Needed], [Chr1], [Chr2], [Chr3], [Chr4], [Chr5], [Chr6], [Chr7], [Chr8], [Chr9], [Chr10], [Message], [DateStamp] FROM [T_T7_Check_Criteria]" 
                UpdateCommand="UPDATE [T_T7_Check_Criteria] SET [MainID] = @MainID, [Stage] = @Stage, [Needed] = @Needed, [Chr1] = @Chr1, [Chr2] = @Chr2, [Chr3] = @Chr3, [Chr4] = @Chr4, [Chr5] = @Chr5, [Chr6] = @Chr6, [Chr7] = @Chr7, [Chr8] = @Chr8, [Chr9] = @Chr9, [Chr10] = @Chr10, [Message] = @Message, [DateStamp] = @DateStamp WHERE [Key] = @Key">
                <DeleteParameters>
                    <asp:Parameter Name="Key" Type="Int32" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="MainID" Type="String" />
                    <asp:Parameter Name="Stage" Type="String" />
                    <asp:Parameter Name="Needed" Type="Boolean" />
                    <asp:Parameter Name="Chr1" Type="String" />
                    <asp:Parameter Name="Chr2" Type="String" />
                    <asp:Parameter Name="Chr3" Type="String" />
                    <asp:Parameter Name="Chr4" Type="String" />
                    <asp:Parameter Name="Chr5" Type="String" />
                    <asp:Parameter Name="Chr6" Type="String" />
                    <asp:Parameter Name="Chr7" Type="String" />
                    <asp:Parameter Name="Chr8" Type="String" />
                    <asp:Parameter Name="Chr9" Type="String" />
                    <asp:Parameter Name="Chr10" Type="String" />
                    <asp:Parameter Name="Message" Type="String" />
                    <asp:Parameter Name="DateStamp" Type="DateTime" />
                    <asp:Parameter Name="Key" Type="Int32" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="MainID" Type="String" />
                    <asp:Parameter Name="Stage" Type="String" />
                    <asp:Parameter Name="Needed" Type="Boolean" />
                    <asp:Parameter Name="Chr1" Type="String" />
                    <asp:Parameter Name="Chr2" Type="String" />
                    <asp:Parameter Name="Chr3" Type="String" />
                    <asp:Parameter Name="Chr4" Type="String" />
                    <asp:Parameter Name="Chr5" Type="String" />
                    <asp:Parameter Name="Chr6" Type="String" />
                    <asp:Parameter Name="Chr7" Type="String" />
                    <asp:Parameter Name="Chr8" Type="String" />
                    <asp:Parameter Name="Chr9" Type="String" />
                    <asp:Parameter Name="Chr10" Type="String" />
                    <asp:Parameter Name="Message" Type="String" />
                    <asp:Parameter Name="DateStamp" Type="DateTime" />
                </InsertParameters>
            </asp:SqlDataSource>
            <br />
            <br />
        
        </asp:Panel>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

