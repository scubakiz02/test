

<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="NonConformingManagment.aspx.vb" Inherits="PC_NonConformingManagment" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">    

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
               
                <table class="style1" style="text-align: left; vertical-align: top">
                    <tr style="vertical-align: top; text-align: left">
                        <td style="width: 359px">
                            Select A Customer: &nbsp; 
                           
                            <asp:DropDownList ID="DropDownListCustomer" runat="server" 
                                DataSourceID="SqlDataSourceCustomers" DataTextField="Customer_Name" 
                                DataValueField="Customer_Name" Width="207px" 
                                AutoPostBack="True">
                            </asp:DropDownList><br />
                            Select Diameter:&nbsp;
                            <asp:DropDownList ID="DropDownListDiameter" runat="server" 
                                DataSourceID="SqlDataSourceDiameter" DataTextField="Diameter" 
                                DataValueField="Diameter">
                            </asp:DropDownList>&nbsp;
                            <asp:Button ID="ButtonGetData" runat="server" Text="Go" />
                            <br />
                           <asp:ListBox ID="ListBoxId" runat="server" DataSourceID="SqlDataSourceIDlist" 
                                DataTextField="MainID" DataValueField="MainID" AutoPostBack="True" 
                                Height="370px" Width="123px"></asp:ListBox>
                    
                           
                    
                        </td>                                       
                        <td><br />
                            Selected ID: &nbsp;<asp:Label ID="LabelSelectedID" runat="server" Text="0"></asp:Label>&nbsp;&nbsp;&nbsp;
                            Diameter:  &nbsp;<asp:Label ID="LabelDiameter" runat="server" Text="0"></asp:Label><br /><br />
                            This Is A Genral Solar Wafer: 
                            <asp:DropDownList ID="DropDownListSolarType" runat="server" Width="114px" 
                                AutoPostBack="True">
                                <asp:ListItem Value="C">No</asp:ListItem>
                                <asp:ListItem Value="P">P Type</asp:ListItem>
                                <asp:ListItem Value="N">N Type</asp:ListItem>
                            </asp:DropDownList> 
                            &nbsp;<br /><br />
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                                CellPadding="4" DataKeyNames="Key" DataSourceID="SqlDataSourceIDinfo" 
                                ForeColor="#333333" GridLines="None">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:CommandField ShowEditButton="True" />
                                    <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" 
                                        ReadOnly="True" SortExpression="Key" Visible="False" />
                                    <asp:BoundField DataField="PackingNote" HeaderText="PackingNote" 
                                        SortExpression="PackingNote" />
                                    <asp:CheckBoxField DataField="Sell" HeaderText="Sell" SortExpression="Sell" />
                                    <asp:BoundField DataField="PWI_Percent" HeaderText="PWI_Percent" 
                                        SortExpression="PWI_Percent" />
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" /> 
                            </asp:GridView>
                            <br />
                            <asp:Label ID="LabelBelong" runat="server" Text="Label" BackColor="Yellow" Font-Size="XX-Large" Visible="False"></asp:Label>
                            <table class="style1">
                                <tr>
                                    <td>
                                        Sub ID's<br />
                                        <asp:ListBox ID="ListBoxSubId" runat="server" Height="230px" Width="127px" 
                                            DataSourceID="SqlDataSourceSubId" DataTextField="ID" DataValueField="ID"></asp:ListBox>
                                       
                                    </td>
                                    <td>
                                        Avalible Sub ID's<br />
                                        <asp:ListBox ID="ListBoxAvalibleSubId" runat="server" Height="230px" 
                                            Width="127px"></asp:ListBox>
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        Remove Sub ID<br />
                                        <asp:Button ID="ButtonSubIdRemove" runat="server" Text="Remove Selected" />
                                    </td>
                                    <td>
                                        Add Sub ID<br />
                                        <asp:Button ID="ButtonSubIdAdd" runat="server" Text="Add Selected" />
                                    </td>
                                </tr>
                            </table>
                            
                        </td>
                    </tr>
                </table>
            
            
                
                
                
                
                &nbsp;<br />
                
                
                
                
            </asp:Panel>
            
            
            
            <asp:SqlDataSource ID="SqlDataSourceSubId" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                                            SelectCommand="SELECT [ID] FROM [T_NC_ID_Info] WHERE ([PackWithID] = @PackWithID)">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="LabelSelectedID" Name="PackWithID" 
                                                    PropertyName="Text" Type="String" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
            
            <asp:SqlDataSource ID="SqlDataSourceIDlist" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                                SelectCommand="SELECT dbo.MainID.MainID, dbo.MainID.Diameter FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID WHERE (dbo.Customer.Customer_Name = N'Exsil') AND (dbo.MainID.Diameter = 10)">
                            </asp:SqlDataSource>
              
            <asp:SqlDataSource ID="SqlDataSourceCustomers" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                    SelectCommand="SELECT Customer_Name FROM dbo.Customer GROUP BY Customer_Name ORDER BY Customer_Name">
                </asp:SqlDataSource>
                
            <asp:SqlDataSource ID="SqlDataSourceDiameter" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                    SelectCommand="SELECT dbo.Customer.Customer_Name, dbo.MainID.Diameter FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID GROUP BY dbo.Customer.Customer_Name, dbo.MainID.Diameter HAVING (dbo.Customer.Customer_Name = N'Exsil') ORDER BY dbo.MainID.Diameter">
                </asp:SqlDataSource>
               
            <asp:SqlDataSource ID="SqlDataSourceIDinfo" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                                
                                SelectCommand="SELECT [Key], [PackingNote], [Sell], [PWI_Percent] FROM [T_NC_ID_Info] WHERE ([ID] = @ID)" 
                                DeleteCommand="DELETE FROM [T_NC_ID_Info] WHERE [Key] = @Key" 
                                InsertCommand="INSERT INTO [T_NC_ID_Info] ([PackingNote], [Sell], [PWI_Percent]) VALUES (@PackingNote, @Sell, @PWI_Percent)" 
                                
                    UpdateCommand="UPDATE [T_NC_ID_Info] SET [PackingNote] = @PackingNote, [Sell] = @Sell, [PWI_Percent] = @PWI_Percent WHERE [Key] = @Key">
                                <SelectParameters>                                
                                    <asp:ControlParameter ControlID="LabelSelectedID" Name="ID" PropertyName="Text" 
                                        Type="String" />
                                </SelectParameters>
                                <DeleteParameters>
                                    <asp:Parameter Name="Key" Type="Int32" />
                                </DeleteParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="PackingNote" Type="String" />
                                    <asp:Parameter Name="Sell" Type="Boolean" />
                                    <asp:Parameter Name="PWI_Percent" Type="Int32" />
                                    <asp:Parameter Name="Key" Type="Int32" />
                                </UpdateParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="PackingNote" Type="String" />
                                    <asp:Parameter Name="Sell" Type="Boolean" />
                                    <asp:Parameter Name="PWI_Percent" Type="Int32" />
                                </InsertParameters>
                            </asp:SqlDataSource>
            
        </ContentTemplate>
    </asp:UpdatePanel>        
</asp:Content>


