<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MR_ToolSub_Group.aspx.vb" Inherits="DBMaintenance_MR_ToolSub_Group" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="2000px">
                Add Sub Group Tag to Tool.<br />
                <br />
                Current List.<br />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Key" DataSourceID="SqlDataSource_SG_List" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="Tool" HeaderText="Tool" SortExpression="Tool" />
                        <asp:BoundField DataField="SG_Name" HeaderText="SG_Name" SortExpression="SG_Name" />
                        <asp:BoundField DataField="SB_Tag" HeaderText="SB_Tag" SortExpression="SB_Tag" />
                        <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True" SortExpression="Key" />
                    </Columns>
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource_SG_List" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT T_Tools.Tool, T_Tool_SubGroup_Tag_Names.SG_Name, T_Tool_SubGroup_Tag_Names.SB_Tag, T_Tool_SubGroup_Tag_Names.[Key] FROM T_Tools INNER JOIN T_Tool_SubGroup_Tag_Names ON T_Tools.[Key] = T_Tool_SubGroup_Tag_Names.Tool_Key"></asp:SqlDataSource>
                <br />
                <asp:Button ID="Button1" runat="server" Text="Add Tag" /><br />
                
                <asp:Panel ID="Panel2" runat="server" Visible="False" BackColor="#00CCFF" Width="300" >
                    <br />
                    Select a Tool.<br />
                    <asp:DropDownList ID="DropDownListTools" runat="server" Width="250px" AutoPostBack="True" DataSourceID="SqlDataSourceTools" DataTextField="Tool" DataValueField="Key"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSourceTools" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT [Key], Tool FROM T_Tools WHERE ([Key] = 0) ORDER BY Tool"></asp:SqlDataSource>
                    <br />
                    <br />
                    Sub Group (SG) Name. <br />
                    <asp:TextBox ID="TextBox_SGN" runat="server" Width="250"></asp:TextBox><br />
                    <br />
                    Tag.<br />
                    <asp:TextBox ID="TextBox_TAG" runat="server" Width="75"></asp:TextBox><br />
                    <br />
                    <asp:Button ID="Button_add" runat="server" Text="Add" />
                    <asp:Label ID="LabelInfo" runat="server" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Button_Close" runat="server" Text="Close" />
                    <br />
                    <br />
                </asp:Panel>                             

            </asp:Panel>            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

