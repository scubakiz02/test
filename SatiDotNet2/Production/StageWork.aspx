<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="StageWork.aspx.vb" Inherits="Production_StageWork" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            
            <h2 style="text-align: center"><asp:Label ID="LabelStage" runat="server" Text="Department"></asp:Label>                
               

            </h2>
            <table class="style1">
                <tr>
                    <td >
                        <asp:Panel ID="PanelDefect" runat="server">
                            
                            <asp:GridView ID="GridViewDefects" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataSourceID="SqlDataSourceDefectList">
                                <AlternatingRowStyle BackColor="#99CCFF" />
                                <Columns>
                                    <asp:BoundField DataField="Defect" HeaderText="Defect" SortExpression="Defect" />
                                    <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                                    <asp:BoundField DataField="Group" HeaderText="Group" SortExpression="Group" />
                                    <asp:TemplateField HeaderText="Qty">
                                        <ItemTemplate>
                                            <asp:TextBox ID="TextBoxDefectQty" runat="server" Width="50px">0</asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <RowStyle ForeColor="#000066" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                            </asp:GridView>
                            <asp:Button ID="ButtonAddDefect" runat="server" Text="Button" />

                            <asp:SqlDataSource ID="SqlDataSourceDefectList" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT TOP (100) PERCENT T_ID_Defects.Defect, T_ID_Defects.Type, T_ID_Defects.[Group] FROM T_ID_Defects INNER JOIN DefectDefs ON T_ID_Defects.Defect = DefectDefs.DefectName WHERE (T_ID_Defects.ID = '0') AND (DefectDefs.StageName = N'nothing') GROUP BY T_ID_Defects.Defect, T_ID_Defects.Type, T_ID_Defects.[Group] ORDER BY T_ID_Defects.Defect"></asp:SqlDataSource>
                            
                        </asp:Panel>
                    </td>
                    <td  style="vertical-align: top">LOT # <asp:Label ID="LabelLotNumber" runat="server" Text="1234-1234-1234"></asp:Label>
                        <asp:Panel ID="PanelMain" runat="server">
                           <asp:Label ID="Label1" runat="server" Text="In: "></asp:Label>
                            <asp:Label ID="LabelTotalIn" runat="server" Text="0000"></asp:Label>
                            <br /> 
                            <asp:Label ID="Label2" runat="server" Text="Out: "></asp:Label>
                            <asp:Label ID="LabelTotalOut" runat="server" Text="0000"></asp:Label>
                           <br />
                            <asp:Label ID="Label3" runat="server" Text="Defects: "></asp:Label>
                            <asp:Label ID="LabelTotalDefect" runat="server" Text="0000"></asp:Label>
                            <br />
                            <asp:Label ID="Label4" runat="server" Text="Remaining: "></asp:Label>
                            <asp:Label ID="LabelTotalRemaining" runat="server" Text="0000"></asp:Label>
                            <br />
                        </asp:Panel><br />
                                                                    
                        <asp:Panel ID="PanelNotes" runat="server">
                            <asp:TextBox ID="NotesBox" runat="server"></asp:TextBox>
                            <asp:Button ID="Button1" runat="server" Text="Button" />
                        </asp:Panel><br />
                                                 
                        <asp:Panel ID="PanelSplits" runat="server">
                            3
                        </asp:Panel><br />
                       
                        <asp:Panel ID="PanelMerge" runat="server">
                            4
                        </asp:Panel><br />
                        
                        <asp:Panel ID="PanelPartial" runat="server">
                            5
                        </asp:Panel><br />
                       
                    </td>
                </tr>
                <tr>
                    <td style="width: 543px">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <asp:Panel ID="PanelPreviewDefect" runat="server">
                Do you want to make these defects?
                <asp:GridView ID="GridViewDefect_OK" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">   
                    <AlternatingRowStyle BackColor="White" />
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <SortedAscendingCellStyle BackColor="#FDF5AC" />
                    <SortedAscendingHeaderStyle BackColor="#4D0000" />
                    <SortedDescendingCellStyle BackColor="#FCF6C0" />
                    <SortedDescendingHeaderStyle BackColor="#820000" />
                </asp:GridView>
                <asp:Button ID="ButtonDefectOK" runat="server" Text="OK" />
            </asp:Panel>
             </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

