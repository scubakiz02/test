<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="CleanroomClimate.aspx.vb" Inherits="ToolData_CleanroomClimate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
            <asp:Panel ID="Panel1" runat="server">
                <asp:Label ID="Label2" runat="server" Text="Cleanroom Climate" Font-Size="Larger"></asp:Label> 
                <br />
                last 48hr<br />
                
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSource1" GridLines="Horizontal" BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="3px">
                    <Columns>
                        <asp:BoundField DataField="TimeStamp" HeaderText="Sample Time" SortExpression="TimeStamp" />
                        <asp:TemplateField ItemStyle-BackColor="Black" HeaderText="CR1">
                            <HeaderStyle BackColor="Black" />
                            <ItemStyle BackColor="Black" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CR1_Temp" HeaderText="Temp" SortExpression="CR1_Temp" />
                        <asp:BoundField DataField="CR1_Humidity" HeaderText="RH%" SortExpression="CR1_Humidity" />
                        <asp:TemplateField ItemStyle-BackColor="Black" HeaderText="CR2">
                            <HeaderStyle BackColor="Black" />
                            <ItemStyle BackColor="Black" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CR2_Temp" HeaderText="Temp" SortExpression="CR2_Temp" />
                        <asp:BoundField DataField="CR2_Humidity" HeaderText="RH%" SortExpression="CR2_Humidity" />
                        <asp:TemplateField ItemStyle-BackColor="Black" HeaderText="CR3">
                            <HeaderStyle BackColor="Black" />
                            <ItemStyle BackColor="Black" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="CR3_Temp" HeaderText="Temp" SortExpression="CR3_Temp" />
                        <asp:BoundField DataField="CR3_Humidity" HeaderText="RH%" SortExpression="CR3_Humidity" />
                        
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#333333" />
                    <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="White" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#487575" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#275353" />
                </asp:GridView>
    



                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:SatiToolsConnectionString %>" SelectCommand="SELECT TimeStamp, CR1_Temp, CR1_Humidity, CR2_Temp, CR2_Humidity, CR3_Temp, CR3_Humidity FROM Q_CR_Climate WHERE (TimeStamp &gt; DATEADD(dd, - 1, GETDATE())) ORDER BY TimeStamp DESC"></asp:SqlDataSource>
    



                <asp:UpdateProgress id="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <IMG src="../Color/Animated_LoadingBigger.gif" />Working...
                    </ProgressTemplate>
                </asp:UpdateProgress>


            </asp:Panel>

        </contenttemplate>
    </asp:UpdatePanel>    
</asp:Content>