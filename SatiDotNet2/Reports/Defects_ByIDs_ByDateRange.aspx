<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="Defects_ByIDs_ByDateRange.aspx.vb" Inherits="Reports_Defects_ByIDs_ByDateRange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <contenttemplate>
            <asp:Panel ID="Panel1" runat="server">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Defects By ID's and By Date Range"></asp:Label><br />
                <br />
                
               
                &nbsp;  &nbsp;  &nbsp;  &nbsp;  &nbsp;  IDs:  &nbsp; <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox> &nbsp;Example: "3143;3144;3601;3602;" or just "3143;" <br />
                Start Date:&nbsp; <asp:TextBox ID="TextBoxDateStart" runat="server"></asp:TextBox>  <br />
                &nbsp; End Date: <asp:TextBox ID="TextBoxDateEnd" runat="server"></asp:TextBox><br />
                
                <asp:Button ID="ButtonRun" runat="server" Text="Run" />
               
                          
                <asp:Panel ID="Panel3" runat="server">
                    <asp:HyperLink ID="HyperLinkReport" runat="server" Visible="False">View in Excel</asp:HyperLink>
                </asp:Panel>
               
                <br />
                
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataSourceID="SqlDataSource1">
                    <Columns>

                        <asp:BoundField DataField="DefectName" HeaderText="DefectName" SortExpression="DefectName" />
                        <asp:BoundField DataField="code" HeaderText="code" SortExpression="code" />
                        <asp:BoundField DataField="QTY_Sum" HeaderText="QTY_Sum" SortExpression="QTY_Sum" ReadOnly="True" />
                        
                    </Columns>
                    <FooterStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />                    
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <RowStyle ForeColor="#000066" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT TOP (100) PERCENT DefectTracking.DefectName, DefectTracking.Location AS code, SUM(DefectTracking.Qty) AS QTY_Sum FROM UniqueProcesses INNER JOIN WaferMover ON UniqueProcesses.LotEntry = WaferMover.LotEntry AND UniqueProcesses.ProcessOrder = WaferMover.[Order] INNER JOIN DefectTracking ON WaferMover.MovementEntry = DefectTracking.MovementEntry WHERE (UniqueProcesses.LotEntry LIKE N'9999999%') AND (UniqueProcesses.Complete &gt;= CONVERT (DATETIME, '2018-12-30 00:00:00', 102)) AND (UniqueProcesses.Complete &lt; CONVERT (DATETIME, '2019-03-30 23:59:59', 102)) GROUP BY DefectTracking.DefectName, DefectTracking.Location ORDER BY QTY_Sum DESC"></asp:SqlDataSource>
            
            </asp:Panel>
           
          
        <asp:UpdateProgress id="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <IMG src="../Color/Animated_LoadingBigger.gif" />Working...
            </ProgressTemplate>
        </asp:UpdateProgress>
        </contenttemplate>
</asp:Content>

