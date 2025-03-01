<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SurfScan.aspx.vb" Inherits="Reports_SurfScan" title="Bin Fall" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate> 
            <asp:Button ID="SPxPopupFakeButton" runat="server" Style="display: none" Text="spxtrig" />
            <asp:Button ID="TencorPopupFakeButton" runat="server" Style="display: none" Text="tencortrig" />&nbsp;
            <asp:Button ID="mapPopupFakeButton" runat="server" Style="display: none" Text="maptrig" />
            <asp:HiddenField ID="HiddenField_SPx" runat="server" />
            <asp:HiddenField ID="HiddenField_Row" runat="server" />
            <asp:HiddenField ID="HiddenField_Slot" runat="server" />
            <asp:Panel ID="Panel1" runat="server" >
                Tool:&nbsp;
                <asp:RadioButton ID="SPxRadioButton" runat="server" AutoPostBack="True" Checked="True" GroupName="SurfScan" OnCheckedChanged="SPxRadioButton_CheckedChanged" Text="SPx" />&nbsp;
                
                <asp:CheckBox ID="SP1CheckBox" runat="server" AutoPostBack="True" OnCheckedChanged="SP1CheckBox_CheckedChanged" Text="SP1," />
                <asp:CheckBox ID="SP12CheckBox" runat="server" AutoPostBack="True" OnCheckedChanged="SP12CheckBox_CheckedChanged" Text="SP1-2," />
                <asp:CheckBox ID="SP13CheckBox" runat="server" AutoPostBack="True" OnCheckedChanged="SP13CheckBox_CheckedChanged" Text="SP1-3," />
                <asp:CheckBox ID="SP2CheckBox" runat="server" AutoPostBack="True" OnCheckedChanged="SP2CheckBox_CheckedChanged" Text="SP2" />
                <asp:CheckBox ID="SP3CheckBox" runat="server" AutoPostBack="True" OnCheckedChanged="SP3CheckBox_CheckedChanged" Text="SP3" />
                <asp:CheckBox ID="TencorCheckBox" runat="server" AutoPostBack="True" OnCheckedChanged="TencorCheckBox_CheckedChanged" Text="Tencor," Visible="False" />&nbsp;
                <asp:CheckBox ID="Tencor3CheckBox" runat="server" AutoPostBack="True" OnCheckedChanged="Tencor3CheckBox_CheckedChanged" Text="Tencor 3," Visible="False" />&nbsp;
                <asp:CheckBox ID="Tencor4CheckBox" runat="server" AutoPostBack="True" OnCheckedChanged="Tencor4CheckBox_CheckedChanged" Text="Tencor 4"  Visible="False" />
                &nbsp; &nbsp; &nbsp; &nbsp;<asp:CheckBox ID="CheckBoxDaily" runat="server" AutoPostBack="True" Text="Find all 'Daily' tests on all tools" />
                &nbsp;&nbsp; &nbsp;
                <br />
                Session Qty: (
                <asp:RadioButton ID="RadioButton10" runat="server" AutoPostBack="True" Checked="True" GroupName="Qty" OnCheckedChanged="RadioButton10_CheckedChanged" Text="10" />,
                <asp:RadioButton ID="RadioButton25" runat="server" AutoPostBack="True" GroupName="Qty" OnCheckedChanged="RadioButton25_CheckedChanged" Text="25" />,
                <asp:RadioButton ID="RadioButton50" runat="server" AutoPostBack="True" GroupName="Qty" OnCheckedChanged="RadioButton50_CheckedChanged" Text="50" />,
                <asp:RadioButton ID="RadioButton75" runat="server" AutoPostBack="True" GroupName="Qty" OnCheckedChanged="RadioButton75_CheckedChanged" Text="75" />,
                <asp:RadioButton ID="RadioButtonSelectDate" runat="server" AutoPostBack="True" GroupName="Qty" Text="Select date"/> &nbsp;
                <asp:Label ID="LabelStart" runat="server" Text="Start:" Visible="False" Font-Underline="True"></asp:Label>
                <asp:TextBox ID="TextBoxStartDate" runat="server" TextMode="Date" AutoPostBack="True" Visible="False"></asp:TextBox>
                <asp:Label ID="LabelEnd" runat="server" Text="End:" Visible="False" Font-Underline="True"></asp:Label>
                <asp:TextBox ID="TextBoxEndDate" runat="server" TextMode="Date" AutoPostBack="True" Visible="False"></asp:TextBox>
                )

                &nbsp;
                Diameter:
                <asp:DropDownList ID="DropDownListDiameter" runat="server" AutoPostBack="True">
                    <asp:ListItem>All</asp:ListItem>
                    <asp:ListItem>200mm</asp:ListItem>
                    <asp:ListItem>300mm</asp:ListItem>
                </asp:DropDownList>
                 &nbsp;
                <asp:CheckBox ID="AdvancedCheckBox" runat="server" AutoPostBack="True" OnCheckedChanged="AdvancedCheckBox_CheckedChanged" Text="Advanced Filter" />
                
               
                 &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;<br />
                
                <asp:Panel ID="AdvancedPanel" runat="server" Style="text-align: left" Visible="False"  BackColor="PowderBlue" Width="1000px">
                    <asp:Panel ID="Panel3" runat="server" Style="text-align: right" Width="896px">
                        
                        &nbsp;&nbsp;
                        <asp:Button ID="SP1UpdateButton" runat="server" Text="Update SP1" OnClick="SP1UpdateButton_Click" />
                        <asp:Button ID="SP12UpdateButton" runat="server" Text="Update SP12" OnClick="SP12UpdateButton_Click" />
                        <asp:Button ID="SP13UpdateButton" runat="server" Text="Update SP13" />
                    </asp:Panel>
                    <asp:Panel ID="Panel4" runat="server">
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;ID:
                        <asp:TextBox ID="IDTextBox" runat="server" AutoPostBack="True" OnTextChanged="IDTextBox_TextChanged" Width="144px"></asp:TextBox>
                        &nbsp;
                        <asp:RadioButton ID="IDFindRadioButton" runat="server" Text="Find Only" AutoPostBack="True" Checked="True" GroupName="id" OnCheckedChanged="LotFindRadioButton_CheckedChanged" />
                        or&nbsp;
                        <asp:RadioButton ID="IDNotRadioButton" runat="server" Text="Exclude" AutoPostBack="True" GroupName="id" OnCheckedChanged="IDNotRadioButton_CheckedChanged" />
                        &nbsp; &nbsp;
                        <asp:CheckBox ID="IDActivateCheckBox" runat="server" Text="Activate" AutoPostBack="True" OnCheckedChanged="IDActivateCheckBox_CheckedChanged" /><br />
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        Run:
                        <asp:TextBox ID="RunTextBox" runat="server" AutoPostBack="True" OnTextChanged="RunTextBox_TextChanged" Width="144px"></asp:TextBox>
                        &nbsp;
                        <asp:RadioButton ID="RunFindRadioButton" runat="server" Text="Find Only" AutoPostBack="True" Checked="True" GroupName="run" OnCheckedChanged="RunFindRadioButton_CheckedChanged" />
                        or&nbsp;
                        <asp:RadioButton ID="RunNotRadioButton" runat="server" Text="Exclude" AutoPostBack="True" GroupName="run" OnCheckedChanged="RunNotRadioButton_CheckedChanged" />
                        &nbsp; &nbsp;
                        <asp:CheckBox ID="RunActivateCheckBox" runat="server" Text="Activate" AutoPostBack="True" OnCheckedChanged="RunActivateCheckBox_CheckedChanged" /><br />
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;WL:
                        <asp:TextBox ID="WLTextBox" runat="server" AutoPostBack="True" OnTextChanged="WLTextBox_TextChanged" Width="144px"></asp:TextBox>
                        &nbsp;
                        <asp:RadioButton ID="WLFindRadioButton" runat="server" Text="Find Only" AutoPostBack="True" Checked="True" GroupName="wl" OnCheckedChanged="WLFindRadioButton_CheckedChanged" />
                        or&nbsp;
                        <asp:RadioButton ID="WLNotRadioButton" runat="server" Text="Exclude" AutoPostBack="True" GroupName="wl" OnCheckedChanged="WLNotRadioButton_CheckedChanged" />
                        &nbsp;&nbsp;
                        <asp:CheckBox ID="WLActivateCheckBox" runat="server" Text="Activate" AutoPostBack="True" OnCheckedChanged="WLActivateCheckBox_CheckedChanged" /><br />
                        Session&nbsp; Name:
                        <asp:TextBox ID="SessionTextBox" runat="server" Width="144px" AutoPostBack="True" OnTextChanged="SessionTextBox_TextChanged"></asp:TextBox>
                        &nbsp;
                        <asp:RadioButton ID="SessionFindRadioButton" runat="server" Text="Find Only" AutoPostBack="True" Checked="True" GroupName="sessionName" OnCheckedChanged="SessionFindRadioButton_CheckedChanged" />
                        or&nbsp;
                        <asp:RadioButton ID="SessionNotRadioButton" runat="server" Text="Exclude" AutoPostBack="True" GroupName="sessionName" OnCheckedChanged="SessionNotRadioButton_CheckedChanged" />
                        &nbsp;&nbsp;
                        <asp:CheckBox ID="SessionActivateCheckBox" runat="server" Text="Activate" AutoPostBack="True" OnCheckedChanged="SessionActivateCheckBox_CheckedChanged" />
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp; 
                        Comment 1:
                        <asp:TextBox ID="Comment1TextBox" runat="server" AutoPostBack="True" Width="144px"></asp:TextBox>
                        &nbsp;
                        <asp:RadioButton ID="Comment1FindRadioButton" runat="server" AutoPostBack="True" Checked="True" GroupName="Comm1" Text="Find Only" />
                        or&nbsp;
                        <asp:RadioButton ID="Comment1NotRadioButton" runat="server" AutoPostBack="True" GroupName="Comm1" Text="Exclude" />
                        &nbsp;&nbsp;
                        <asp:CheckBox ID="Comment1ActivateCheckBox" runat="server" AutoPostBack="True" Text="Activate" />
                        <br />
                        &nbsp;&nbsp;&nbsp;&nbsp; 
                        Comment 2:
                        <asp:TextBox ID="Comment2TextBox" runat="server" AutoPostBack="True" Width="144px"></asp:TextBox>
                        &nbsp;
                        <asp:RadioButton ID="Comment2FindRadioButton" runat="server" AutoPostBack="True" Checked="True" GroupName="Comm2" Text="Find Only" />
                        or&nbsp;
                        <asp:RadioButton ID="Comment2NotRadioButton" runat="server" AutoPostBack="True" GroupName="Comm2" Text="Exclude" />
                        &nbsp;&nbsp;
                        <asp:CheckBox ID="Comment2ActivateCheckBox" runat="server" AutoPostBack="True" Text="Activate" />
                        &nbsp;(Instence Numbers)<br />
                        &nbsp; &nbsp; &nbsp;Find Records Flaged With CMP Type&nbsp;
                        <asp:DropDownList ID="DropDownListCMP" runat="server" Width="160px" AutoPostBack="True">
                            <asp:ListItem>CMP 1</asp:ListItem>
                            <asp:ListItem>CMP 2</asp:ListItem>
                            <asp:ListItem>CMP 3</asp:ListItem>
                            <asp:ListItem>CMP 4L</asp:ListItem>
                            <asp:ListItem>CMP 4R</asp:ListItem>
                            <asp:ListItem>CMP 5</asp:ListItem>
                        </asp:DropDownList>
                        
                        &nbsp;&nbsp;
                        <asp:CheckBox ID="CMPCheckBox" runat="server" AutoPostBack="True" OnCheckedChanged="CMPCheckBox_CheckedChanged" Text="Activate" /><br />
                                                
                        &nbsp; &nbsp; &nbsp;Pass Bins = &nbsp;
                        <asp:RadioButton ID="RadioButtonPassBinBoth2_3" runat="server" AutoPostBack="True" GroupName="PassBins" Text="Bins 2 & 3" />&nbsp;
                        <asp:RadioButton ID="RadioButtonPassBins2" runat="server" AutoPostBack="True" GroupName="PassBins" Checked="True" Text="Bin 2" />&nbsp;
                        <asp:RadioButton ID="RadioButtonPassBins3" runat="server" AutoPostBack="True" GroupName="PassBins" Text="Bin 3" />
                        <br />
                        <br />
                            
                        &nbsp; &nbsp; &nbsp;
                        <asp:Button ID="ButtonAllSlots" runat="server" Text="Select All Slots" />
                        &nbsp; &nbsp; &nbsp;
                        <asp:Button ID="ButtonNoSlots" runat="server" Text="Unselect All Slots" />
                        &nbsp; &nbsp; &nbsp;
                        <asp:Button ID="ButtonRefresh" runat="server" Text="Refresh Data" />
                        <br />
                        
                        
                        <asp:CheckBox ID="S1CheckBox" runat="server" Checked="True" Text="1" Font-Size="Smaller" /> 
                        <asp:CheckBox ID="S2CheckBox" runat="server" Checked="True" Text="2" Font-Size="Smaller" />
                        <asp:CheckBox ID="S3CheckBox" runat="server" Checked="True" Text="3" Font-Size="Smaller" />
                        <asp:CheckBox ID="S4CheckBox" runat="server" Checked="True" Text="4" Font-Size="Smaller" />
                        <asp:CheckBox ID="S5CheckBox" runat="server" Checked="True" Text="5" Font-Size="Smaller" />
                        <asp:CheckBox ID="S6CheckBox" runat="server" Checked="True" Text="6" Font-Size="Smaller" />
                        <asp:CheckBox ID="S7CheckBox" runat="server" Checked="True" Text="7" Font-Size="Smaller" />
                        <asp:CheckBox ID="S8CheckBox" runat="server" Checked="True" Text="8" Font-Size="Smaller" />
                        <asp:CheckBox ID="S9CheckBox" runat="server" Checked="True" Text="9" Font-Size="Smaller" />
                        <asp:CheckBox ID="S10CheckBox" runat="server" Checked="True" Text="10" Font-Size="Smaller" />
                        <asp:CheckBox ID="S11CheckBox" runat="server" Checked="True" Text="11" Font-Size="Smaller" />
                        <asp:CheckBox ID="S12CheckBox" runat="server" Checked="True" Text="12" Font-Size="Smaller" />
                        <asp:CheckBox ID="S13CheckBox" runat="server" Checked="True" Text="13" Font-Size="Smaller" />
                        <asp:CheckBox ID="S14CheckBox" runat="server" Checked="True" Text="14" Font-Size="Smaller" />
                        <asp:CheckBox ID="S15CheckBox" runat="server" Checked="True" Text="15" Font-Size="Smaller" />
                        <asp:CheckBox ID="S16CheckBox" runat="server" Checked="True" Text="16" Font-Size="Smaller" />
                        <asp:CheckBox ID="S17CheckBox" runat="server" Checked="True" Text="17" Font-Size="Smaller" />
                        <asp:CheckBox ID="S18CheckBox" runat="server" Checked="True" Text="18" Font-Size="Smaller" />
                        <asp:CheckBox ID="S19CheckBox" runat="server" Checked="True" Text="19" Font-Size="Smaller" />
                        <asp:CheckBox ID="S20CheckBox" runat="server" Checked="True" Text="20" Font-Size="Smaller" />
                        <asp:CheckBox ID="S21CheckBox" runat="server" Checked="True" Text="21" Font-Size="Smaller" />
                        <asp:CheckBox ID="S22CheckBox" runat="server" Checked="True" Text="22" Font-Size="Smaller" />
                        <asp:CheckBox ID="S23CheckBox" runat="server" Checked="True" Text="23" Font-Size="Smaller" />
                        <asp:CheckBox ID="S24CheckBox" runat="server" Checked="True" Text="24" Font-Size="Smaller" />
                        <asp:CheckBox ID="S25CheckBox" runat="server" Checked="True" Text="25" Font-Size="Smaller" />
                        <br />
                    </asp:Panel>


                    <asp:Panel ID="Panel2" runat="server" Style="text-align: right" Width="990px">

                        <table class="MasterPagePanelSub">
                            <tr>
                                <td style="border-style: dotted; vertical-align: bottom; text-align: center;">
                                    
                                    Email:&nbsp;<asp:TextBox ID="TextBoxE" runat="server"></asp:TextBox> @purewafer.com&nbsp;&nbsp;<asp:Button ID="ButtonExport" runat="server" Text="Export Data" />
                                </td>
                                <td style="text-align: right">
                                    <asp:CheckBox ID="CheckBoxArchive" runat="server" AutoPostBack="True" Text="Archives" ToolTip="This will get Archive Records only. Not checked gets live last 90 days of records." />&nbsp; &nbsp; &nbsp;
                                    <asp:CheckBox ID="CheckBoxRemoveDaily" runat="server" AutoPostBack="True" Text="Remove Daily Tests" />&nbsp; &nbsp; &nbsp;
                                    <asp:CheckBox ID="FooterSumCheckBox" runat="server" AutoPostBack="True" OnCheckedChanged="FooterSumCheckBox_CheckedChanged" Text="Show Footer As Sum" />&nbsp;&nbsp;

                                </td>
                            </tr>
                        </table>

                        
                                                                                            
                    </asp:Panel>
                </asp:Panel>
                
                <br />
                
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <img src="../Color/Animated_LoadingBigger.gif" />Working...
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataSourceID="Sp1SqlDataSource" ForeColor="#333333" GridLines="None"  ShowFooter="True"  >
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" CommandName="Session" ImageUrl="~/Color/SavedStar.png" />
                        
                        <asp:BoundField DataField="SessionDate" HeaderText="SessionDate"  SortExpression="SessionDate" FooterText="SessionDate" >
                            <HeaderStyle Font-Size="Small" />
                            <HeaderStyle Wrap="false" />
                            <ControlStyle Font-Size="Medium" />                            
                            <ItemStyle Font-Size="Small" />
                            <FooterStyle Font-Size="Small" /> 
                        </asp:BoundField>
                                                     
                        <asp:BoundField DataField="Comment2" HeaderText="Com2(I)" SortExpression="Comment2" FooterText="Instence" >
                            <HeaderStyle Font-Size="Small" />
                            <HeaderStyle Wrap="false" />
                            <ControlStyle Font-Size="Medium" />                            
                            <ItemStyle Font-Size="Small" />
                            <FooterStyle Font-Size="Small" /> 
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="SPSessionName" HeaderText="SPSessionName" SortExpression="SPSessionName" FooterText="SPSessionName" >
                            <HeaderStyle Font-Size="Small" />
                            <HeaderStyle Wrap="false" />
                            <ControlStyle Font-Size="Medium" />                            
                            <ItemStyle Font-Size="Small" />
                            <FooterStyle Font-Size="Small" />                            
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Lot" HeaderText="Lot" SortExpression="Lot" FooterText="Lot" >
                            <HeaderStyle Font-Size="Small" />
                            <HeaderStyle Wrap="false" />
                            <ControlStyle Font-Size="Medium" />                            
                            <ItemStyle Font-Size="Small" />
                            <FooterStyle Font-Size="Small" /> 
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Wafers In" HeaderText="Qty In" ReadOnly="True" SortExpression="Wafers In" FooterText="Qty In" >
                            <HeaderStyle Font-Size="Small" />
                            <HeaderStyle Wrap="false" />
                            <ControlStyle Font-Size="Medium" />                            
                            <ItemStyle Font-Size="Small" />
                            <FooterStyle Font-Size="Small" /> 
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Passed" HeaderText="Passed" ReadOnly="True" SortExpression="Passed" FooterText="Passed" >
                            <HeaderStyle Font-Size="Small" />
                            <HeaderStyle Wrap="false" />
                            <ControlStyle Font-Size="Medium" />                            
                            <ItemStyle Font-Size="Small" />
                            <FooterStyle Font-Size="Small" /> 
                        </asp:BoundField>
                        
                        <asp:TemplateField HeaderText="%Pass" FooterText="%Pass">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Format((Eval("Passed")) / Eval("Wafers in"), "0.0%") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Font-Size="Small" />
                            <HeaderStyle Wrap="false" />
                            <ControlStyle Font-Size="Medium" />                            
                            <ItemStyle Font-Size="Small" />
                            <FooterStyle Font-Size="Small" /> 
                        </asp:TemplateField>
                        
                        <asp:BoundField DataField="Rejects" HeaderText="Rejects" ReadOnly="True" SortExpression="Rejects" FooterText="Rejects" >
                            <HeaderStyle Font-Size="Small" />
                            <HeaderStyle Wrap="false" />
                            <ControlStyle Font-Size="Medium" />                            
                            <ItemStyle Font-Size="Small" />
                            <FooterStyle Font-Size="Small" /> 
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="OverLoads" HeaderText="OverLoads" ReadOnly="True" SortExpression="OverLoads" FooterText="OverLoads" >
                            <HeaderStyle Font-Size="Small" />
                            <HeaderStyle Wrap="false" />
                            <ControlStyle Font-Size="Medium" />                            
                            <ItemStyle Font-Size="Small" />
                            <FooterStyle Font-Size="Small" /> 
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Bin2" HeaderText="Bin2" ReadOnly="True" SortExpression="Bin2" FooterText="Bin2" >
                            <HeaderStyle Font-Size="Small" />
                            <HeaderStyle Wrap="false" />
                            <ControlStyle Font-Size="Medium" />                            
                            <ItemStyle Font-Size="Small" />
                            <FooterStyle Font-Size="Small" /> 
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Bin3" HeaderText="Bin3" ReadOnly="True" SortExpression="Bin3" FooterText="Bin3" >
                            <HeaderStyle Font-Size="Small" />
                            <HeaderStyle Wrap="false" />
                            <ControlStyle Font-Size="Medium" />                            
                            <ItemStyle Font-Size="Small" />
                            <FooterStyle Font-Size="Small" /> 
                        </asp:BoundField>
                        
                        <asp:BoundField DataField="Comment1" HeaderText="Com1(CMP)" ReadOnly="True" SortExpression="Comment1" FooterText="CMP" >
                            <HeaderStyle Font-Size="Small" />
                            <HeaderStyle Wrap="false" />
                            <ControlStyle Font-Size="Medium" />                            
                            <ItemStyle Font-Size="Small" />
                            <FooterStyle Font-Size="Small" /> 
                        </asp:BoundField>
                        
                    </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <asp:SqlDataSource ID="Sp1SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:AutoDataConnectionString %>"
                    SelectCommand="SELECT TOP 25 SessionDate, Machine, SPSessionName, ID# + N'-' + RUN# + N'-' + Wafer_log AS Lot, COUNT(DestinationStationID) AS [Wafers In], SUM(CASE WHEN DispositionName = 'Rejected' THEN 0 WHEN DispositionName = 'Overload' THEN 0 WHEN DispositionName = 'RW' THEN 0 WHEN DispositionName = 'Rerun' THEN 0 ELSE 1 END) AS Passed, SUM(CASE WHEN DispositionName = 'Rejected' THEN 1 WHEN DispositionName = 'Overload' THEN 1 ELSE 0 END) AS Rejects, SUM(CASE WHEN DispositionName = 'Overload' THEN 1 ELSE 0 END) AS OverLoads, SUM(CASE WHEN DispositionName = 'RW' THEN 1 ELSE 0 END) AS Rework, SUM(CASE WHEN DispositionName = 'Rerun' THEN 1 ELSE 0 END) AS Reruns FROM dbo.SP1_Data GROUP BY Machine, SPSessionName, SessionDate, ID#, RUN#, Wafer_log, ID# + N'-' + RUN# + N'-' + Wafer_log HAVING (NOT (ID# = N'move')) AND (Machine = N'S') AND (NOT (RUN# = N'99999')) AND (NOT (Wafer_log = N'99999')) ORDER BY SessionDate DESC">
                </asp:SqlDataSource>
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataSourceID="TencorSqlDataSource" ForeColor="#333333" GridLines="None" ShowFooter="True"
                    Visible="False" Width="904px">
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:ButtonField ButtonType="Image" CommandName="Session" ImageUrl="~/Color/SavedStar.png"
                            Text="Button" />
                        <asp:BoundField DataField="Run Time" FooterText="Run Time" HeaderText="Run Time"
                            SortExpression="Run Time" />
                        <asp:BoundField DataField="Tencor" FooterText="Tencor" HeaderText="Tencor" SortExpression="Tencor" />
                        <asp:BoundField DataField="RECIPE" FooterText="RECIPE" HeaderText="RECIPE" SortExpression="RECIPE" />
                        <asp:BoundField DataField="OPERATOR" FooterText="OPERATOR" HeaderText="OPERATOR"
                            SortExpression="OPERATOR" />
                        <asp:BoundField DataField="Lot" FooterText="Lot" HeaderText="Lot" SortExpression="Lot" />
                        <asp:BoundField DataField="Wafers" FooterText="Wafers" HeaderText="Wafers" ReadOnly="True"
                            SortExpression="Wafers" />
                        <asp:BoundField DataField="Passed" FooterText="Passed" HeaderText="Passed" ReadOnly="True"
                            SortExpression="Passed" />
                        <asp:BoundField DataField="Reject" FooterText="Reject" HeaderText="Reject" ReadOnly="True"
                            SortExpression="Reject" />
                        <asp:TemplateField FooterText="%Pass" HeaderText="%Pass">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Format((Eval("Passed")) / Eval("Wafers"), "0.0%") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <asp:SqlDataSource ID="TencorSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:AutoDataConnectionString %>"
                    SelectCommand="SELECT TOP 100 PERCENT MAX(EventTime) AS [Run Time], MACHINE AS Tencor, RECIPE, OPERATOR, ID# + N'-' + RUN# + N'-' + WAFER_LOG AS Lot, COUNT(WAFER#) AS Wafers, SUM(CASE WHEN Sort = 'Pass' THEN 1 ELSE 0 END) AS Passed, SUM(CASE WHEN Sort = 'FAIL' THEN 1 ELSE 0 END) AS Reject FROM dbo.Tencor_Data WHERE (MACHINE = N'') GROUP BY RECIPE, OPERATOR, ID# + N'-' + RUN# + N'-' + WAFER_LOG, MACHINE ORDER BY MAX(EventTime) DESC">
                </asp:SqlDataSource>
                &nbsp;<br />
                <br />
            </asp:Panel>

            &nbsp;




            <cc1:ModalPopupExtender ID="SPxModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                DropShadow="True" PopupControlID="SPxDetailPanel" TargetControlID="SPxPopupFakeButton" CancelControlID="SPxCloseButton" Y="10">
            </cc1:ModalPopupExtender>

            &nbsp; &nbsp;&nbsp; &nbsp;
            
            <cc1:ModalPopupExtender ID="MapModalPopupExtender" runat="server"
                BackgroundCssClass="modalBackground" DropShadow="True" PopupControlID="MapPanel"
                TargetControlID="mapPopupFakeButton" Y="10">
            </cc1:ModalPopupExtender>

            <cc1:ModalPopupExtender ID="TencorModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
                DropShadow="True" OkControlID="SPxCloseButton" PopupControlID="TencorDetailPanel"
                TargetControlID="TencorPopupFakeButton" Y="0">
            </cc1:ModalPopupExtender>
            



                <asp:Panel ID="SPxDetailPanel" runat="server" BackColor="Silver" 
                HorizontalAlign="Center">
                    <br />
                    <table class="style1" style="text-align: center">
                        <tr>
                            <td style="text-align: right">
                                <asp:Label ID="LabelSpecTime" runat="server" Text="Spec Time xxxxxx , "></asp:Label>
                                <asp:Label ID="LabelSessionDate" runat="server" Text="Label"></asp:Label>
                                <asp:Button ID="SPxCloseButton" runat="server" Text="Close" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center">
                                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" 
                                    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                                    CellPadding="3" DataSourceID="SPxDetailSqlDataSource" ForeColor="Black" 
                                    GridLines="Vertical">
                                    <Columns>
                                        <asp:BoundField DataField="From" HeaderText="From" SortExpression="From" />
                                        <asp:BoundField DataField="To" HeaderText="To" SortExpression="To" />
                                        <asp:BoundField DataField="To Slot" HeaderText="To Slot" 
                                            SortExpression="To Slot" />
                                        <asp:BoundField DataField="Class" HeaderText="Class" SortExpression="Class" />
                                        <asp:BoundField DataField="SOD1" HeaderText="SOD1" 
                                            SortExpression="SOD1" />
                                        <asp:BoundField DataField="SOD2" HeaderText="SOD2" 
                                            SortExpression="SOD2" />
                                        <asp:BoundField DataField="SOD3" HeaderText="SOD3" 
                                            SortExpression="SOD3" />
                                        <asp:BoundField DataField="SOD4" HeaderText="SOD4" 
                                            SortExpression="SOD4" />
                                        <asp:BoundField DataField="SOD5" HeaderText="SOD5" 
                                            SortExpression="SOD5" />
                                        <asp:BoundField DataField="SOD6" HeaderText="SOD6" 
                                            SortExpression="SOD6" />
                                        <asp:BoundField DataField="SOD7" HeaderText="SOD7" 
                                            SortExpression="SOD7" />
                                        <asp:BoundField DataField="SOD8" HeaderText="SOD8" 
                                            SortExpression="SOD8" />
                                        <asp:BoundField DataField="SC" HeaderText="SC" SortExpression="SC" />
                                        <asp:BoundField DataField="CAC" HeaderText="CAC" SortExpression="CAC" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="MapButton" runat="server" CommandArgument='<%# Eval("Map") %>' 
                                                    CommandName="Map" Text="Map" Visible="False" />
                                                <asp:Label ID="MapfileLabel" runat="server" Text='<%# Eval("Map") %>' 
                                                    Visible="False"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:ButtonField ButtonType="Button" CommandName="NewMap" Text="Map" />
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" />
                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="#CCCCCC" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right">
                               <asp:Button ID="ButtonBackSession" runat="server" Text="Back Session" />&nbsp;&nbsp;&nbsp;
                               <asp:Button ID="ButtonNextSession" runat="server" Text="Next Session" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    
                    
                    <asp:SqlDataSource ID="SPxDetailSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:AutoDataConnectionString %>"
                        SelectCommand="SELECT SourceSlotID AS [From], DestinationStationID AS [To], DestinationSlotID AS [To Slot], DispositionName AS Class, SOD1, SOD2, SOD3, SOD4, SOD5, SOD6, SOD7, SOD8, ScratchCnt AS SC, ClusterAreaCnt AS CAC, Map FROM dbo.SP1_Data WHERE (SessionDate = CONVERT (DATETIME, '', 102))">
                    </asp:SqlDataSource>
                    
                    &nbsp;</asp:Panel>
            <br />
            
            &nbsp;<asp:Panel ID="TencorDetailPanel" runat="server" BackColor="Silver" Height="456px"
                HorizontalAlign="Center" ScrollBars="Vertical" Width="1014px">
                <table class="style1">
                    <tr>
                        <td style="text-align: right">
                            <asp:Button ID="tencorCloseButton" runat="server" Text="Close" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" BackColor="White"
                    BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" DataSourceID="TencorDetailSqlDataSource"
                    ForeColor="Black" GridLines="Vertical" Width="1024px">
                    <Columns>
                        <asp:BoundField DataField="EventTime" HeaderText="EventTime" SortExpression="EventTime" />
                        <asp:BoundField DataField="Slot" HeaderText="Slot" SortExpression="Slot" />
                        <asp:BoundField DataField="SORT" HeaderText="SORT" SortExpression="SORT" />
                        <asp:BoundField DataField="LPD Count" HeaderText="LPD Count" SortExpression="LPD Count" />
                        <asp:BoundField DataField="130" HeaderText="130" SortExpression="130" />
                        <asp:BoundField DataField="160" HeaderText="160" SortExpression="160" />
                        <asp:BoundField DataField="200" HeaderText="200" SortExpression="200" />
                        <asp:BoundField DataField="250" HeaderText="250" SortExpression="250" />
                        <asp:BoundField DataField="300" HeaderText="300" SortExpression="300" />
                        <asp:BoundField DataField="500" HeaderText="500" SortExpression="500" />
                        <asp:BoundField DataField="1000" HeaderText="1000" SortExpression="1000" />
                        <asp:BoundField DataField="SC" HeaderText="SC" SortExpression="SC" />
                        <asp:BoundField DataField="AC" HeaderText="AC" SortExpression="AC" />
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                </asp:GridView>
                <asp:SqlDataSource ID="TencorDetailSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:AutoDataConnectionString %>"
                    SelectCommand="SELECT EventTime, WAFER# AS Slot, SORT, LPD_COUNT AS [LPD Count], [130_BIN] AS [130], [160_BIN] AS [160], [200_BIN] AS [200], [250_BIN] AS [250], [300_BIN] AS [300], [500_BIN] AS [500], [1000_BIN] AS [1000], SCRATCH_COUNT AS SC, AREA_COUNT AS AC FROM dbo.Tencor_Data WHERE (RECIPE = N'') AND (OPERATOR = N'') AND (ID# = N'') AND (RUN# = N'') AND (WAFER_LOG = N'') AND (MACHINE = N'')">
                </asp:SqlDataSource>
            </asp:Panel>
            &nbsp;&nbsp;&nbsp;
            <asp:Panel ID="MapPanel" runat="server" BackColor="#E0E0E0" Width="1009px">
                <table class="style1">
                    <tr>
                        <td style="text-align: center">
                            <asp:Button ID="ButtonMapBackSession" runat="server" Text="Back Session" />&nbsp;
                        <asp:Label ID="MapRowLabel" runat="server" Text="test" Visible="False"></asp:Label>
                            <asp:Label ID="CSlotLabel" runat="server" Text="Label"></asp:Label>&nbsp;
                            <asp:Button ID="ButtonMapNextSession" runat="server" Text="Next Session" />
                        </td>
                        <td style="text-align: right">
                            
                            <asp:Label ID="LabelMapSessionName" runat="server" Text="Label"></asp:Label>
                            <asp:Button ID="MapCloseButton" runat="server" OnClick="MapCloseButton_Click" 
                                Text="Close" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Image ID="MapImage" runat="server" Width="1002px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: right">
                            <asp:Button ID="BackMapButton" runat="server" OnClick="BackMapButton_Click" 
                                Text="Back" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                           
                            <asp:Button ID="ButtonNextMap" runat="server" Text="Next" />
                        </td>
                    </tr>
                </table>
                
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    &nbsp;<br />
    <br />
    <br />
</asp:Content>

