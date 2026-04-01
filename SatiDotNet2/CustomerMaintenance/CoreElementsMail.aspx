<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="CoreElementsMail.aspx.vb" Inherits="DBMaintenance_CoreElementsMail" title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <strong>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        Welcome To Sati.Net Core Elements.....<br />
    </strong>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            Getting Data...<img src="../Color/Animated_LoadingBigger.gif" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="XX-Large" ForeColor="Red"
        Text="Warning !!!!!!!! Live Data!!!!!! "></asp:Label>
    <cc1:AnimationExtender ID="AnimationExtender1" runat="server" TargetControlID="Label2">
    </cc1:AnimationExtender>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="SelectCustomerPanel" runat="server" Font-Bold="True">
                Select Customer...<br />
                <asp:DropDownList ID="CustomerDropDownList" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                    DataSourceID="CustomerSqlDataSource" DataTextField="Business_Name" DataValueField="Customer_Name"
                    Width="808px" OnSelectedIndexChanged="CustomerDropDownList_SelectedIndexChanged" Font-Bold="True" Font-Size="XX-Large">
                    <asp:ListItem>Select Customer...</asp:ListItem>
                    <asp:ListItem>Add Customer...</asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;<br />
                <asp:Panel ID="CustomerPanel" runat="server" BackColor="LightBlue" Visible="False" Width="808px">
                    Add Customer<br />
                    <asp:TextBox ID="AddCustomerIDTextBox" runat="server" Width="328px"></asp:TextBox>
                    Customer ID &nbsp; ( Fab Name Like "NSC-TX")<br />
                    <asp:TextBox ID="AddCustomerBusinessNameTextBox" runat="server" Width="328px"></asp:TextBox>
                    Customer Business Name<br />
                    <asp:TextBox ID="AddCustomerNameTextBox" runat="server" Width="328px"></asp:TextBox>
                    Customer Name<br />
                    <asp:Button ID="AddCustomerButton" runat="server" OnClick="AddCustomerButton_Click"
                        Text="Add Customer" /><br />
                </asp:Panel>
                <br />
                Select Fab..<br />
                
    
                <asp:DropDownList ID="FabDropDownList" runat="server" AutoPostBack="True" DataSourceID="FabsOnlySqlDataSource"
                    DataTextField="CustomerID" DataValueField="CustomerID" Width="816px" AppendDataBoundItems="True" OnSelectedIndexChanged="FabDropDownList_SelectedIndexChanged" Font-Bold="True" Font-Size="XX-Large">
                    <asp:ListItem>Select Fab...</asp:ListItem>
                    <asp:ListItem>Add Fab...</asp:ListItem>
                </asp:DropDownList><br />
                <asp:Panel ID="FabPanel" runat="server" BackColor="LightBlue" Visible="False">
                    Customer Fab Detail&nbsp;<br />
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="CustomerID"
                    DataSourceID="FabDataSource" >
                    <Columns>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:BoundField DataField="CustomerID" HeaderText="CustomerID" ReadOnly="True" SortExpression="CustomerID" />
                        <asp:BoundField DataField="MacolaID" HeaderText="MacolaID" SortExpression="MacolaID" />
                        <asp:BoundField DataField="Supplier_Number" HeaderText="Supplier_Number" SortExpression="Supplier_Number" />
                        <asp:BoundField DataField="Transit_Days" HeaderText="Transit_Days" SortExpression="Transit_Days" />
                        <asp:BoundField DataField="Note1" HeaderText="Note1" SortExpression="Note1" />
                        <asp:BoundField DataField="Note2" HeaderText="Note2" SortExpression="Note2" />
                    </Columns>
                </asp:GridView>
                    <br />
                    <asp:Panel ID="NewFabPanel" runat="server" BackColor="White" Visible="False" Width="816px">
                        <asp:TextBox ID="NewFabNameTextBox" runat="server" Width="328px"></asp:TextBox>
                        Customer ID &nbsp; ( Fab Name Like "NSC-TX")<br />
                        <asp:TextBox ID="NewFabBusinessNameTextBox" runat="server" Width="328px"></asp:TextBox>
                        Customer Business Name<br />
                        <asp:TextBox ID="NewFabCustomerNameTextBox" runat="server" Width="328px"></asp:TextBox>
                        Customer Name<br />
                        <asp:TextBox ID="NewFabSupplierNumberTextBox" runat="server" Width="328px"></asp:TextBox>
                        Suppler Number<br />
                        <asp:TextBox ID="NewFabMacolaNumberTextBox" runat="server" Width="328px"></asp:TextBox>
                        Macola Accounting ID
                        <br />
                        <asp:TextBox ID="NewFabTransitDaysTextBox" runat="server" Width="328px"></asp:TextBox>
                        Transit Days ( Numbers Only )<br />
                        <asp:TextBox ID="NewFabNote1TextBox" runat="server" Width="328px"></asp:TextBox>
                        Note 1<br />
                        <asp:TextBox ID="NewFabNote2TextBox" runat="server" Width="328px"></asp:TextBox>
                        Note 2<br />
                        <asp:Button ID="FabAddButton" runat="server" OnClick="FabAddButton_Click" Text="Add Fab" /></asp:Panel>
                    <br />
                    Gray</asp:Panel>
                &nbsp;&nbsp;<br />
                &nbsp;&nbsp;<br />
                ID's<br />
                <asp:DropDownList ID="IDDropDownList" runat="server" AppendDataBoundItems="True"
                    AutoPostBack="True" DataSourceID="MainSqlDataSource" DataTextField="MainID" DataValueField="MainID"
                    Font-Bold="True" Font-Size="XX-Large" OnSelectedIndexChanged="IDDropDownList_SelectedIndexChanged"
                    Width="232px">
                </asp:DropDownList><br />
                &nbsp;<asp:Panel ID="NewIDPanel" runat="server" BackColor="LightBlue"
                    Height="50px" Width="125px" Visible="False">
                    Add New ID<br />
                </asp:Panel>
                <br />
                
                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server"
                    TargetControlID="IDContentPanel"
                    ExpandControlID="IDTitlePanel" 
                    CollapseControlID="IDTitlePanel" 
                    Collapsed="True"
                    TextLabelID="Label1" 
                    ExpandedText="(Hide Details...)" 
                    CollapsedText="(Show Details...)"
                    ImageControlID="Image1" 
                    ExpandedImage="~/Color/Up.gif" 
                    CollapsedImage="~/Color/Down.gif"
                   >
                </cc1:CollapsiblePanelExtender>
                
                <asp:Panel ID="IDTitlePanel" runat="server" BackColor="White" Height="16px" Visible="False"
                    Width="984px">
                    &nbsp;<asp:Image ID="Image1" runat="server" ImageUrl="~/Color/Down.gif"/>&nbsp;&nbsp;
                    ID Detail&nbsp; &nbsp;
                    <asp:Label ID="Label1" runat="server">(Show Details...)</asp:Label>&nbsp;</asp:Panel>
                 
                <asp:Panel ID="IDContentPanel" runat="server" Height="0" Width="296px" BackColor="LightBlue" Visible="False">
                    <br />
                    <asp:DropDownList ID="DiameterDropDownList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DiameterDropDownList_SelectedIndexChanged"
                        Width="56px">
                        <asp:ListItem>100</asp:ListItem>
                        <asp:ListItem>125</asp:ListItem>
                        <asp:ListItem>150</asp:ListItem>
                        <asp:ListItem>200</asp:ListItem>
                        <asp:ListItem>300</asp:ListItem>
                    </asp:DropDownList>
                    Diameter<br />
                    <asp:CheckBox ID="ReceivingIDCheckBox" runat="server" OnCheckedChanged="ReceivingIDCheckBox_CheckedChanged"
                        Text="Receiving ID" />&nbsp;&nbsp;
                <br />
                    <asp:CheckBox ID="SuppliedCheckBox" runat="server" Text="Pure Wafer Supplied" AutoPostBack="True" OnCheckedChanged="SuppliedCheckBox_CheckedChanged" /><br />
                    <asp:CheckBox ID="ConsignmentCheckBox" runat="server" Text="Consignment" AutoPostBack="True" OnCheckedChanged="ConsignmentCheckBox_CheckedChanged" /><br />
                    &nbsp;</asp:Panel>
                &nbsp;
                
                <br />
            
                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server"
                    TargetControlID="SpecContentPanel"
                    ExpandControlID="SpecTiltlePanel" 
                    CollapseControlID="SpecTiltlePanel" 
                    Collapsed="True"
                    TextLabelID="SpecTiltleLabel" 
                    ExpandedText="(Hide Details...)" 
                    CollapsedText="(Show Details...)"
                    ImageControlID="SpecTiltleImage" 
                    ExpandedImage="~/Color/Up.gif" 
                    CollapsedImage="~/Color/Down.gif"
                   >
                </cc1:CollapsiblePanelExtender>
                
                <asp:Panel ID="SpecTiltlePanel" runat="server" BackColor="White" Height="24px" Visible="False"
                    Width="984px">
                    &nbsp;<asp:Image ID="SpecTiltleImage" runat="server" ImageUrl="~/Color/Down.gif"/>&nbsp;&nbsp;
                    Spec Detail&nbsp; &nbsp;
                    <asp:Label ID="SpecTiltleLabel" runat="server">(Show Details...)</asp:Label>&nbsp;</asp:Panel>
                 
                <asp:Panel ID="SpecContentPanel" runat="server" Height="0" Width="984px" BackColor="LightBlue" Visible="False">
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                
                </asp:Panel>
                <br />
                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server"
                    TargetControlID="PackingContentPanel"
                    ExpandControlID="PackingTiltlePanel" 
                    CollapseControlID="PackingTiltlePanel" 
                    Collapsed="True"
                    TextLabelID="PackingTiltleLabel" 
                    ExpandedText="(Hide Details...)" 
                    CollapsedText="(Show Details...)"
                    ImageControlID="PackingTiltleImage" 
                    ExpandedImage="~/Color/Up.gif" 
                    CollapsedImage="~/Color/Down.gif"
                   >
                </cc1:CollapsiblePanelExtender>
                
                <asp:Panel ID="PackingTiltlePanel" runat="server" BackColor="White" Height="24px" Visible="False"
                    Width="984px">
                    &nbsp;<asp:Image ID="PackingTiltleImage" runat="server" ImageUrl="~/Color/Down.gif"/>&nbsp;&nbsp;
                    Packing &amp; Labeling Detail&nbsp; &nbsp;
                    <asp:Label ID="PackingTiltleLabel" runat="server">(Show Details...)</asp:Label>&nbsp;</asp:Panel>
                 
                <asp:Panel ID="PackingContentPanel" runat="server" Height="0" Width="376px" BackColor="LightBlue" Visible="False">
                    <br />
                    <asp:CheckBox ID="PO_OnLabelCheckBox" runat="server" OnCheckedChanged="PO_OnLabelCheckBox_CheckedChanged"
                        Text="PO On Label" /><br />
                    <asp:DropDownList ID="WafersPerCassetteDropDownList" runat="server" AutoPostBack="True"
                        Width="56px" OnSelectedIndexChanged="WafersPerCassetteDropDownList_SelectedIndexChanged">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                        <asp:ListItem>7</asp:ListItem>
                        <asp:ListItem>8</asp:ListItem>
                        <asp:ListItem>9</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                        <asp:ListItem>13</asp:ListItem>
                        <asp:ListItem>14</asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>16</asp:ListItem>
                        <asp:ListItem>17</asp:ListItem>
                        <asp:ListItem>18</asp:ListItem>
                        <asp:ListItem>19</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>21</asp:ListItem>
                        <asp:ListItem>22</asp:ListItem>
                        <asp:ListItem>23</asp:ListItem>
                        <asp:ListItem>24</asp:ListItem>
                        <asp:ListItem>25</asp:ListItem>
                    </asp:DropDownList>
                    Wafers Per Cassette<br />
                    <asp:DropDownList ID="WaferMinPerCassetteDropDownList" runat="server" AutoPostBack="True"
                        Width="56px" OnSelectedIndexChanged="WaferMinPerCassetteDropDownList_SelectedIndexChanged">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                        <asp:ListItem>7</asp:ListItem>
                        <asp:ListItem>8</asp:ListItem>
                        <asp:ListItem>9</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                        <asp:ListItem>13</asp:ListItem>
                        <asp:ListItem>14</asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>16</asp:ListItem>
                        <asp:ListItem>17</asp:ListItem>
                        <asp:ListItem>18</asp:ListItem>
                        <asp:ListItem>19</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>21</asp:ListItem>
                        <asp:ListItem>22</asp:ListItem>
                        <asp:ListItem>23</asp:ListItem>
                        <asp:ListItem>24</asp:ListItem>
                        <asp:ListItem>25</asp:ListItem>
                    </asp:DropDownList>
                    Wafer Min Per Cassette<br />
                    <asp:DropDownList ID="CassettesPerBoxDropDownList" runat="server" AutoPostBack="True"
                        Width="56px" OnSelectedIndexChanged="CassettesPerBoxDropDownList_SelectedIndexChanged">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                        <asp:ListItem>7</asp:ListItem>
                        <asp:ListItem>8</asp:ListItem>
                        <asp:ListItem>9</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                        <asp:ListItem>13</asp:ListItem>
                        <asp:ListItem>14</asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>16</asp:ListItem>
                        <asp:ListItem>17</asp:ListItem>
                        <asp:ListItem>18</asp:ListItem>
                        <asp:ListItem>19</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>21</asp:ListItem>
                        <asp:ListItem>22</asp:ListItem>
                        <asp:ListItem>23</asp:ListItem>
                        <asp:ListItem>24</asp:ListItem>
                        <asp:ListItem>25</asp:ListItem>
                        <asp:ListItem>26</asp:ListItem>
                        <asp:ListItem>27</asp:ListItem>
                        <asp:ListItem>28</asp:ListItem>
                        <asp:ListItem>29</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                    </asp:DropDownList>
                    Cassettes Per Box<br />
                    <br />
                    Packing Slip Note.<br />
                    <asp:TextBox ID="PackingSlipNoteTextBox" runat="server" Width="232px"></asp:TextBox>&nbsp;<asp:Button
                        ID="PackingNoteButton" runat="server" OnClick="PackingNoteButton_Click" Text="Change" /><br />
                    <br />
                    Think...<br />
                    View a Packing Slip<br />
                    View Cassette Label<br />
                    View Shipping Label<br />
                    View CofA Sheet<br />
                
                </asp:Panel>
                <br />
                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server"
                    TargetControlID="PathContentPanel"
                    ExpandControlID="PathTitlePanel" 
                    CollapseControlID="PathTitlePanel" 
                    Collapsed="True"
                    TextLabelID="PathTitleLabel" 
                    ExpandedText="(Hide Details...)" 
                    CollapsedText="(Show Details...)"
                    ImageControlID="PathTitleImage" 
                    ExpandedImage="~/Color/Up.gif" 
                    CollapsedImage="~/Color/Down.gif"
                   >
                </cc1:CollapsiblePanelExtender>
                
                <asp:Panel ID="PathTitlePanel" runat="server" BackColor="White" Height="24px" Visible="False"
                    Width="984px">
                    &nbsp;<asp:Image ID="PathTitleImage" runat="server" ImageUrl="~/Color/Down.gif"/>&nbsp;&nbsp;
                    Path Detail&nbsp; &nbsp;
                    <asp:Label ID="PathTitleLabel" runat="server">(Show Details...)</asp:Label>&nbsp;</asp:Panel>
                 
                <asp:Panel ID="PathContentPanel" runat="server" Height="0" Width="984px" BackColor="LightBlue" Visible="False">
                    Path Panel
                    <br />
                    <br />
                    Main Path
                    <br />
                    <br />
                    Rework Paths<br />
                    <br />
                    <br />
                                   
                </asp:Panel>
                <br />
                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender5" runat="server"
                    TargetControlID="DefectsContentPanel"
                    ExpandControlID="DefectsTitlePanel" 
                    CollapseControlID="DefectsTitlePanel" 
                    Collapsed="True"
                    TextLabelID="DefectsTitleLabel" 
                    ExpandedText="(Hide Details...)" 
                    CollapsedText="(Show Details...)"
                    ImageControlID="DefectsTitleImage" 
                    ExpandedImage="~/Color/Up.gif" 
                    CollapsedImage="~/Color/Down.gif"
                   >
                </cc1:CollapsiblePanelExtender>
                
                <asp:Panel ID="DefectsTitlePanel" runat="server" BackColor="White" Height="24px" Visible="False"
                    Width="984px">
                    &nbsp;<asp:Image ID="DefectsTitleImage" runat="server" ImageUrl="~/Color/Down.gif"/>&nbsp;&nbsp;
                    Defects Detail&nbsp; &nbsp;
                    <asp:Label ID="DefectsTitleLabel" runat="server">(Show Details...)</asp:Label>&nbsp;</asp:Panel>
                 
                <asp:Panel ID="DefectsContentPanel" runat="server" Height="0" Width="616px" BackColor="LightBlue" Visible="False">
                    <asp:CheckBox ID="AddDefectCheckBox" runat="server" AutoPostBack="True" OnCheckedChanged="AddDefectCheckBox_CheckedChanged"
                        Text="Check To Add A Defect" /><br />
                    <br />
                    <asp:Panel ID="DeffectAddPanel" runat="server" Visible="False" Width="300px">
                    Add A
                    Defect<br />
                    <br />
                    <asp:DropDownList ID="DefectsDropDownList" runat="server" AppendDataBoundItems="True" DataSourceID="AllDefectsNamesSqlDataSource"
                        DataTextField="DefectName" DataValueField="DefectName" Width="224px">
                        <asp:ListItem>Select Defect...</asp:ListItem>
                    </asp:DropDownList><br />
                    <br />
                    Type<br />
                    <asp:DropDownList ID="DefectTypeDropDownList" runat="server" Width="224px">
                        <asp:ListItem>Select Type...</asp:ListItem>
                        <asp:ListItem>Rework</asp:ListItem>
                        <asp:ListItem>Reject</asp:ListItem>
                    </asp:DropDownList><br />
                    <br />
                    Group<br />
                    <asp:DropDownList ID="DefectGroupDropDownList" runat="server" Width="224px">
                        <asp:ListItem>Select Group...</asp:ListItem>
                        <asp:ListItem>StripEtch</asp:ListItem>
                        <asp:ListItem>Polish</asp:ListItem>
                        <asp:ListItem>Reject</asp:ListItem>
                        <asp:ListItem>Lap</asp:ListItem>
                        <asp:ListItem>T7</asp:ListItem>
                    </asp:DropDownList><br />
                        <br />
                        <asp:Button ID="DefectAddButton" runat="server" OnClick="DefectAddButton_Click" Text="Add Defect" />
                        <asp:Label ID="DefectInfoLabel" runat="server" Width="216px"></asp:Label><br />
                    <asp:SqlDataSource ID="AllDefectsNamesSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                        SelectCommand="SELECT DefectName FROM dbo.DefectNames AS DefectNames_1 WHERE (DefectName NOT IN (SELECT Defect AS DefectName FROM dbo.T_ID_Defects WHERE (ID = '0001'))) AND (NOT (DefectName = N'test')) AND (NOT (DefectName = N'GFAA')) GROUP BY DefectName">
                    </asp:SqlDataSource>
                    </asp:Panel>
                    <br />
                    <asp:GridView ID="IDDefectsGridView" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                        CellPadding="4" DataKeyNames="Key" DataSourceID="IDDefectsSqlDataSource" ForeColor="#333333"
                        GridLines="None" >
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:CommandField ShowEditButton="True" >
                                <ItemStyle ForeColor="#00C000" />
                            </asp:CommandField>
                            <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True"
                                SortExpression="Key" Visible="False" />
                            <asp:TemplateField HeaderText="Defect" SortExpression="Defect">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Enabled="False" Text='<%# Bind("Defect") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("Defect") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Type" SortExpression="Type">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DropDownList1" runat="server" SelectedValue='<%# Bind("Type") %>'
                                        Width="80px">
                                        <asp:ListItem>Rework</asp:ListItem>
                                        <asp:ListItem>Reject</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Group" SortExpression="Group">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="DropDownList2" runat="server" SelectedValue='<%# Bind("Group") %>'>
                                        <asp:ListItem>StripEtch</asp:ListItem>
                                        <asp:ListItem>Polish</asp:ListItem>
                                        <asp:ListItem>Lap</asp:ListItem>
                                        <asp:ListItem>T7</asp:ListItem>
                                        <asp:ListItem>Reject</asp:ListItem>
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("Group") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowDeleteButton="True">
                                <ItemStyle ForeColor="Red" />
                            </asp:CommandField>
                        </Columns>
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                    <br />
                    <asp:SqlDataSource ID="IDDefectsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                        DeleteCommand="DELETE FROM [T_ID_Defects] WHERE [Key] = @Key" InsertCommand="INSERT INTO [T_ID_Defects] ([ID], [Defect], [Type], [Group]) VALUES (@ID, @Defect, @Type, @Group)"
                        SelectCommand="SELECT [Key], ID, Defect, Type, [Group] FROM dbo.T_ID_Defects WHERE (ID = '1234')"
                        UpdateCommand="UPDATE [T_ID_Defects] SET [Defect] = @Defect, [Type] = @Type, [Group] = @Group WHERE [Key] = @Key">
                        <DeleteParameters>
                            <asp:Parameter Name="Key" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="Defect" Type="String" />
                            <asp:Parameter Name="Type" Type="String" />
                            <asp:Parameter Name="Group" Type="String" />
                            <asp:Parameter Name="Key" Type="Int32" />
                        </UpdateParameters>
                        <InsertParameters>
                            <asp:Parameter Name="ID" Type="String" />
                            <asp:Parameter Name="Defect" Type="String" />
                            <asp:Parameter Name="Type" Type="String" />
                            <asp:Parameter Name="Group" Type="String" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                                   
                </asp:Panel>
                <br />
                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender6" runat="server"
                    TargetControlID="AddressContentPanel"
                    ExpandControlID="AddressTitlePanel" 
                    CollapseControlID="AddressTitlePanel" 
                    Collapsed="True"
                    TextLabelID="AddressTitleLabel" 
                    ExpandedText="(Hide Details...)" 
                    CollapsedText="(Show Details...)"
                    ImageControlID="AddressTitleImage" 
                    ExpandedImage="~/Color/Up.gif" 
                    CollapsedImage="~/Color/Down.gif"
                   >
                </cc1:CollapsiblePanelExtender>
                
                <asp:Panel ID="AddressTitlePanel" runat="server" BackColor="White" Height="24px" Visible="False"
                    Width="984px">
                    &nbsp;<asp:Image ID="AddressTitleImage" runat="server" ImageUrl="~/Color/Down.gif"/>&nbsp;&nbsp;
                    Address Detail&nbsp; &nbsp;
                    <asp:Label ID="AddressTitleLabel" runat="server">(Show Details...)</asp:Label>&nbsp;</asp:Panel>
                 
                <asp:Panel ID="AddressContentPanel" runat="server" Width="976px" BackColor="LightBlue" Visible="False">
                    <br />
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="Large" Text="Shipping Address"></asp:Label>
                    <asp:Label ID="ShipKeyLabel" runat="server" Text="Key" Visible="False"></asp:Label><br />
                    <asp:Panel ID="Panel1" runat="server" Width="976px">
                        <asp:Button ID="AddressShippingEditButton" runat="server" OnClick="AddressShippingEditButton_Click"
                            Text="New" /><br />
                        <asp:Panel ID="AdressAddShippingPanel" runat="server" Visible="False" Width="968px">
                            <asp:RadioButton ID="AddressShippingStreetRadioButton" runat="server" Text="Street Address" GroupName="ASType" AutoPostBack="True" Checked="True" OnCheckedChanged="AddressShippingStreetRadioButton_CheckedChanged" />&nbsp;<asp:RadioButton
                                ID="AddressShippingPOBoxRadioButton" runat="server" Text="PO Box Address" GroupName="ASType" AutoPostBack="True" OnCheckedChanged="AddressShippingPOBoxRadioButton_CheckedChanged" />
                            <br />
                            <br />
                            <asp:Label ID="Label6" runat="server" Text="Attn: "></asp:Label>
                            <asp:TextBox ID="ASAddAttnTextBox" runat="server" BackColor="White"></asp:TextBox>
                            &nbsp; &nbsp;
                            <asp:Label ID="Label7" runat="server" Text="Building: "></asp:Label>
                            <asp:TextBox ID="ASAddBuildingTextBox" runat="server" BackColor="White"></asp:TextBox>&nbsp;
                            <br />
                            <br />
                            <asp:Label ID="ASAddPOBoxLabel" runat="server" Text="PO Box" Visible="False"></asp:Label>
                            <asp:TextBox ID="ASAddPoBoxTextBox" runat="server" BackColor="White" Visible="False"></asp:TextBox><asp:Label ID="ASAddStreetNumberLabel" runat="server" Text="Street#: "></asp:Label>
                            <asp:TextBox ID="ASAddStreetNumberTextBox" runat="server" Width="56px" BackColor="White"></asp:TextBox>
                            &nbsp;&nbsp;
                            
                            <asp:Label ID="ASAddDirectionLabel" runat="server" Text="Direction:"></asp:Label>
                            <asp:TextBox ID="ASAddDirectionTextBox" runat="server" Width="40px" BackColor="White"></asp:TextBox>&nbsp;
                            
                            <asp:Label ID="ASAddStreetNameLabel" runat="server" Text="Street Name:"></asp:Label>
                            <asp:TextBox ID="ASAddStreetNameTextBox" runat="server" BackColor="White"></asp:TextBox>
                            
                            <asp:Label ID="ASAddStreetTypeLabel" runat="server" Text="Type:"></asp:Label>
                            <asp:TextBox ID="ASAddStreetTypeTextBox" runat="server" Width="40px" BackColor="White"></asp:TextBox><br />
                            <br />
                            
                            <asp:Label ID="Label12" runat="server" Text="City:"></asp:Label>
                            <asp:TextBox ID="ASAddCityTextBox" runat="server" BackColor="White"></asp:TextBox>
                            
                            <asp:Label ID="Label13" runat="server" Text="State:"></asp:Label>
                            <asp:TextBox ID="ASAddStateTextBox" runat="server" Width="40px" BackColor="White"></asp:TextBox>
                            
                            <asp:Label ID="Label14" runat="server" Text="Zip Code:"></asp:Label>
                            <asp:TextBox ID="ASAddZipCodeTextBox" runat="server" Width="40px" BackColor="White"></asp:TextBox>
                            
                            <asp:Label ID="Label15" runat="server" Text="Country:"></asp:Label>
                            <asp:TextBox ID="ASAddCountryTextBox" runat="server" BackColor="White"></asp:TextBox>
                            <br />
                            <br />
                            <asp:Label ID="Label16" runat="server" Text="Phone# "></asp:Label>
                            <asp:TextBox ID="ASAddPhoneTextBox" runat="server" BackColor="White"></asp:TextBox>
                            
                            <asp:Label ID="Label17" runat="server" Text="Fax#"></asp:Label>
                            <asp:TextBox ID="ASAddFaxTextBox" runat="server" BackColor="White"></asp:TextBox>
                            <br />
                            
                            <asp:Button ID="ASSaveButton" runat="server" OnClick="AddressShippingSaveButton_Click"
                            Text="Save" />
                            <br />

                            
                        </asp:Panel>
                        <br />
                        <asp:Label ID="AddressShip1Label" runat="server" Text="Label"></asp:Label>
                        <br />
                        <asp:Label ID="AddressShip2Label" runat="server" Text="Label"></asp:Label><br />
                        <asp:Label ID="AddressShip3Label" runat="server" Text="Label"></asp:Label><br />
                        <asp:Label ID="AddressShip4Label" runat="server" Text="Label"></asp:Label><br />
                        <asp:Label ID="AddressShip5Label" runat="server" Text="Label"></asp:Label><br />
                        <asp:Label ID="AddressShip6Label" runat="server" Text="Label"></asp:Label><br />
                    </asp:Panel>
                    <br />
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="Large" Text="Billing Address"></asp:Label>
                    <asp:Label ID="BillKeyLabel" runat="server" Text="Key" Visible="False"></asp:Label><br />
                    <asp:Panel ID="Panel2" runat="server" Width="648px">
                        <asp:Button ID="AddressBillingEditButton" runat="server" Text="New" OnClick="AddressBillingEditButton_Click" /><br />
                        <asp:Panel ID="AdressAddBillingPanel" runat="server" Visible="False" Width="968px">
                            <asp:RadioButton ID="AddressBillingStreetRadioButton" runat="server" Text="Street Address" GroupName="ASType" />&nbsp;<asp:RadioButton
                                ID="AddressBillingPOBoxRadioButton" runat="server" Text="PO Box Address" GroupName="ASType" />
                            <br />
                            <br />
                            <asp:Label ID="Label8" runat="server" Text="Attn: "></asp:Label>
                            <asp:TextBox ID="ABAddAttnTextBox" runat="server" BackColor="White"></asp:TextBox>
                            &nbsp; &nbsp;
                            <asp:Label ID="Label9" runat="server" Text="Building: "></asp:Label>
                            <asp:TextBox ID="ABAddBuildingTextBox" runat="server" BackColor="White"></asp:TextBox>&nbsp;
                            <br />
                            <br />
                            <asp:Label ID="ABAddPOBoxLabel" runat="server" Text="PO Box"></asp:Label>
                            <asp:TextBox ID="ABAddPoBoxTextBox" runat="server" BackColor="White"></asp:TextBox><asp:Label ID="ABAddStreetNumberLabel" runat="server" Text="Street#: "></asp:Label>
                            <asp:TextBox ID="ABAddStreetNumberTextBox" runat="server" Width="32px" BackColor="White"></asp:TextBox>
                            &nbsp;&nbsp;
                            
                            <asp:Label ID="ABAddDirectionLabel" runat="server" Text="Direction:"></asp:Label>
                            <asp:TextBox ID="ABAddDirectionTextBox" runat="server" Width="40px" BackColor="White"></asp:TextBox>&nbsp;
                            
                            <asp:Label ID="ABAddStreetNameLabel" runat="server" Text="Street Name:"></asp:Label>
                            <asp:TextBox ID="ABAddStreetNameTextBox" runat="server" BackColor="White"></asp:TextBox>
                            
                            <asp:Label ID="ABAddStreetTypeLabel" runat="server" Text="Type:"></asp:Label>
                            <asp:TextBox ID="ABAddStreetTypeTextBox" runat="server" Width="40px" BackColor="White"></asp:TextBox><br />
                            <br />
                            
                            <asp:Label ID="Label10" runat="server" Text="City:"></asp:Label>
                            <asp:TextBox ID="ABAddCityTextBox" runat="server" BackColor="White"></asp:TextBox>
                            
                            <asp:Label ID="Label11" runat="server" Text="State:"></asp:Label>
                            <asp:TextBox ID="ABAddStateTextBox" runat="server" Width="40px" BackColor="White"></asp:TextBox>
                            
                            <asp:Label ID="Label18" runat="server" Text="Zip Code:"></asp:Label>
                            <asp:TextBox ID="ABAddZipCodeTextBox" runat="server" Width="40px" BackColor="White"></asp:TextBox>
                            
                            <asp:Label ID="Label19" runat="server" Text="Country:"></asp:Label>
                            <asp:TextBox ID="ABAddCountryTextBox" runat="server" BackColor="White"></asp:TextBox>
                            <br />
                            <br />
                            <asp:Label ID="Label20" runat="server" Text="Phone# "></asp:Label>
                            <asp:TextBox ID="ABAddPhoneTextBox" runat="server" BackColor="White"></asp:TextBox>
                            
                            <asp:Label ID="Label21" runat="server" Text="Fax#"></asp:Label>
                            <asp:TextBox ID="ABAddFaxTextBox" runat="server" BackColor="White"></asp:TextBox>
                            <br />
                            
                            <asp:Button ID="ABSaveButton" runat="server" 
                            Text="Save" OnClick="ABSaveButton_Click" />
                            <br />

                            
                        </asp:Panel>
                        <br />
                        <asp:Label ID="AddressBill1Label" runat="server" Text="Label"></asp:Label><br />
                        <asp:Label ID="AddressBill2Label" runat="server" Text="Label"></asp:Label><br />
                        <asp:Label ID="AddressBill3Label" runat="server" Text="Label"></asp:Label><br />
                        <asp:Label ID="AddressBill4Label" runat="server" Text="Label"></asp:Label><br />
                        <asp:Label ID="AddressBill5Label" runat="server" Text="Label"></asp:Label><br />
                        <asp:Label ID="AddressBill6Label" runat="server" Text="Label"></asp:Label><br />
                    </asp:Panel>
                    <br />
                    <asp:SqlDataSource ID="AddressGetSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                        SelectCommand="SELECT MainID, Type, Row1, Row2, Row3, Row4, Row5, Row6, AddressKey FROM dbo.fctn_q_Customer_Address() AS fctn_q_Customer_Address_1 WHERE (MainID = N'2386')">
                    </asp:SqlDataSource>
                                   
                </asp:Panel>
                &nbsp;
                <br />
                <cc1:CollapsiblePanelExtender ID="CollapsiblePanelExtender7" runat="server"
                    TargetControlID="CofAInfoContentPanel"
                    ExpandControlID="CofAInfoTitlePanel" 
                    CollapseControlID="CofAInfoTitlePanel" 
                    Collapsed="True"
                    TextLabelID="CofAInfoTitleLabel" 
                    ExpandedText="(Hide Details...)" 
                    CollapsedText="(Show Details...)"
                    ImageControlID="CofAInfoTitleImage" 
                    ExpandedImage="~/Color/Up.gif" 
                    CollapsedImage="~/Color/Down.gif"
                   >
                </cc1:CollapsiblePanelExtender>
                
                <asp:Panel ID="CofAInfoTitlePanel" runat="server" BackColor="White" Height="24px" Visible="False"
                    Width="984px">
                    &nbsp;<asp:Image ID="CofAInfoTitleImage" runat="server" ImageUrl="~/Color/Down.gif"/>&nbsp;&nbsp;
                    CofA Detail&nbsp; &nbsp;
                    <asp:Label ID="CofAInfoTitleLabel" runat="server">(Show Details...)</asp:Label>&nbsp;</asp:Panel>
                 
                <asp:Panel ID="CofAInfoContentPanel" runat="server" Height="0" Width="984px" BackColor="LightBlue" Visible="False">
                    CofA Info Panel
                    <br />
                    <br />
                    <br />
                    <br />
                                   
                </asp:Panel>
                <br />
                &nbsp; &nbsp;&nbsp;
                <asp:SqlDataSource ID="CustomerSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT Business_Name, Customer_Name FROM dbo.Customer GROUP BY Business_Name, Customer_Name ORDER BY Business_Name">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="FabsOnlySqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT CustomerID FROM dbo.Customer WHERE (Business_Name = N'') ORDER BY CustomerID">
                </asp:SqlDataSource>
                &nbsp;
                <br />
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    DeleteCommand="DELETE FROM [Customer] WHERE [CustomerID] = @CustomerID" InsertCommand="INSERT INTO [Customer] ([CustomerID], [Customer_Name], [Business_Name], [MacolaID], [Supplier_Number], [Transit_Days], [Note1], [Note2]) VALUES (@CustomerID, @Customer_Name, @Business_Name, @MacolaID, @Supplier_Number, @Transit_Days, @Note1, @Note2)"
                    SelectCommand="SELECT [CustomerID], [Customer_Name], [Business_Name], [MacolaID], [Supplier_Number], [Transit_Days], [Note1], [Note2] FROM [Customer]"
                    UpdateCommand="UPDATE [Customer] SET [Customer_Name] = @Customer_Name, [Business_Name] = @Business_Name, [MacolaID] = @MacolaID, [Supplier_Number] = @Supplier_Number, [Transit_Days] = @Transit_Days, [Note1] = @Note1, [Note2] = @Note2 WHERE [CustomerID] = @CustomerID">
                    <DeleteParameters>
                        <asp:Parameter Name="CustomerID" Type="String" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Customer_Name" Type="String" />
                        <asp:Parameter Name="Business_Name" Type="String" />
                        <asp:Parameter Name="MacolaID" Type="String" />
                        <asp:Parameter Name="Supplier_Number" Type="String" />
                        <asp:Parameter Name="Transit_Days" Type="Byte" />
                        <asp:Parameter Name="Note1" Type="String" />
                        <asp:Parameter Name="Note2" Type="String" />
                        <asp:Parameter Name="CustomerID" Type="String" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="CustomerID" Type="String" />
                        <asp:Parameter Name="Customer_Name" Type="String" />
                        <asp:Parameter Name="Business_Name" Type="String" />
                        <asp:Parameter Name="MacolaID" Type="String" />
                        <asp:Parameter Name="Supplier_Number" Type="String" />
                        <asp:Parameter Name="Transit_Days" Type="Byte" />
                        <asp:Parameter Name="Note1" Type="String" />
                        <asp:Parameter Name="Note2" Type="String" />
                    </InsertParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="MainSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT MainID FROM dbo.MainID WHERE (CustomerID = N'')"></asp:SqlDataSource>
                <asp:SqlDataSource ID="FabDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    DeleteCommand="DELETE FROM [Customer] WHERE [CustomerID] = @CustomerID" InsertCommand="INSERT INTO [Customer] ([CustomerID], [Customer_Name], [Business_Name], [MacolaID], [Supplier_Number], [Transit_Days], [Note1], [Note2]) VALUES (@CustomerID, @Customer_Name, @Business_Name, @MacolaID, @Supplier_Number, @Transit_Days, @Note1, @Note2)"
                    SelectCommand="SELECT CustomerID, Customer_Name, Business_Name, MacolaID, Supplier_Number, Transit_Days, Note1, Note2 FROM dbo.Customer WHERE (CustomerID = N'ADV')"
                    UpdateCommand="UPDATE [Customer] SET [MacolaID] = @MacolaID, [Supplier_Number] = @Supplier_Number, [Transit_Days] = @Transit_Days, [Note1] = @Note1, [Note2] = @Note2 WHERE [CustomerID] = @CustomerID">
                    <DeleteParameters>
                        <asp:Parameter Name="CustomerID" Type="String" />
                    </DeleteParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="MacolaID" Type="String" />
                        <asp:Parameter Name="Supplier_Number" Type="String" />
                        <asp:Parameter Name="Transit_Days" Type="Byte" />
                        <asp:Parameter Name="Note1" Type="String" />
                        <asp:Parameter Name="Note2" Type="String" />
                        <asp:Parameter Name="CustomerID" Type="String" />
                    </UpdateParameters>
                    <InsertParameters>
                        <asp:Parameter Name="CustomerID" Type="String" />
                        <asp:Parameter Name="Customer_Name" Type="String" />
                        <asp:Parameter Name="Business_Name" Type="String" />
                        <asp:Parameter Name="MacolaID" Type="String" />
                        <asp:Parameter Name="Supplier_Number" Type="String" />
                        <asp:Parameter Name="Transit_Days" Type="Byte" />
                        <asp:Parameter Name="Note1" Type="String" />
                        <asp:Parameter Name="Note2" Type="String" />
                    </InsertParameters>
                </asp:SqlDataSource>
                &nbsp; &nbsp;</asp:Panel>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

