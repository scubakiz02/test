<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SPC_Managment.aspx.vb" Inherits="SPC_SPC_Managment" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <asp:Label ID="Label3" runat="server" Text="SPC Managment" Font-Bold="True" Font-Size="X-Large"></asp:Label><br />
                <br />
                <asp:Panel ID="PanelToolSetups" runat="server" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px" >                    
                    
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="3" DataKeyNames="Key" DataSourceID="SqlDataSourceToolSetups" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black" GridLines="Vertical">
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                            <asp:BoundField DataField="Key" HeaderText="k" InsertVisible="False" SortExpression="Key" >
                            <ItemStyle BackColor="#666666" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" />
                            <asp:BoundField DataField="Tool_Name" HeaderText="Tool_Name" SortExpression="Tool_Name" />
                            <asp:BoundField DataField="SQL_Function" HeaderText="SQL_Function" SortExpression="SQL_Function" />
                            <asp:CheckBoxField DataField="Enable" HeaderText="Enable" SortExpression="Enable" />
                            <asp:BoundField DataField="Info" HeaderText="Info" SortExpression="Info" />
                            <asp:BoundField DataField="Picture_Path" HeaderText="Picture_Path" SortExpression="Picture_Path" />
                            <asp:CommandField ShowEditButton="True" />
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" />
                        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#33CCCC" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#808080" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#383838" />
                    </asp:GridView><br />
                    
                    <asp:Button ID="ButtonNewTool" runat="server" Text="Add Tool" /><br />
                    <br />
                    <asp:Panel ID="PanelNewTool" runat="server" Visible="False" Width="300" BackColor="#33CCCC" BorderColor="#33CCCC" BorderWidth="10px" BorderStyle="Solid" ForeColor="Black">
                        Select a Department:<br />
                        <asp:DropDownList ID="DropDownListDepartments" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource_Departments" DataTextField="Department" DataValueField="Department"></asp:DropDownList><br />
                        <br />
                        Select Tool:<br />
                        <asp:DropDownList ID="DropDownListTools" runat="server" AutoPostBack="True" DataSourceID="SqlDataSourceTools" DataTextField="Tool" DataValueField="Tool"></asp:DropDownList><br />
                        <br />
                        <asp:Button ID="ButtonAddTool" runat="server" Text="Add Tool" />
                        &nbsp;&nbsp; 
                        <asp:Button ID="ButtonCloseAddToolPanel" runat="server" Text="Close" />

                    </asp:Panel>
                   
                </asp:Panel><br />
                
                <asp:Panel ID="PanelParameters" runat="server" Visible="False" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px">
                    <asp:Label ID="Label1" runat="server" Font-Size="Large" Text="Parameters for "></asp:Label>
                    <asp:Label ID="LabelParameterTool" runat="server" Font-Size="X-Large" BackColor="#33CCCC"></asp:Label><br />
                    <asp:Label ID="LabelToolNumber" runat="server" Text="0"></asp:Label><br />
                    <asp:GridView ID="GridViewParameters" runat="server" CellPadding="3" AutoGenerateColumns="False" DataKeyNames="Key" DataSourceID="SqlDataSourceParameters" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black" GridLines="Vertical" >
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                            <asp:BoundField DataField="Key" HeaderText="k" SortExpression="Key" ShowHeader="False" >
                            <ItemStyle BackColor="#666666" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Tool_Key" HeaderText="k" SortExpression="Tool_Key" ShowHeader="False" InsertVisible="False" >
                            <ItemStyle BackColor="#666666" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Seq_Flow" HeaderText="Seq_Flow" SortExpression="Seq_Flow" />
                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                            <asp:BoundField DataField="DB_Column" HeaderText="DB_Column" SortExpression="DB_Column" />
                            <asp:CheckBoxField DataField="Enable" HeaderText="Enable" SortExpression="Enable" />
                            <asp:BoundField DataField="OCAP_Low" HeaderText="OCAP_Low" SortExpression="OCAP_Low" />
                            <asp:BoundField DataField="OCAP_High" HeaderText="OCAP_High" SortExpression="OCAP_High" />
                            <asp:CommandField ShowEditButton="True" />
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" />
                        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#33CCCC" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#808080" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#383838" />
                    </asp:GridView>                    
                    <br />
                    <asp:Button ID="ButtonAddParameter" runat="server" Text="Add Parameter" />                    
                </asp:Panel>
                <br />
                <asp:Panel ID="PanelLimits" runat="server" Visible="False" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px">
                    <asp:Label ID="Label2" runat="server" Font-Size="Large" Text="Limits for "></asp:Label> 
                    <asp:Label ID="LabelLimitName" runat="server" Text="" Font-Size="X-Large" BackColor="#33CCCC"></asp:Label><br />
                    <asp:Label ID="LabelParameterNumber" runat="server" Text="0"></asp:Label><br />
                    <asp:GridView ID="GridViewLimits" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" DataKeyNames="Key" DataSourceID="SqlDataSourceLimits">
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True" SortExpression="Key" />
                            <asp:BoundField DataField="Parameter_Key" HeaderText="Parameter_Key" SortExpression="Parameter_Key" />
                            <asp:BoundField DataField="Avg_LCL" HeaderText="Avg_LCL" SortExpression="Avg_LCL" />
                            <asp:BoundField DataField="Avg_UCL" HeaderText="Avg_UCL" SortExpression="Avg_UCL" />
                            <asp:BoundField DataField="Stdev_LCL" HeaderText="Stdev_LCL" SortExpression="Stdev_LCL" />
                            <asp:BoundField DataField="Stdev_UCL" HeaderText="Stdev_UCL" SortExpression="Stdev_UCL" />
                            <asp:CheckBoxField DataField="Enable" HeaderText="Enable" SortExpression="Enable" />
                            <asp:CommandField ShowEditButton="True" />
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" />
                        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#33CCCC" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#808080" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#383838" />
                    </asp:GridView>
                  
                    <br />
                    <asp:Button ID="ButtonAddLimit" runat="server" Text="Add Limit" /> 
                </asp:Panel>



















            </asp:Panel> 


            <asp:SqlDataSource ID="SqlDataSourceLimits" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:SATI_SPCConnectionString %>" DeleteCommand="DELETE FROM [T_SPC_Limits] WHERE [Key] = @original_Key AND [Parameter_Key] = @original_Parameter_Key AND (([Avg_LCL] = @original_Avg_LCL) OR ([Avg_LCL] IS NULL AND @original_Avg_LCL IS NULL)) AND (([Avg_UCL] = @original_Avg_UCL) OR ([Avg_UCL] IS NULL AND @original_Avg_UCL IS NULL)) AND (([Stdev_LCL] = @original_Stdev_LCL) OR ([Stdev_LCL] IS NULL AND @original_Stdev_LCL IS NULL)) AND (([Stdev_UCL] = @original_Stdev_UCL) OR ([Stdev_UCL] IS NULL AND @original_Stdev_UCL IS NULL)) AND (([Enable] = @original_Enable) OR ([Enable] IS NULL AND @original_Enable IS NULL))" InsertCommand="INSERT INTO [T_SPC_Limits] ([Parameter_Key], [Avg_LCL], [Avg_UCL], [Stdev_LCL], [Stdev_UCL], [Enable]) VALUES (@Parameter_Key, @Avg_LCL, @Avg_UCL, @Stdev_LCL, @Stdev_UCL, @Enable)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [Key], [Parameter_Key], [Avg_LCL], [Avg_UCL], [Stdev_LCL], [Stdev_UCL], [Enable] FROM [T_SPC_Limits] WHERE ([Parameter_Key] = @Parameter_Key)" UpdateCommand="UPDATE [T_SPC_Limits] SET [Parameter_Key] = @Parameter_Key, [Avg_LCL] = @Avg_LCL, [Avg_UCL] = @Avg_UCL, [Stdev_LCL] = @Stdev_LCL, [Stdev_UCL] = @Stdev_UCL, [Enable] = @Enable WHERE [Key] = @original_Key AND [Parameter_Key] = @original_Parameter_Key AND (([Avg_LCL] = @original_Avg_LCL) OR ([Avg_LCL] IS NULL AND @original_Avg_LCL IS NULL)) AND (([Avg_UCL] = @original_Avg_UCL) OR ([Avg_UCL] IS NULL AND @original_Avg_UCL IS NULL)) AND (([Stdev_LCL] = @original_Stdev_LCL) OR ([Stdev_LCL] IS NULL AND @original_Stdev_LCL IS NULL)) AND (([Stdev_UCL] = @original_Stdev_UCL) OR ([Stdev_UCL] IS NULL AND @original_Stdev_UCL IS NULL)) AND (([Enable] = @original_Enable) OR ([Enable] IS NULL AND @original_Enable IS NULL))">
                        <DeleteParameters>
                            <asp:Parameter Name="original_Key" Type="Int32" />
                            <asp:Parameter Name="original_Parameter_Key" Type="Int32" />
                            <asp:Parameter Name="original_Avg_LCL" Type="Single" />
                            <asp:Parameter Name="original_Avg_UCL" Type="Single" />
                            <asp:Parameter Name="original_Stdev_LCL" Type="Single" />
                            <asp:Parameter Name="original_Stdev_UCL" Type="Single" />
                            <asp:Parameter Name="original_Enable" Type="Boolean" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="Parameter_Key" Type="Int32" />
                            <asp:Parameter Name="Avg_LCL" Type="Single" />
                            <asp:Parameter Name="Avg_UCL" Type="Single" />
                            <asp:Parameter Name="Stdev_LCL" Type="Single" />
                            <asp:Parameter Name="Stdev_UCL" Type="Single" />
                            <asp:Parameter Name="Enable" Type="Boolean" />
                        </InsertParameters>
                        <SelectParameters>
                            <asp:ControlParameter ControlID="LabelParameterNumber" Name="Parameter_Key" PropertyName="Text" Type="Int32" />
                        </SelectParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="Parameter_Key" Type="Int32" />
                            <asp:Parameter Name="Avg_LCL" Type="Single" />
                            <asp:Parameter Name="Avg_UCL" Type="Single" />
                            <asp:Parameter Name="Stdev_LCL" Type="Single" />
                            <asp:Parameter Name="Stdev_UCL" Type="Single" />
                            <asp:Parameter Name="Enable" Type="Boolean" />
                            <asp:Parameter Name="original_Key" Type="Int32" />
                            <asp:Parameter Name="original_Parameter_Key" Type="Int32" />
                            <asp:Parameter Name="original_Avg_LCL" Type="Single" />
                            <asp:Parameter Name="original_Avg_UCL" Type="Single" />
                            <asp:Parameter Name="original_Stdev_LCL" Type="Single" />
                            <asp:Parameter Name="original_Stdev_UCL" Type="Single" />
                            <asp:Parameter Name="original_Enable" Type="Boolean" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
            
            <asp:SqlDataSource 
                ID="SqlDataSourceParameters" 
                runat="server" 
                ConflictDetection="CompareAllValues" 
                ConnectionString="<%$ ConnectionStrings:SATI_SPCConnectionString %>" 
                DeleteCommand="DELETE FROM [T_SPC_Parameters] WHERE [Key] = @original_Key AND [Tool_Key] = @original_Tool_Key AND [Seq_Flow] = @original_Seq_Flow AND (([Name] = @original_Name) OR ([Name] IS NULL AND @original_Name IS NULL)) AND (([DB_Column] = @original_DB_Column) OR ([DB_Column] IS NULL AND @original_DB_Column IS NULL)) AND [Enable] = @original_Enable AND (([OCAP_Low] = @original_OCAP_Low) OR ([OCAP_Low] IS NULL AND @original_OCAP_Low IS NULL)) AND (([OCAP_High] = @original_OCAP_High) OR ([OCAP_High] IS NULL AND @original_OCAP_High IS NULL))" 
                InsertCommand="INSERT INTO [T_SPC_Parameters] ([Tool_Key], [Seq_Flow], [Name], [DB_Column], [Enable], [OCAP_Low], [OCAP_High]) VALUES (@Tool_Key, @Seq_Flow, @Name, @DB_Column, @Enable, @OCAP_Low, @OCAP_High)" 
                OldValuesParameterFormatString="original_{0}" 
                SelectCommand="SELECT [Key], [Tool_Key], [Seq_Flow], [Name], [DB_Column], [Enable], [OCAP_Low], [OCAP_High] FROM [T_SPC_Parameters] WHERE ([Tool_Key] = @Tool_Key) ORDER BY [Seq_Flow]" 
                UpdateCommand="UPDATE [T_SPC_Parameters] SET [Tool_Key] = @Tool_Key, [Seq_Flow] = @Seq_Flow, [Name] = @Name, [DB_Column] = @DB_Column, [Enable] = @Enable, [OCAP_Low] = @OCAP_Low, [OCAP_High] = @OCAP_High WHERE [Key] = @original_Key AND [Tool_Key] = @original_Tool_Key AND [Seq_Flow] = @original_Seq_Flow AND (([Name] = @original_Name) OR ([Name] IS NULL AND @original_Name IS NULL)) AND (([DB_Column] = @original_DB_Column) OR ([DB_Column] IS NULL AND @original_DB_Column IS NULL)) AND [Enable] = @original_Enable AND (([OCAP_Low] = @original_OCAP_Low) OR ([OCAP_Low] IS NULL AND @original_OCAP_Low IS NULL)) AND (([OCAP_High] = @original_OCAP_High) OR ([OCAP_High] IS NULL AND @original_OCAP_High IS NULL))">
                        <DeleteParameters>
                            <asp:Parameter Name="original_Key" Type="Int32" />
                            <asp:Parameter Name="original_Tool_Key" Type="Int32" />
                            <asp:Parameter Name="original_Seq_Flow" Type="Int32" />
                            <asp:Parameter Name="original_Name" Type="String" />
                            <asp:Parameter Name="original_DB_Column" Type="String" />
                            <asp:Parameter Name="original_Enable" Type="Boolean" />
                            <asp:Parameter Name="original_OCAP_Low" Type="String" />
                            <asp:Parameter Name="original_OCAP_High" Type="String" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="Tool_Key" Type="Int32" />
                            <asp:Parameter Name="Seq_Flow" Type="Int32" />
                            <asp:Parameter Name="Name" Type="String" />
                            <asp:Parameter Name="DB_Column" Type="String" />
                            <asp:Parameter Name="Enable" Type="Boolean" />
                            <asp:Parameter Name="OCAP_Low" Type="String" />
                            <asp:Parameter Name="OCAP_High" Type="String" />
                        </InsertParameters>
                        <SelectParameters>
                            <asp:ControlParameter ControlID="LabelToolNumber" Name="Tool_Key" PropertyName="Text" Type="Int32" />
                        </SelectParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="Tool_Key" Type="Int32" />
                            <asp:Parameter Name="Seq_Flow" Type="Int32" />
                            <asp:Parameter Name="Name" Type="String" />
                            <asp:Parameter Name="DB_Column" Type="String" />
                            <asp:Parameter Name="Enable" Type="Boolean" />
                            <asp:Parameter Name="OCAP_Low" Type="String" />
                            <asp:Parameter Name="OCAP_High" Type="String" />
                            <asp:Parameter Name="original_Key" Type="Int32" />
                            <asp:Parameter Name="original_Tool_Key" Type="Int32" />
                            <asp:Parameter Name="original_Seq_Flow" Type="Int32" />
                            <asp:Parameter Name="original_Name" Type="String" />
                            <asp:Parameter Name="original_DB_Column" Type="String" />
                            <asp:Parameter Name="original_Enable" Type="Boolean" />
                            <asp:Parameter Name="original_OCAP_Low" Type="String" />
                            <asp:Parameter Name="original_OCAP_High" Type="String" />
                        </UpdateParameters>
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlDataSourceTools" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT Tool FROM T_Tools WHERE (Department = '1') ORDER BY Tool"></asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlDataSource_Departments" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT Department FROM T_Tools GROUP BY Department ORDER BY Department"></asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlDataSourceToolSetups" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:SATI_SPCConnectionString %>" DeleteCommand="DELETE FROM [T_SPC_Tool_Info] WHERE [Key] = @original_Key AND [Department] = @original_Department AND (([SQL_Function] = @original_SQL_Function) OR ([SQL_Function] IS NULL AND @original_SQL_Function IS NULL)) AND [Enable] = @original_Enable AND (([Info] = @original_Info) OR ([Info] IS NULL AND @original_Info IS NULL)) AND (([Picture_Path] = @original_Picture_Path) OR ([Picture_Path] IS NULL AND @original_Picture_Path IS NULL)) AND [Tool_Name] = @original_Tool_Name" InsertCommand="INSERT INTO [T_SPC_Tool_Info] ([Department], [SQL_Function], [Enable], [Info], [Picture_Path], [Tool_Name]) VALUES (@Department, @SQL_Function, @Enable, @Info, @Picture_Path, @Tool_Name)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [Key], [Department], [SQL_Function], [Enable], [Info], [Picture_Path], [Tool_Name] FROM [T_SPC_Tool_Info] ORDER BY [Department], [Tool_Name]" UpdateCommand="UPDATE [T_SPC_Tool_Info] SET [Department] = @Department, [SQL_Function] = @SQL_Function, [Enable] = @Enable, [Info] = @Info, [Picture_Path] = @Picture_Path, [Tool_Name] = @Tool_Name WHERE [Key] = @original_Key AND [Department] = @original_Department AND (([SQL_Function] = @original_SQL_Function) OR ([SQL_Function] IS NULL AND @original_SQL_Function IS NULL)) AND [Enable] = @original_Enable AND (([Info] = @original_Info) OR ([Info] IS NULL AND @original_Info IS NULL)) AND (([Picture_Path] = @original_Picture_Path) OR ([Picture_Path] IS NULL AND @original_Picture_Path IS NULL)) AND [Tool_Name] = @original_Tool_Name">
                        <DeleteParameters>
                            <asp:Parameter Name="original_Key" Type="Int32" />
                            <asp:Parameter Name="original_Department" Type="String" />
                            <asp:Parameter Name="original_SQL_Function" Type="String" />
                            <asp:Parameter Name="original_Enable" Type="Boolean" />
                            <asp:Parameter Name="original_Info" Type="String" />
                            <asp:Parameter Name="original_Picture_Path" Type="String" />
                            <asp:Parameter Name="original_Tool_Name" Type="String" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="Department" Type="String" />
                            <asp:Parameter Name="SQL_Function" Type="String" />
                            <asp:Parameter Name="Enable" Type="Boolean" />
                            <asp:Parameter Name="Info" Type="String" />
                            <asp:Parameter Name="Picture_Path" Type="String" />
                            <asp:Parameter Name="Tool_Name" Type="String" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="Department" Type="String" />
                            <asp:Parameter Name="SQL_Function" Type="String" />
                            <asp:Parameter Name="Enable" Type="Boolean" />
                            <asp:Parameter Name="Info" Type="String" />
                            <asp:Parameter Name="Picture_Path" Type="String" />
                            <asp:Parameter Name="Tool_Name" Type="String" />
                            <asp:Parameter Name="original_Key" Type="Int32" />
                            <asp:Parameter Name="original_Department" Type="String" />
                            <asp:Parameter Name="original_SQL_Function" Type="String" />
                            <asp:Parameter Name="original_Enable" Type="Boolean" />
                            <asp:Parameter Name="original_Info" Type="String" />
                            <asp:Parameter Name="original_Picture_Path" Type="String" />
                            <asp:Parameter Name="original_Tool_Name" Type="String" />
                        </UpdateParameters>
                    </asp:SqlDataSource>

                      
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

