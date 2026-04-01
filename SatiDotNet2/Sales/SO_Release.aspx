<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SO_Release.aspx.vb" Inherits="Sales_SO_Release" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelMain" runat="server">  
                <asp:Panel ID="PanelHeader" runat="server">                    
                    &nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="Release SO" Font-Bold="True" Font-Size="Large"></asp:Label>                    
                    
                </asp:Panel>
                    <cc1:RoundedCornersExtender ID="PanelMain_RoundedCornersExtender" runat="server" BehaviorID="PanelMain_RoundedCornersExtender" TargetControlID="PanelHeader" BorderColor="Black" Color="SlateGray" Radius="10">
                    </cc1:RoundedCornersExtender>            
                 <br />
                
                <asp:Panel ID="PanelAdd" runat="server">
                    
                    <table class="style1">
                        <tr>
                            <td>
                                Avalible For Release:&nbsp;
                            </td>
                            
                         </tr>
                         <tr>
                           
                             <td>
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Key" DataSourceID="SqlDataSource1" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                                    <Columns>
                                        <asp:ButtonField CommandName="Release" Text="Release"></asp:ButtonField>
                                        <asp:BoundField DataField="MainID" HeaderText="MainID" SortExpression="MainID"></asp:BoundField>
                                        <asp:BoundField DataField="SO" HeaderText="SO" SortExpression="SO"></asp:BoundField>
                                        <asp:BoundField DataField="PO" HeaderText="PO" SortExpression="PO"></asp:BoundField>
                                        <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty"></asp:BoundField>
                                        <asp:BoundField DataField="Note" HeaderText="Note" SortExpression="Note"></asp:BoundField>
                                        <asp:BoundField DataField="DateStamp" HeaderText="DateStamp" SortExpression="DateStamp"></asp:BoundField>
                            
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066"></FooterStyle>

                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"></HeaderStyle>

                                    <PagerStyle HorizontalAlign="Left" BackColor="White" ForeColor="#000066"></PagerStyle>

                                    <RowStyle ForeColor="#000066"></RowStyle>

                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White"></SelectedRowStyle>

                                    <SortedAscendingCellStyle BackColor="#F1F1F1"></SortedAscendingCellStyle>

                                    <SortedAscendingHeaderStyle BackColor="#007DBB"></SortedAscendingHeaderStyle>

                                    <SortedDescendingCellStyle BackColor="#CAC9C9"></SortedDescendingCellStyle>

                                    <SortedDescendingHeaderStyle BackColor="#00547E"></SortedDescendingHeaderStyle>
                                </asp:GridView>&nbsp;
                            </td>
                         </tr>
                    </table>
                   
                                      
                   

                    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:ALTSConnectionString %>' SelectCommand="SELECT * FROM [T_SO_Future_List]" DeleteCommand="DELETE FROM [T_SO_Future_List] WHERE [Key] = @Key" InsertCommand="INSERT INTO [T_SO_Future_List] ([MainID], [SO], [PO], [Qty], [Note], [DateStamp]) VALUES (@MainID, @SO, @PO, @Qty, @Note, @DateStamp)" UpdateCommand="UPDATE [T_SO_Future_List] SET [MainID] = @MainID, [SO] = @SO, [PO] = @PO, [Qty] = @Qty, [Note] = @Note, [DateStamp] = @DateStamp WHERE [Key] = @Key">
                        <DeleteParameters>
                            <asp:Parameter Name="Key" Type="Int32"></asp:Parameter>
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="MainID" Type="String"></asp:Parameter>
                            <asp:Parameter Name="SO" Type="String"></asp:Parameter>
                            <asp:Parameter Name="PO" Type="String"></asp:Parameter>
                            <asp:Parameter Name="Qty" Type="Int32"></asp:Parameter>
                            <asp:Parameter Name="Note" Type="String"></asp:Parameter>
                            <asp:Parameter Name="DateStamp" Type="DateTime"></asp:Parameter>
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="MainID" Type="String"></asp:Parameter>
                            <asp:Parameter Name="SO" Type="String"></asp:Parameter>
                            <asp:Parameter Name="PO" Type="String"></asp:Parameter>
                            <asp:Parameter Name="Qty" Type="Int32"></asp:Parameter>
                            <asp:Parameter Name="Note" Type="String"></asp:Parameter>
                            <asp:Parameter Name="DateStamp" Type="DateTime"></asp:Parameter>
                            <asp:Parameter Name="Key" Type="Int32"></asp:Parameter>
                        </UpdateParameters>
                    </asp:SqlDataSource>
                   
                
                </asp:Panel>
                <cc1:RoundedCornersExtender runat="server" BehaviorID="PanelAdd_RoundedCornersExtender" TargetControlID="PanelAdd" ID="PanelAdd_RoundedCornersExtender" BorderColor="Black" Color="SlateGray" Radius="10"></cc1:RoundedCornersExtender>
            
            </asp:Panel>
        </ContentTemplate>           
 </asp:UpdatePanel>
</asp:Content>
