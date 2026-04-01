<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SPC_Record_Maintenance.aspx.vb" Inherits="SPC_SPC_Record_Maintenance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <asp:Label ID="Label0" runat="server" Text="SPC Record Maintenance" Font-Bold="True" Font-Size="X-Large"></asp:Label><br />
            </asp:Panel><br />
             
            Select Tool to view Records:
            <asp:DropDownList ID="DropDownListTool" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="Tool_Name" DataValueField="Key" AppendDataBoundItems="True">
                
            </asp:DropDownList><br />
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SATI_SPCConnectionString %>" SelectCommand="SELECT [Key], Tool_Name FROM T_SPC_Tool_Info WHERE (Enable = 1) ORDER BY Tool_Name"></asp:SqlDataSource>
               
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSourceRecords" ForeColor="#333333" GridLines="None" DataKeyNames="Key">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" />
                    <asp:BoundField DataField="Key" HeaderText="Key" SortExpression="Key" InsertVisible="False" ReadOnly="True" />
                    <asp:BoundField DataField="DSC" HeaderText="DSC" SortExpression="DSC" />
                    <asp:BoundField DataField="Recipe" HeaderText="Recipe" SortExpression="Recipe" />
                    <asp:BoundField DataField="Seq" HeaderText="Seq" SortExpression="Seq" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="Para" HeaderText="Para" SortExpression="Para" />
                    <asp:BoundField DataField="LCL" HeaderText="LCL" SortExpression="LCL" />
                    <asp:BoundField DataField="Value" HeaderText="Value" SortExpression="Value" />
                    <asp:BoundField DataField="UCL" HeaderText="UCL" SortExpression="UCL" />
                    <asp:CommandField ShowEditButton="True" />
                </Columns>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>




            <asp:SqlDataSource ID="SqlDataSourceRecords" runat="server" ConnectionString="<%$ ConnectionStrings:SATI_SPCConnectionString %>" SelectCommand="SELECT [Key], [DSC], [Recipe], [Seq], [Name], [Para], [LCL], [Value], [UCL] FROM [T_SPC_DataPoints] WHERE ([Tool_Key] = @Tool_Key) ORDER BY [DSC] DESC, [Seq]" DeleteCommand="DELETE FROM [T_SPC_DataPoints] WHERE [Key] = @Key" InsertCommand="INSERT INTO [T_SPC_DataPoints] ([DSC], [Recipe], [Seq], [Name], [Para], [LCL], [Value], [UCL]) VALUES (@DSC, @Recipe, @Seq, @Name, @Para, @LCL, @Value, @UCL)" UpdateCommand="UPDATE [T_SPC_DataPoints] SET [DSC] = @DSC, [Recipe] = @Recipe, [Seq] = @Seq, [Name] = @Name, [Para] = @Para, [LCL] = @LCL, [Value] = @Value, [UCL] = @UCL WHERE [Key] = @Key">
                <DeleteParameters>
                    <asp:Parameter Name="Key" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="DSC" Type="String" />
                    <asp:Parameter Name="Recipe" Type="String" />
                    <asp:Parameter Name="Seq" Type="Int32" />
                    <asp:Parameter Name="Name" Type="String" />
                    <asp:Parameter Name="Para" Type="String" />
                    <asp:Parameter Name="LCL" Type="Double" />
                    <asp:Parameter Name="Value" Type="Double" />
                    <asp:Parameter Name="UCL" Type="Double" />
                </InsertParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="DropDownListTool" Name="Tool_Key" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="DSC" Type="String" />
                    <asp:Parameter Name="Recipe" Type="String" />
                    <asp:Parameter Name="Seq" Type="Int32" />
                    <asp:Parameter Name="Name" Type="String" />
                    <asp:Parameter Name="Para" Type="String" />
                    <asp:Parameter Name="LCL" Type="Double" />
                    <asp:Parameter Name="Value" Type="Double" />
                    <asp:Parameter Name="UCL" Type="Double" />
                    <asp:Parameter Name="Key" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>




        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
