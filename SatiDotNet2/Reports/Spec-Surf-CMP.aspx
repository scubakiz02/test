<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="Spec-Surf-CMP.aspx.vb" Inherits="Reports_Spec_Surf_CMP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="Panel1" runat="server">
        
            <asp:Label ID="Label1" runat="server" Text="Last 1,000 Spec Scan with Surf & CMP" Font-Size="Larger"></asp:Label><br />
            <br /><br />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                CellPadding="4" DataSourceID="SqlDataSource1" ForeColor="#333333" 
                GridLines="None" Height="181px">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="Timestamp" HeaderText="Timestamp" 
                        SortExpression="Timestamp" >
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Scan" HeaderText="Scan" SortExpression="Scan"  >
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Surf Tool" HeaderText="Surf Tool" SortExpression="Surf Tool" >
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Session" HeaderText="Session" SortExpression="Session" >
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Lot" HeaderText="Lot" SortExpression="Lot" >
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Total wafers" HeaderText="Total wafers" SortExpression="Total wafers" >
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="BIN1" HeaderText="BIN1" SortExpression="BIN1" >
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" BackColor="#FFBBBB" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Reject %" HeaderText="Reject %" ReadOnly="True" SortExpression="Reject %" >
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" BackColor="#FFBBBB" />
                    </asp:BoundField>
                    <asp:BoundField DataField="BIN2" HeaderText="BIN2" SortExpression="BIN2" >
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" BackColor="#B3FFB3" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Bin 2%" HeaderText="Bin 2%" ReadOnly="True" SortExpression="Bin 2%" >
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False"  BackColor="#B3FFB3"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="BIN3" HeaderText="BIN3" SortExpression="BIN3" >
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" BackColor="#FFFFBF"  />
                    </asp:BoundField>
                    <asp:BoundField DataField="Bin 3%" HeaderText="Bin 3%" ReadOnly="True" SortExpression="Bin 3%" >
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" BackColor="#FFFFBF" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CMP Tool" HeaderText="CMP Tool" ReadOnly="True" SortExpression="CMP Tool" >
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False"/>
                    </asp:BoundField>
                    <asp:BoundField DataField="Total Pass%" HeaderText="Total Pass%" ReadOnly="True" SortExpression="Total Pass%" >
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" BackColor="#A8BEFF" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CMP_Bind_Time" HeaderText="CMP_Bind_Time" SortExpression="CMP_Bind_Time" >
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Wrap="False" />
                    </asp:BoundField>
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
       
        
        
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                
                SelectCommand="SELECT [Q_Spec-Surf].Timestamp, [Q_Spec-Surf].Scan, [Q_Spec-Surf].[Surf Tool], [Q_Spec-Surf].Session, [Q_Spec-Surf].Lot, [Q_Spec-Surf].[Total wafers], [Q_Spec-Surf].BIN1, 'No Data' AS [Reject %], [Q_Spec-Surf].BIN2, 'No Data' AS [Bin 2%], [Q_Spec-Surf].BIN3, 'No Data' AS [Bin 3%], CASE WHEN Com1 LIKE '%-1' THEN 'CMP1' WHEN Com1 LIKE '%-2' THEN 'CMP2' WHEN Com1 LIKE '%-3' THEN 'CMP3' WHEN Com1 LIKE '%-4L' THEN 'CMP4L' WHEN Com1 LIKE '%-4R' THEN 'CMP4R' WHEN Com1 LIKE '%-5' THEN 'CMP5' ELSE 'No Data' END AS [CMP Tool], 'No Data' AS [Total Pass%], T_InstanceToolBinds.TimeStamp AS CMP_Bind_Time FROM [Q_Spec-Surf] LEFT OUTER JOIN T_InstanceToolBinds ON [Q_Spec-Surf].Scan = CAST(T_InstanceToolBinds.Instance AS nvarchar(20)) ORDER BY [Q_Spec-Surf].Timestamp DESC">
            </asp:SqlDataSource>
       
        
        
        <br /><br /><br />
        </asp:Panel>
      
    </ContentTemplate>
    
    </asp:UpdatePanel>

</asp:Content>


