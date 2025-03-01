<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="WipX_Report.aspx.vb" Inherits="Reports_WipX_Report" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
            <asp:Panel ID="Panel1" runat="server">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="WipX Report"></asp:Label><br />
                <br />

              
                Select.....
                <table class="style1">
                    <tr >
                        <td width="400">
                            <asp:RadioButton ID="RadioButtonWip3" runat="server" Text="Wip 3" Checked="True" GroupName="Wip" />
                            <asp:RadioButton ID="RadioButtonWip25" runat="server" Text="Wip 2.5" GroupName="Wip" />
                            <asp:RadioButton ID="RadioButtonWip2" runat="server" Text="Wip 2" GroupName="Wip" />
                            <asp:RadioButton ID="RadioButtonWip1" runat="server" Text="Wip 1" GroupName="Wip" />
                            <br />
                            <asp:RadioButton ID="RadioButtonDiameterAll" runat="server" Text="All Diameter" Checked="True" GroupName="Diameter" />
                            <asp:RadioButton ID="RadioButtonDiameter300" runat="server" Text="300mm" GroupName="Diameter" />
                            <asp:RadioButton ID="RadioButtonDiameter200" runat="server" Text="200mm" GroupName="Diameter" />
                            <asp:RadioButton ID="RadioButtonDiameterOther" runat="server" Text="Smaller" GroupName="Diameter" />
                            <br />
                            <asp:RadioButton ID="RadioButtonPassAll" runat="server" Text="All Pass" Checked="True" GroupName="Pass" />
                            <asp:RadioButton ID="RadioButtonPassFirst" runat="server" Text="First Pass" GroupName="Pass" />
                            <asp:RadioButton ID="RadioButtonPassNotFirst" runat="server" Text="Not First Pass" GroupName="Pass" />
                            <br />
                            <asp:RadioButton ID="RadioButtonTypeAll" runat="server" Text="All Type" Checked="True" GroupName="Type" />
                            <asp:RadioButton ID="RadioButtonTypeReclaim" runat="server" Text="Reclaim" GroupName="Type" />
                            <asp:RadioButton ID="RadioButtonTypeSupplied" runat="server" Text="Supplied" GroupName="Type" />
                        </td>                                          
                        <td>
                            <asp:Panel ID="Panel2" runat="server" Width="250" HorizontalAlign="Right">
                                Date Range<br />
                                Start Date:&nbsp; <asp:TextBox ID="TextBoxDateStart" runat="server"></asp:TextBox> <br />
                                &nbsp; End Date: <asp:TextBox ID="TextBoxDateEnd" runat="server"></asp:TextBox><br />
                                <asp:Button ID="ButtonRun" runat="server" Text="Run" />                                            
                            </asp:Panel>
                            
                        </td>                        
                    </tr>
                </table>
                <asp:Panel ID="Panel3" runat="server">
                    <asp:HyperLink ID="HyperLinkReport" runat="server" Visible="False">View in Excel</asp:HyperLink>
                </asp:Panel>
                <asp:Button ID="ButtonShowTheAdanced" runat="server" Text="Show Advanced" />
                <br />
                
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataSourceID="SqlDataSource1">
                    <Columns>

                        <asp:BoundField DataField="Diameter" HeaderText="Diameter" SortExpression="Diameter" />
                        <asp:BoundField DataField="LotType" HeaderText="LotType" SortExpression="LotType" />
                        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                        <asp:BoundField DataField="Start" HeaderText="Start" ReadOnly="True" SortExpression="Start" DataFormatString="{0:N0}" />
                        <asp:BoundField DataField="End" HeaderText="End" ReadOnly="True" SortExpression="End" DataFormatString="{0:N0}" />
                        <asp:BoundField DataField="Rejects" HeaderText="Rejects" ReadOnly="True" SortExpression="Rejects" DataFormatString="{0:N0}" />
                        <asp:BoundField DataField="Rework" HeaderText="Rework" ReadOnly="True" SortExpression="Rework" DataFormatString="{0:N0}" />
                        <asp:BoundField DataField="lot count" HeaderText="Lots" />
                        <asp:BoundField DataField="Stage" HeaderText="Stage" SortExpression="Stage" />
                        <asp:BoundField DataField="Exsil_Supplied" HeaderText="Supply" />
                        <asp:TemplateField HeaderText="Yield"></asp:TemplateField>
                        
                        <asp:ButtonField CommandName="Lots" Text="View" />
                        
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
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT TOP (100) PERCENT MainID.Diameter, T_Stage_Report.LotType, T_Stage_Report.ID, SUM(T_Stage_Report.[In]) AS Start, SUM(T_Stage_Report.Out) AS [End], SUM(T_Stage_Report.Rejects) AS Rejects, SUM(T_Stage_Report.Rework) AS Rework, COUNT(T_Stage_Report.LotNumber) AS [lot count], T_Stage_Report.Stage, MainID.Exsil_Supplied FROM T_Stage_Report INNER JOIN MainID ON T_Stage_Report.ID = MainID.MainID WHERE (T_Stage_Report.Date &gt; CONVERT (DATETIME, '2018-10-01 00:00:00', 102)) AND (T_Stage_Report.Date &lt; CONVERT (DATETIME, '2018-12-31 23:59:59', 102)) GROUP BY MainID.Diameter, T_Stage_Report.LotType, T_Stage_Report.ID, T_Stage_Report.Stage, MainID.Exsil_Supplied HAVING (T_Stage_Report.Stage = N'WIP 3') AND (T_Stage_Report.LotType = N'x') AND (MainID.Diameter = 200) ORDER BY MainID.Diameter, T_Stage_Report.ID"></asp:SqlDataSource>
            
            </asp:Panel>
            
            <!--************************************************************************************************************************* -->
            <!--************************************************************************************************************************* -->
            <!--************************************************************************************************************************* -->
            <asp:Button ID="ButtonShowLots" runat="server" Text="Show" style="display:none" />
            
                    <asp:Panel ID="PanelLotView" runat="server" HorizontalAlign="Right" BackColor="Black" BorderColor="Black" BorderStyle="Solid" BorderWidth="20">
                
                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataSourceID="SqlDataSource2">
                        <Columns>
                            <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                            <asp:BoundField DataField="LotNumber" HeaderText="LotNumber" SortExpression="LotNumber" />
                            <asp:BoundField DataField="Start" HeaderText="Start" SortExpression="Start" />
                            <asp:BoundField DataField="End" HeaderText="End" SortExpression="End" />
                            <asp:BoundField DataField="Rejects" HeaderText="Rejects" SortExpression="Rejects" />
                            <asp:BoundField DataField="Rework" HeaderText="Rework" SortExpression="Rework" />
                            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                            <asp:ButtonField CommandName="LotNumber" Text="View" />
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
                    <br />  
                    <asp:Button ID="ButtonClose" runat="server" Text="Close" />
                    <br />
                </asp:Panel>
                      
            <cc1:ModalPopupExtender 
                ID="PanelLotView_ModalPopupExtender"
                runat="server" 
                TargetControlID="ButtonShowLots" 
                PopupControlID="PanelLotView"  
                OkControlID ="ButtonClose"  
                       
                >
            </cc1:ModalPopupExtender>
            <!--************************************************************************************************************************* -->
            <!--************************************************************************************************************************* -->           
            <!--************************************************************************************************************************* -->

            <!--********************************************* The Advanced Search ******************************************************* -->
            <!--*************************************************** Panel *************************************************************** -->
            <!--************************************************************************************************************************* -->           
            <!--************************************************************************************************************************* -->
            <asp:Button ID="ButtonShowAdvanced" runat="server" Text="Show" style="display:none" />

                <asp:Panel ID="PanelAdvancedView" runat="server"  HorizontalAlign="Left"  BackColor="#33CCFF" BorderColor="#0066CC" BorderStyle="Solid" BorderWidth="20">
                     <h2>Advanced Wip View</h2><br />
                   
                    &nbsp;&nbsp;Select The Year:&nbsp;<asp:DropDownList ID="DropDownListYear" runat="server" DataSourceID="SqlDataSourceYear" DataTextField="Year" DataValueField="Year" AutoPostBack="True"></asp:DropDownList>
                   
                     <asp:Panel ID="PanelRangeType" runat="server" BorderColor="Black" BorderStyle="Solid" Width="500px">
                        
                        &nbsp;&nbsp;Select The Data Range Type:&nbsp
                        &nbsp;<asp:RadioButton ID="RadioButtonQQ" runat="server" Text="Quarter" GroupName="RangeType" AutoPostBack="True" />
                        &nbsp;<asp:RadioButton ID="RadioButtonMM" runat="server" Text="Month" GroupName="RangeType" AutoPostBack="True" />
                        &nbsp;<asp:RadioButton ID="RadioButtonWW" runat="server" Text="WorkWeek" GroupName="RangeType" AutoPostBack="True" />
                    </asp:Panel>
                    
                    <asp:Panel ID="PanelRangeSelect" runat="server">
                        <asp:CheckBoxList ID="CheckBoxListRangeSelected" runat="server" RepeatColumns="12" RepeatDirection="Horizontal">

                        </asp:CheckBoxList>
                    </asp:Panel>

                    <asp:Panel ID="PanelFabs" runat="server" BorderColor="Black" BorderStyle="Solid" Width="500px">
                        
                        &nbsp;&nbsp;Select The Fab:&nbsp
                        &nbsp;<asp:DropDownList ID="DropDownListFabs" runat="server" AutoPostBack="True"></asp:DropDownList>
                    </asp:Panel>
                    
                    <asp:Panel ID="PanelIds" runat="server">
                        <asp:CheckBoxList ID="CheckBoxListFabIDs" runat="server" RepeatColumns="12" RepeatDirection="Horizontal">

                        </asp:CheckBoxList>
                    </asp:Panel>
                   
                    <asp:Panel ID="PanelWip" runat="server" BorderColor="Black" BorderStyle="Solid" Width="500px">
                        
                        &nbsp;&nbsp;Select The Wip Area:&nbsp
                        &nbsp;
                        <asp:RadioButton ID="RadioButtonWippy3" runat="server" Text="Wip 3" Checked="True" GroupName="Wippy" />
                            <asp:RadioButton ID="RadioButtonWippy25" runat="server" Text="Wip 2.5" GroupName="Wippy" />
                            <asp:RadioButton ID="RadioButtonWippy2" runat="server" Text="Wip 2" GroupName="Wippy" />
                            <asp:RadioButton ID="RadioButtonWippy1" runat="server" Text="Wip 1" GroupName="Wippy" />                            
                    </asp:Panel>

                    <asp:Panel ID="PanelPass" runat="server" BorderColor="Black" BorderStyle="Solid" Width="500px">
                        
                        &nbsp;&nbsp;Select The Run Type:&nbsp
                        &nbsp;
                            <asp:RadioButton ID="RadioButtonPassyAll" runat="server" Text="All Pass" Checked="True" GroupName="Passy" />
                            <asp:RadioButton ID="RadioButtonPassyFirst" runat="server" Text="First Pass" GroupName="Passy" />
                            <asp:RadioButton ID="RadioButtonPassyNotFirst" runat="server" Text="Not First Pass" GroupName="Passy" />
                    </asp:Panel>
                    <asp:Button ID="ButtonAdvanced" runat="server" Text="Run" />
                    <asp:SqlDataSource ID="SqlDataSourceYear" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT DATEPART(yyyy, Date) AS Year FROM T_Stage_Report GROUP BY DATEPART(yyyy, Date) ORDER BY Year"></asp:SqlDataSource>
                   

                </asp:Panel> 
             <cc1:ModalPopupExtender 
                ID="PanelAdvancedView_ModalPopupExtender"
                runat="server" 
                TargetControlID="ButtonShowAdvanced" 
                PopupControlID="PanelAdvancedView"  
                OkControlID ="ButtonClose"  
                       
                >
            </cc1:ModalPopupExtender>


            <!--************************************************************************************************************************* -->
            <!--************************************************************************************************************************* -->           
            <!--************************************************************************************************************************* -->


            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT TOP (100) PERCENT LotType AS Type, LotNumber, [In] AS Start, Out AS [End], Rejects, Rework, Date FROM T_Stage_Report WHERE (Date &lt; CONVERT (DATETIME, '2018-12-31 23:59:59', 102)) AND (Date &gt; CONVERT (DATETIME, '2018-12-01 00:00:00', 102)) AND (Stage = N'WIP 3') AND (ID = N'0') AND (LotType = N'f') ORDER BY Date"></asp:SqlDataSource>







        <asp:UpdateProgress id="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <IMG src="../Color/Animated_LoadingBigger.gif" />Working...
            </ProgressTemplate>
        </asp:UpdateProgress>
        </contenttemplate>
    </asp:UpdatePanel>    
</asp:Content>


