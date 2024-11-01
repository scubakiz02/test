

<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ReceivingNotes.aspx.vb" Inherits="DBMaintenance_ReceivingNotes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Receiving Notes"></asp:Label><br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <asp:RadioButton ID="RadioButtonView" runat="server" Text="View and Edit Notes" Checked="True" GroupName="PanelView" />
                <asp:RadioButton ID="RadioButtonNew" runat="server" Text="Insert New Notes" GroupName="PanelView" />
            </asp:Panel>
            <asp:Panel ID="PanelViewEdit" runat="server" >
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" DataKeyNames="Key" DataSourceID="SqlDataSource1" 
                    ForeColor="#333333" GridLines="None" Width="996px">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:CommandField ShowEditButton="True" />
                    <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" 
                        ReadOnly="True" SortExpression="Key" Visible="False" />
                    <asp:BoundField DataField="Customer_Name" HeaderText="Customer_Name" 
                        SortExpression="Customer_Name" />
                    <asp:TemplateField HeaderText="Note" SortExpression="Note">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Note") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Note") %>' 
                                TextMode="MultiLine" Width="199px" ></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ExpireTime" HeaderText="ExpireTime" 
                        SortExpression="ExpireTime" />
                    <asp:BoundField DataField="UserName" HeaderText="UserName" 
                        SortExpression="UserName" />
                    <asp:BoundField DataField="DataFolder" HeaderText="DataFolder" 
                        SortExpression="DataFolder" />
                    <asp:CheckBoxField DataField="SpecialEvent" HeaderText="SpecialEvent" 
                        SortExpression="SpecialEvent" />
                    <asp:CheckBoxField DataField="SpecialEventCompleat" 
                        HeaderText="SpecialEventCompleat" SortExpression="SpecialEventCompleat" />
                    <asp:BoundField DataField="EventTime" HeaderText="EventTime" 
                        SortExpression="EventTime" />
                </Columns>
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
</asp:GridView>
            
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                    DeleteCommand="DELETE FROM [T_ReceivingNotesList] WHERE [Key] = @Key" 
                    InsertCommand="INSERT INTO [T_ReceivingNotesList] ([Customer_Name], [Note], [EventTime], [ExpireTime], [UserName], [DataFolder], [SpecialEvent], [SpecialEventCompleat]) VALUES (@Customer_Name, @Note, @EventTime, @ExpireTime, @UserName, @DataFolder, @SpecialEvent, @SpecialEventCompleat)" 
                    SelectCommand="SELECT [Key], [Customer_Name], [Note], [EventTime], [ExpireTime], [UserName], [DataFolder], [SpecialEvent], [SpecialEventCompleat] FROM [T_ReceivingNotesList] ORDER BY [Customer_Name], [SpecialEvent] DESC" 
                    UpdateCommand="UPDATE [T_ReceivingNotesList] SET [Customer_Name] = @Customer_Name, [Note] = @Note, [EventTime] = @EventTime, [ExpireTime] = @ExpireTime, [UserName] = @UserName, [DataFolder] = @DataFolder, [SpecialEvent] = @SpecialEvent, [SpecialEventCompleat] = @SpecialEventCompleat WHERE [Key] = @Key">
                    <DeleteParameters>
                        <asp:Parameter Name="Key" Type="Int32" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Customer_Name" Type="String" />
                        <asp:Parameter Name="Note" Type="String" />
                        <asp:Parameter Name="EventTime" Type="DateTime" />
                        <asp:Parameter Name="ExpireTime" Type="DateTime" />
                        <asp:Parameter Name="UserName" Type="String" />
                        <asp:Parameter Name="DataFolder" Type="String" />
                        <asp:Parameter Name="SpecialEvent" Type="Boolean" />
                        <asp:Parameter Name="SpecialEventCompleat" Type="Boolean" />
                        <asp:Parameter Name="Key" Type="Int32" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="Customer_Name" Type="String" />
                        <asp:Parameter Name="Note" Type="String" />
                        <asp:Parameter Name="EventTime" Type="DateTime" />
                        <asp:Parameter Name="ExpireTime" Type="DateTime" />
                        <asp:Parameter Name="UserName" Type="String" />
                        <asp:Parameter Name="DataFolder" Type="String" />
                        <asp:Parameter Name="SpecialEvent" Type="Boolean" />
                        <asp:Parameter Name="SpecialEventCompleat" Type="Boolean" />
                    </InsertParameters>
                </asp:SqlDataSource>
            
            </asp:Panel>
            <asp:Panel ID="PanelNewNote" runat="server">
            
            
            
            </asp:Panel>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <img src="../Color/Animated_LoadingBigger.gif" />Working...
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>



