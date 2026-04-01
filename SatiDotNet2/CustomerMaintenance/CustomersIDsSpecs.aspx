<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="CustomersIDsSpecs.aspx.vb" Inherits="CustomerMaintenance_CustomersIDsSpecs" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="Panel1" runat="server">
        
            Cusomer, ID, Spec Edit<br />
            <br />
            Select Customer
            <br />
            <asp:DropDownList ID="DropDownListCustomersList" runat="server" 
                AppendDataBoundItems="True" AutoPostBack="True" 
                DataSourceID="SqlDataSourceCustomerlist" DataTextField="Customer_Name" 
                DataValueField="Customer_Name" Width="168px">
                <asp:ListItem>Select One...</asp:ListItem>
                <asp:ListItem>New Customer...</asp:ListItem>
            </asp:DropDownList>
            <br />
            <br /> 
           
            <asp:Panel ID="PanelCustomerSelected" runat="server" Visible="False">
             Select a Fab<br />
                <asp:GridView ID="GridViewSelectedCustomer" runat="server" 
                    AutoGenerateColumns="False" CellPadding="4" DataKeyNames="CustomerID" 
                    DataSourceID="SqlDataSourceCustomerSelected" Width="1000px" 
                    ForeColor="#333333" GridLines="None">
                    <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <Columns>
                        <asp:ButtonField ButtonType="Button" CommandName="GetFab" Text="Select" />
                        <asp:BoundField DataField="CustomerID" HeaderText="Fab" ReadOnly="True" 
                            SortExpression="CustomerID" />
                        <asp:BoundField DataField="Customer_Name" HeaderText="Customer" 
                            SortExpression="Customer_Name" />
                        <asp:BoundField DataField="Business_Name" HeaderText="Business Name" 
                            SortExpression="Business_Name" />
                        <asp:BoundField DataField="Operator" HeaderText="User" 
                            SortExpression="Operator" />
                        <asp:BoundField DataField="EventTime" DataFormatString="{0:d}" 
                            HeaderText="Date" SortExpression="EventTime" />
                        <asp:CommandField ShowEditButton="True" />
                    </Columns>
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:GridView>
            </asp:Panel>
            
            <asp:Panel ID="PanelCustomerNew" runat="server" Visible="False">
             Make New Customer a Fab<br />
                <asp:DetailsView ID="DetailsViewNewCustomer" runat="server" 
                    AutoGenerateRows="False" DataKeyNames="CustomerID" 
                    DataSourceID="SqlDataSourceNewCustomer" DefaultMode="Insert" Height="50px" 
                    Width="125px" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <CommandRowStyle BackColor="#E2DED6" Font-Bold="True" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <FieldHeaderStyle BackColor="#E9ECF1" Font-Bold="True" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <Fields>
                        <asp:BoundField DataField="CustomerID" HeaderText="Fab" ReadOnly="True" 
                            SortExpression="CustomerID" />
                        <asp:BoundField DataField="Customer_Name" HeaderText="Customer" 
                            SortExpression="Customer_Name" />
                        <asp:BoundField DataField="Business_Name" HeaderText="Business Name" 
                            SortExpression="Business_Name" />
                        <asp:BoundField DataField="PackingSlip_Note" HeaderText="PackingSlip Note" 
                            SortExpression="PackingSlip_Note" />
                        <asp:BoundField DataField="Operator" HeaderText="Operator" 
                            SortExpression="Operator" />
                        <asp:CommandField ShowInsertButton="True" />
                    </Fields>
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                </asp:DetailsView>
            </asp:Panel>
            <br />
            <asp:Panel ID="PanelSelectedFab" runat="server" Visible="False">
            Selected Fab: <asp:Label ID="LabelWorkingFab" runat="server"></asp:Label>&nbsp;<br />
                Select a ID:<br />
                <asp:DropDownList ID="DropDownListIDs" runat="server" 
                    AppendDataBoundItems="True" AutoPostBack="True" 
                    DataSourceID="SqlDataSourceFabIDs" DataTextField="MainID" 
                    DataValueField="MainID">
                </asp:DropDownList>
                    <asp:Panel ID="PanelBar" runat="server" BackColor="#5D7B9D" HorizontalAlign="Center" 
                    Visible="False" BorderColor="#666666" BorderStyle="Double">
                    <br />
                    <asp:CheckBox ID="CheckIDDetail" runat="server" Text="ID Detail" 
                        AutoPostBack="True" BorderColor="#666666" BorderStyle="Double" 
                            BorderWidth="5px" Width="125px" />
                    &nbsp;<asp:CheckBox ID="CheckCusomerSpec" runat="server" Text="Customer Spec" 
                        AutoPostBack="True" BorderColor="#666666" BorderStyle="Double" 
                            BorderWidth="5px" Width="125px"/>
                    &nbsp;<asp:CheckBox ID="CheckBoxAddress" runat="server" Text="Address" 
                        AutoPostBack="True" BorderColor="#666666" BorderStyle="Double" 
                            BorderWidth="5px" Width="125px"/>
                    &nbsp;<asp:CheckBox ID="CheckBoxLabels" runat="server" Text="Labels" 
                        AutoPostBack="True" BorderColor="#666666" BorderStyle="Double" 
                            BorderWidth="5px" Width="125px"/>
                    &nbsp;<asp:CheckBox ID="CheckBoxPaths" runat="server" Text="Paths" 
                        AutoPostBack="True" BorderColor="#666666" BorderStyle="Double" 
                            BorderWidth="5px" Width="125px"/>
                    &nbsp;<asp:CheckBox ID="CheckBoxDefects" runat="server" Text="Defects" 
                        AutoPostBack="True" BorderColor="#666666" BorderStyle="Double" 
                            BorderWidth="5px" Width="125px"/>
                    <br />
                    <br />
                </asp:Panel>
                <br />
                <table class="style1">
                    <tr>
                        <td style="vertical-align: top">
                            <asp:Panel ID="PanelIDDetail" runat="server" Visible="False">
                                
                                ID Detail<asp:FormView ID="FormViewSelectedID" runat="server" BorderColor="#5D7B9D" 
                                BorderStyle="Double" BorderWidth="5px" CellPadding="4" DataKeyNames="MainID" 
                                DataSourceID="SqlDataSourceSelectedID" ForeColor="#333333" Width="415px">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                <EditItemTemplate>
                                    <table class="style1">
                                        <tr>
                                            <td>
                                                ID:</td>
                                            <td>
                                                <asp:Label ID="MainIDLabel" runat="server" Text='<%# Eval("MainID") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Fab:</td>
                                            <td>
                                                <asp:TextBox ID="CustomerIDLabel" runat="server" 
                                                    Text='<%# Bind("CustomerID") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Receiving ID</td>
                                            <td>
                                                <asp:CheckBox ID="column1CheckBox" runat="server" 
                                                    Checked='<%# Bind("column1") %>' Enabled="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                PO On Label:</td>
                                            <td>
                                                <asp:CheckBox ID="PO_On_LabelCheckBox" runat="server" 
                                                    Checked='<%# Bind("PO_On_Label") %>' Enabled="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                RF-ID Enabled:</td>
                                            <td>
                                                <asp:CheckBox ID="CheckBoxRF" runat="server" 
                                                    Checked='<%# Bind("RFID_Enable") %>' Enabled="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Diameter:</td>
                                            <td>
                                                <asp:TextBox ID="DiameterLabel" runat="server" Text='<%# Bind("Diameter") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Max Wafers Pre Cassette</td>
                                            <td>
                                                <asp:TextBox ID="WAFERS_PER_CASSLabel" runat="server" 
                                                    Text='<%# Bind("WAFERS_PER_CASS") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Min Wafers Per Cassette:</td>
                                            <td>
                                                <asp:TextBox ID="Minimum_Per_CassLabel" runat="server" 
                                                    Text='<%# Bind("Minimum_Per_Cass") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Cassettes Pre Box:</td>
                                            <td>
                                                <asp:TextBox ID="CassetteLabel" runat="server" Text='<%# Bind("Cassette") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                PackingSlip Note:</td>
                                            <td>
                                                <asp:TextBox ID="PackingSlip_NoteLabel" runat="server" 
                                                    Text='<%# Bind("PackingSlip_Note") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                CrossFabShip:
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="CrossFabShipCheckBox" runat="server" 
                                                    Checked='<%# Bind("CrossFabShip") %>' Enabled="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Consignment:</td>
                                            <td>
                                                <asp:CheckBox ID="ConsignmentCheckBox" runat="server" 
                                                    Checked='<%# Bind("Consignment") %>' Enabled="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                PWI Supplied:</td>
                                            <td>
                                                <asp:CheckBox ID="Exsil_SuppliedCheckBox" runat="server" 
                                                    Checked='<%# Bind("Exsil_Supplied") %>' Enabled="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Operator:</td>
                                            <td>
                                                <asp:TextBox ID="OperatorLabel" runat="server" Text='<%# Bind("Operator") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                EffectiveDtd:</td>
                                            <td>
                                                <asp:TextBox ID="EffectiveDtdLabel" runat="server" 
                                                    Text='<%# Bind("EffectiveDtd") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                EventTime:</td>
                                            <td>
                                                <asp:Label ID="EventTimeLabel" runat="server" Text='<%# Bind("EventTime") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
                                        CommandName="Update" Text="Update" />
                                    &nbsp;&nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" 
                                        CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                                </EditItemTemplate>
                                <InsertItemTemplate>
                                    <table class="style1">
                                        <tr>
                                            <td>
                                                ID:</td>
                                            <td>
                                                <asp:TextBox ID="MainIDLabel" runat="server" Text='<%# Bind("MainID") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Fab:</td>
                                            <td>
                                                <asp:TextBox ID="FabTextBox" runat="server" Text='<%# Bind("CustomerID") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Receiving ID</td>
                                            <td>
                                                <asp:CheckBox ID="column1CheckBox" runat="server" 
                                                    Checked='<%# Bind("column1") %>' Enabled="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                PO On Label:</td>
                                            <td>
                                                <asp:CheckBox ID="PO_On_LabelCheckBox" runat="server" 
                                                    Checked='<%# Bind("PO_On_Label") %>' Enabled="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                RF_ID Enabled:</td>
                                            <td>
                                                <asp:CheckBox ID="CheckBoxRF" runat="server" 
                                                    Checked='<%# Bind("RFID_Enable") %>' Enabled="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Diameter:</td>
                                            <td>
                                                <asp:DropDownList ID="DiameterDropdownlist" runat="server" 
                                                    SelectedValue='<%# Bind("Diameter") %>' Width="127px">
                                                    <asp:ListItem>100</asp:ListItem>
                                                    <asp:ListItem>125</asp:ListItem>
                                                    <asp:ListItem>150</asp:ListItem>
                                                    <asp:ListItem>200</asp:ListItem>
                                                    <asp:ListItem>300</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Max Wafers Pre Cassette</td>
                                            <td>
                                                <asp:TextBox ID="WPC_Textbox" runat="server" 
                                                    Text='<%# Bind("WAFERS_PER_CASS") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Min Wafers Per Cassette:</td>
                                            <td>
                                                <asp:TextBox ID="MPC_Textbox" runat="server" 
                                                    Text='<%# Bind("Minimum_Per_Cass") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Cassettes Pre Box:</td>
                                            <td>
                                                <asp:TextBox ID="CPB_Textbox" runat="server" Text='<%# Bind("Cassette") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                PackingSlip Note:</td>
                                            <td>
                                                <asp:TextBox ID="PackingSlip_NoteLabel" runat="server" 
                                                    Text='<%# Bind("PackingSlip_Note") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                CrossFabShip:
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="CrossFabShipCheckBox" runat="server" 
                                                    Checked='<%# Bind("CrossFabShip") %>' Enabled="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Consignment:</td>
                                            <td>
                                                <asp:CheckBox ID="ConsignmentCheckBox" runat="server" 
                                                    Checked='<%# Bind("Consignment") %>' Enabled="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                PWI Supplied:</td>
                                            <td>
                                                <asp:CheckBox ID="Exsil_SuppliedCheckBox" runat="server" 
                                                    Checked='<%# Bind("Exsil_Supplied") %>' Enabled="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Operator:</td>
                                            <td>
                                                <asp:TextBox ID="UserTextbox" runat="server" Text='<%# Bind("Operator") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                EffectiveDtd:</td>
                                            <td>
                                                <asp:TextBox ID="ED_Textbox" runat="server" 
                                                    Text='<%# Bind("EffectiveDtd") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                EventTime:</td>
                                            <td>
                                                <asp:TextBox ID="ET_Textbox" runat="server" Text='<%# Bind("EventTime") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" 
                                        CommandName="Insert" Text="Insert" />
                                    &nbsp;&nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" 
                                        CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                                </InsertItemTemplate>
                                <ItemTemplate>
                                    <table class="style1">
                                        <tr>
                                            <td>
                                                ID:</td>
                                            <td>
                                                <asp:Label ID="MainIDLabel" runat="server" Text='<%# Eval("MainID") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Fab:</td>
                                            <td>
                                                <asp:Label ID="CustomerIDLabel" runat="server" 
                                                    Text='<%# Bind("CustomerID") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Receiving ID</td>
                                            <td>
                                                <asp:CheckBox ID="column1CheckBox" runat="server" 
                                                    Checked='<%# Bind("column1") %>' Enabled="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                PO On Label:</td>
                                            <td>
                                                <asp:CheckBox ID="PO_On_LabelCheckBox" runat="server" 
                                                    Checked='<%# Bind("PO_On_Label") %>' Enabled="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                RF-ID Enabled:</td>
                                            <td>
                                                <asp:CheckBox ID="CheckBoxRF" runat="server" 
                                                    Checked='<%# Bind("RFID_Enable") %>' Enabled="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Diameter:</td>
                                            <td>
                                                <asp:Label ID="DiameterLabel" runat="server" Text='<%# Bind("Diameter") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Max Wafers Pre Cassette</td>
                                            <td>
                                                <asp:Label ID="WAFERS_PER_CASSLabel" runat="server" 
                                                    Text='<%# Bind("WAFERS_PER_CASS") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Min Wafers Per Cassette:</td>
                                            <td>
                                                <asp:Label ID="Minimum_Per_CassLabel" runat="server" 
                                                    Text='<%# Bind("Minimum_Per_Cass") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Cassettes Pre Box:</td>
                                            <td>
                                                <asp:Label ID="CassetteLabel" runat="server" Text='<%# Bind("Cassette") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                PackingSlip Note:</td>
                                            <td>
                                                <asp:Label ID="PackingSlip_NoteLabel" runat="server" 
                                                    Text='<%# Bind("PackingSlip_Note") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                CrossFabShip:
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="CrossFabShipCheckBox" runat="server" 
                                                    Checked='<%# Bind("CrossFabShip") %>' Enabled="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Consignment:</td>
                                            <td>
                                                <asp:CheckBox ID="ConsignmentCheckBox" runat="server" 
                                                    Checked='<%# Bind("Consignment") %>' Enabled="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                PWI Supplied:</td>
                                            <td>
                                                <asp:CheckBox ID="Exsil_SuppliedCheckBox" runat="server" 
                                                    Checked='<%# Bind("Exsil_Supplied") %>' Enabled="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Operator:</td>
                                            <td>
                                                <asp:Label ID="OperatorLabel" runat="server" Text='<%# Bind("Operator") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                EffectiveDtd:</td>
                                            <td>
                                                <asp:Label ID="EffectiveDtdLabel" runat="server" 
                                                    Text='<%# Bind("EffectiveDtd") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                EventTime:</td>
                                            <td>
                                                <asp:Label ID="EventTimeLabel" runat="server" Text='<%# Bind("EventTime") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table class="style1">
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="EditButton" runat="server" CausesValidation="False" 
                                                    CommandName="Edit" Text="Edit" />
                                            </td>
                                            <td style="text-align: right">
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#999999" />
                            </asp:FormView>
                                <br />
                                <asp:Panel ID="PanelFabTranList" runat="server">
                                    Shipping ID Change:<br />
                                    <asp:Button ID="ButtonAddTransferShipID" runat="server" Text="Add Shipping Transfer ID" /><br />
                                    <asp:Panel ID="PanelAddTransferShipID" runat="server" Visible="False">
                                        Allow ID:&nbsp; <asp:Label ID="Label_FromShipTransferID" runat="server" Text="0000"></asp:Label><br />
                                        Transfer To Fab:&nbsp;  <asp:DropDownList ID="DropDownList_ToShipTransferFab" runat="server" Width="150px" AutoPostBack="True" DataSourceID="SqlDataSource_TransferFabs" DataTextField="CustomerID" DataValueField="CustomerID">
                                            <asp:ListItem Selected="True">Select One..</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource_TransferFabs" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT CustomerID FROM Customer WHERE (Customer_Name = N'Customers') AND (NOT (CustomerID = N'NotTheSelectedCustomer'))"></asp:SqlDataSource>
                                        <br />
                                        Change to ID :&nbsp; <asp:DropDownList ID="DropDownList_ToShipTransferID" runat="server" Width="150px" DataSourceID="SqlDataSource_TransferIDList" DataTextField="MainID" DataValueField="MainID"></asp:DropDownList> 
                                        <asp:SqlDataSource ID="SqlDataSource_TransferIDList" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT MainID FROM MainID WHERE (CustomerID = N'fab')"></asp:SqlDataSource>
                                        <br />                                       
                                        <br />
                                        <asp:Button ID="Button_SaveTransferShipID" runat="server" Text="Save" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="Button_CloseAddTransferShipID_Panel" runat="server" Text="Close" />
                                    </asp:Panel>
                                    <br />
                                    <asp:GridView ID="GridViewCrossFabShipID" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="Key" DataSourceID="SqlDataSourceCrossFabShipID" ForeColor="#333333" GridLines="None">
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True" SortExpression="Key" />
                                            <asp:BoundField DataField="MainID" HeaderText="MainID" SortExpression="MainID" />
                                            <asp:BoundField DataField="TranID" HeaderText="TranID" SortExpression="TranID" />
                                            <asp:BoundField DataField="Fab" HeaderText="Fab" SortExpression="Fab" />
                                            <asp:ButtonField ButtonType="Button" CommandName="RemoveRecord" Text="Delete" />
                                        </Columns>
                                        <EditRowStyle BackColor="#2461BF" />
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#EFF3FB" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSourceCrossFabShipID" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT [Key], MainID, TranID, Fab FROM T_Sati_CrossFabIDList WHERE (MainID = N'1111') ORDER BY TranID"></asp:SqlDataSource>
                                </asp:Panel>
                            </asp:Panel>
                             <br />
                            <asp:Panel ID="PanelAddress" runat="server" BorderColor="#5D7B9D" BorderStyle="Double" BorderWidth="5px" Visible="False">
                            Address:<br />
                                Current Shipping<br />
                                <asp:Label ID="LabelShippingLine1" runat="server" Text="None"></asp:Label><br />
                                <asp:Label ID="LabelShippingLine2" runat="server" Text="None"></asp:Label><br />
                                <asp:Label ID="LabelShippingLine3" runat="server" Text="None"></asp:Label><br />
                                <asp:Label ID="LabelShippingLine4" runat="server" Text="None"></asp:Label><br />
                                <asp:Label ID="LabelShippingLine5" runat="server" Text="None"></asp:Label><br />
                                <asp:Label ID="LabelShippingLine6" runat="server" Text="None"></asp:Label><br />
                                Current Billing<br />
                                <asp:Label ID="LabelBillingLine1" runat="server" Text="None"></asp:Label><br />
                                <asp:Label ID="LabelBillingLine2" runat="server" Text="None"></asp:Label><br />
                                <asp:Label ID="LabelBillingLine3" runat="server" Text="None"></asp:Label><br />
                                <asp:Label ID="LabelBillingLine4" runat="server" Text="None"></asp:Label><br />
                                <asp:Label ID="LabelBillingLine5" runat="server" Text="None"></asp:Label><br />
                                <asp:Label ID="LabelBillingLine6" runat="server" Text="None"></asp:Label><br />
                                <br />
                                <asp:Button ID="ButtonChangeAddress" runat="server" Text="Change Address" /><br />
                                                              
                                 <asp:Panel ID="PanelAddressChange" runat="server" Visible="false">                              
                                <table class="style1">
                                    <tr>
                                        <td style="vertical-align: top; height: 161px;">
                                            <asp:RadioButton ID="RadioButtonReUseShippingAddress" runat="server" Text="ReUse" GroupName="ShipMethod" Checked="True" AutoPostBack="True" />&nbsp;
                                            <asp:RadioButton ID="RadioButtonNewShippingAddress" runat="server"  Text="New" GroupName="ShipMethod" AutoPostBack="True" /><br />
                                            Shipping Key:&nbsp;<asp:Label ID="LabelCurrentShipKey" runat="server" Text="Label"></asp:Label>
                                            &nbsp;<br />
                                            <asp:label ID="TextBoxShippingLine1" runat="server" Width="260px"></asp:label><br />
                                            <asp:TextBox ID="TextBoxShippingLine2" runat="server" Width="260px"></asp:TextBox><br />
                                            <asp:TextBox ID="TextBoxShippingLine3" runat="server" Width="260px"></asp:TextBox><br />
                                            <asp:TextBox ID="TextBoxShippingLine4" runat="server" Width="260px"></asp:TextBox><br />
                                            <asp:TextBox ID="TextBoxShippingLine5" runat="server" Width="260px"></asp:TextBox><br />
                                            <asp:TextBox ID="TextBoxShippingLine6" runat="server" Width="260px"></asp:TextBox><br />
                                            <asp:Button ID="ButtonSaveShippingAddress" runat="server" Text="Save" />
                                            <asp:Label ID="LabelShipSave" runat="server"></asp:Label>
                                            <br />
                                            
                                            </td>
                                        <td style="vertical-align: top; height: 161px;">
                                            Other Fab Shipping Keys<br />
                                            <asp:ListBox ID="ListBoxShippingKeys" runat="server" AutoPostBack="True" 
                                                DataSourceID="SqlDataSourceShippingKeys" DataTextField="Address_Key" 
                                                DataValueField="Address_Key" Width="75px"></asp:ListBox>
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top">
                                        <asp:RadioButton ID="RadioButtonReUseBillingAddress" runat="server" Text="ReUse" GroupName="BillMethod" Checked="True" AutoPostBack="True" />&nbsp;
                                        <asp:RadioButton ID="RadioButtonNewBillingAddress" runat="server"  Text="New" GroupName="BillMethod" AutoPostBack="True" /><br />
                                            Billing Key:&nbsp;<asp:Label ID="LabelCurrentBillKey" runat="server" Text="Label"></asp:Label><br />
                                            <asp:label ID="TextBoxBillingLine1" runat="server" Width="260px"></asp:label><br />
                                            <asp:TextBox ID="TextBoxBillingLine2" runat="server" Width="260px"></asp:TextBox><br />
                                            <asp:TextBox ID="TextBoxBillingLine3" runat="server" Width="260px"></asp:TextBox><br />
                                            <asp:TextBox ID="TextBoxBillingLine4" runat="server" Width="260px"></asp:TextBox><br />
                                            <asp:TextBox ID="TextBoxBillingLine5" runat="server" Width="260px"></asp:TextBox><br />
                                            <asp:TextBox ID="TextBoxBillingLine6" runat="server" Width="260px"></asp:TextBox><br />
                                            <asp:Button ID="ButtonSaveBillingAddress" runat="server" Text="Save" />
                                            <asp:Label ID="LabelBillSaved" runat="server"></asp:Label>
                                            <br />
                                        </td>
                                        <td style="vertical-align: top">
                                        Other Fab Billing Keys<br />
                                            <asp:ListBox ID="ListBoxBillingKeys" runat="server" 
                                                DataSourceID="SqlDataSourceBillingKeys" DataTextField="Address_Key" 
                                                DataValueField="Address_Key" Width="75px" AutoPostBack="True"></asp:ListBox>
                                            <br />
                                        </td>
                                    </tr> 
                                 </table> <br />
                                 <asp:Button ID="ButtonCloseAddressPanel" runat="server" Text="Close" /><br />
                               </asp:Panel>
                                
                            </asp:Panel>
                            <br />
                            <br />
                            
                            <asp:Panel ID="PanelPaths" runat="server" BorderColor="#5D7B9D" 
                                    BorderStyle="Double" BorderWidth="5px" Visible="False" Width="486px">
                                <table class="style1">
                                    <tr>
                                        <td style="vertical-align: top">
                                            Paths
                                            <br />
                                            <br />
                                            Main Path<br />
                                            <asp:DropDownList ID="DropDownListMainPath" runat="server" 
                                                DataSourceID="SqlDataSourceMainPathList" DataTextField="PathName" 
                                                DataValueField="PathName" Width="230px" AppendDataBoundItems="True">
                                                <asp:ListItem>None</asp:ListItem>
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;
                                            <asp:Button ID="ButtonViewMainPath" runat="server" Text="View" />
                                            <br />
                                            <br />
                                            Lap Path<br />
                                            <asp:DropDownList ID="DropDownListLapPath" runat="server" Width="230px" 
                                                AppendDataBoundItems="True" DataSourceID="SqlDataSourceMainPathList" 
                                                DataTextField="PathName" DataValueField="PathName">
                                                <asp:ListItem>None</asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;
                                            <asp:Button ID="ButtonViewLapPath" runat="server" Text="View" />
                                            <br />
                                            <br />
                                            S&amp;E Path<br />
                                            <asp:DropDownList ID="DropDownListStripEtchPath" runat="server" Width="230px" 
                                                AppendDataBoundItems="True" DataSourceID="SqlDataSourceMainPathList" 
                                                DataTextField="PathName" DataValueField="PathName">
                                                <asp:ListItem>None</asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;
                                            <asp:Button ID="ButtonViewStripEtchPath" runat="server" Text="View" />
                                            <br />
                                            <br />
                                            Polish Path<br />
                                            <asp:DropDownList ID="DropDownListPolishPath" runat="server" Width="230px" 
                                                AppendDataBoundItems="True" DataSourceID="SqlDataSourceMainPathList" 
                                                DataTextField="PathName" DataValueField="PathName">
                                                <asp:ListItem>None</asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;
                                            <asp:Button ID="ButtonViewPolishPath" runat="server" Text="View" />
                                            <br />
                                            <br />
                                            CMP Polish Path<br />
                                            <asp:DropDownList ID="DropDownListCMPPath" runat="server" Width="230px" 
                                                AppendDataBoundItems="True" DataSourceID="SqlDataSourceMainPathList" 
                                                DataTextField="PathName" DataValueField="PathName">
                                                <asp:ListItem>None</asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;
                                            <asp:Button ID="ButtonViewCMPPath" runat="server" Text="View" />
                                            <br />
                                            <br />
                                            DSP Polish Path<br />
                                            <asp:DropDownList ID="DropDownListDSPPath" runat="server" Width="230px" 
                                                AppendDataBoundItems="True" DataSourceID="SqlDataSourceMainPathList" 
                                                DataTextField="PathName" DataValueField="PathName">
                                                <asp:ListItem>None</asp:ListItem>
                                            </asp:DropDownList>
                                            &nbsp;
                                            <asp:Button ID="ButtonViewDSPPath" runat="server" Text="View" />
                                            <br />
                                            <br />
                                            <asp:Button ID="ButtonSavePaths" runat="server" 
                                                Text="Save Current Path Latout" />
                                            <asp:Label ID="LabelPathsaved" runat="server"></asp:Label>
                                            </td>
                                            
                                        <td style="vertical-align: top">
                                            <asp:Panel ID="PanelViewSelctedPath" runat="server" Visible = "false">
                                                <asp:GridView ID="GridViewSelectedPathView" runat="server" 
                                                    AutoGenerateColumns="False" CellPadding="4" 
                                                    DataSourceID="SqlDataSourceViewPath" ForeColor="#333333" GridLines="None">
                                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Step" HeaderText="Step" SortExpression="Step" />
                                                        <asp:BoundField DataField="StageName" HeaderText="StageName" 
                                                            SortExpression="StageName" />
                                                    </Columns>
                                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                    <EditRowStyle BackColor="#999999" />
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                
                            </asp:Panel>
                                
                        </td>
                        <td style="vertical-align: top"><br />
                            <asp:Panel ID="PanelSpecCurrent" runat="server" Width="487px">
                                Current Spec:&nbsp;&nbsp;<asp:DetailsView ID="DetailsViewIDCurrentSpec" runat="server" 
                                    AutoGenerateRows="False" BorderColor="#5D7B9D" BorderStyle="Double" 
                                    BorderWidth="5px" DataSourceID="SqlDataSourceIDCurrentSpec" 
                                    GridLines="None" Height="50px" Width="409px">
                                    <Fields>
                                        <asp:BoundField DataField="MainID" HeaderText="ID" 
                                            SortExpression="MainID" />
                                        <asp:BoundField DataField="RecordNumber" HeaderText="Spec#Key" 
                                            InsertVisible="False" ReadOnly="True" SortExpression="RecordNumber" />
                                        <asp:BoundField DataField="PART_NUMBER" HeaderText="Part#" 
                                            SortExpression="PART_NUMBER" />
                                        <asp:BoundField DataField="PART_REV_NUMBER" HeaderText="Part# Rev" 
                                            SortExpression="PART_REV_NUMBER" />
                                        <asp:BoundField DataField="SPEC_NUMBER" HeaderText="Spec#" 
                                            SortExpression="SPEC_NUMBER" />
                                        <asp:BoundField DataField="SPEC_REV_NUMBER" HeaderText="Spec# Rev" 
                                            SortExpression="SPEC_REV_NUMBER" />
                                        <asp:BoundField DataField="thk_grp" HeaderText="Thick" 
                                            SortExpression="thk_grp" />
                                        <asp:BoundField DataField="res_grp" HeaderText="Res" SortExpression="res_grp" />
                                        <asp:BoundField DataField="ORTN" HeaderText="ORTN" SortExpression="ORTN" />
                                        <asp:BoundField DataField="WTYPE_DOPE" HeaderText="Type" 
                                            SortExpression="WTYPE_DOPE" />
                                        <asp:BoundField DataField="DOPE" HeaderText="DOPE" SortExpression="Dope" />
                                        <asp:BoundField DataField="SAMPLE_STANDARD" HeaderText="Sample Standard" 
                                            SortExpression="SAMPLE_STANDARD" />
                                        <asp:BoundField DataField="Label_Comments" HeaderText="Label Comments" 
                                            SortExpression="Label_Comments" />
                                        <asp:BoundField DataField="Label_Comments2" HeaderText="Label Comments2" 
                                            SortExpression="Label_Comments2" />
                                        <asp:BoundField DataField="Label_Comments3" HeaderText="Label Comments3" 
                                            SortExpression="Label_Comments3" />
                                        <asp:BoundField DataField="EffectiveDtd" HeaderText="EffectiveDtd" 
                                            SortExpression="EffectiveDtd" />
                                        <asp:BoundField DataField="ExpirationDtd" HeaderText="ExpirationDtd" 
                                            SortExpression="ExpirationDtd" />
                                    </Fields>
                                </asp:DetailsView>
                                <asp:Button ID="ButtonNewSpec" runat="server" Text="New Spec" />
                                <br />
                                 <asp:Panel ID="PanelNewSpec" runat="server" BorderColor="#5D7B9D" 
                                    BorderStyle="Double" BorderWidth="5px" Visible="False" Width="466px">
                                     <table class="style1">
                                         <tr>
                                             <td>
                                                 Part#</td>
                                             <td>
                                                 <asp:TextBox ID="TextBoxPart" runat="server" Width="150px"></asp:TextBox>
                                             </td>
                                             <td>
                                                 Part# Rev</td>
                                             <td>
                                                 <asp:TextBox ID="TextBoxPartRev" runat="server" Width="150px"></asp:TextBox>
                                             </td>
                                         </tr>
                                         <tr>
                                             <td>
                                                 Spec#</td>
                                             <td>
                                                 <asp:TextBox ID="TextBoxSpec" runat="server" Width="150px"></asp:TextBox>
                                             </td>
                                             <td>
                                                 Spec# Rev</td>
                                             <td>
                                                 <asp:TextBox ID="TextBoxSpecRev" runat="server" Width="150px"></asp:TextBox>
                                             </td>
                                         </tr>
                                         <tr>
                                             <td>
                                                 Thick</td>
                                             <td>
                                                 <asp:TextBox ID="TextBoxThick" runat="server" Width="150px"></asp:TextBox>
                                             </td>
                                             <td>
                                                 Res</td>
                                             <td>
                                                 <asp:TextBox ID="TextBoxRes" runat="server" Width="150px"></asp:TextBox>
                                             </td>
                                         </tr>
                                         <tr>
                                             <td>
                                                 Ortn</td>
                                             <td>
                                                 <asp:DropDownList ID="DropDownListOrtn" runat="server" 
                                                     DataSourceID="SqlDataSourceOrtn" DataTextField="ORTN" DataValueField="ORTN" 
                                                     Width="150px">
                                                 </asp:DropDownList>
                                             </td>
                                             <td>
                                                 Type</td>
                                             <td>
                                                 <asp:DropDownList ID="DropDownListType" runat="server" 
                                                     DataSourceID="SqlDataSourcetype" DataTextField="WTYPE_DOPE" 
                                                     DataValueField="WTYPE_DOPE" Width="150px">
                                                 </asp:DropDownList>
                                             </td>
                                         </tr>
                                         <tr>
                                             <td>
                                                 &nbsp;</td>
                                             <td>
                                                 &nbsp;</td>
                                             <td>
                                                 Dope</td>
                                             <td>
                                                 <asp:DropDownList ID="DropDownListDopeType" Width="150px" runat="server" 
                                                     DataSourceID="SqlDataSource1" DataTextField="DOPE" DataValueField="DOPE">
                                                 </asp:DropDownList>
                                             </td>
                                         </tr>
                                         <tr>
                                             <td colspan="2">
                                                 Sample Standard</td>
                                             <td colspan="2">
                                                 <asp:DropDownList ID="DropDownListSS" runat="server" Width="225px" 
                                                     DataSourceID="SqlDataSourceSS" DataTextField="SAMPLE_STANDARD" 
                                                     DataValueField="SAMPLE_STANDARD">
                                                 </asp:DropDownList>
                                             </td>
                                         </tr>
                                         <tr>
                                             <td>
                                                 &nbsp;</td>
                                             <td>
                                                 &nbsp;</td>
                                             <td>
                                                 &nbsp;</td>
                                             <td>
                                                 &nbsp;</td>
                                         </tr>
                                         <tr>
                                             <td colspan="2">
                                                 Label Comments1</td>
                                             <td colspan="2">
                                                 <asp:TextBox ID="TextBoxComment1" runat="server" Width="225px"></asp:TextBox>
                                             </td>
                                         </tr>
                                         <tr>
                                             <td colspan="2">
                                                 Label Comments2</td>
                                             <td colspan="2">
                                                 <asp:TextBox ID="TextBoxComment2" runat="server" Width="225px"></asp:TextBox>
                                             </td>
                                         </tr>
                                         <tr>
                                             <td colspan="2">
                                                 Label Comments3</td>
                                             <td colspan="2">
                                                 <asp:TextBox ID="TextBoxComment3" runat="server" Width="225px"></asp:TextBox>
                                             </td>
                                         </tr>
                                     </table><br />
                                     <table class="style1">
                                         <tr>
                                             <td>
                                                 <asp:Button ID="ButtonSaveSpec" runat="server" Text="Save" />
                                             </td>
                                             <td style="text-align: right">
                                                 <asp:Label ID="LabelCurrentSpec" runat="server"></asp:Label>
                                                 <asp:Button ID="ButtonCancelSpec" runat="server" Text="Cancel" />
                                             </td>
                                         </tr>
                                     </table>
                                     
                                </asp:Panel>
                                
                            </asp:Panel>
                            <br />
                            <br />
                            <asp:Panel ID="PanelLabels" runat="server" BorderColor="#5D7B9D" BorderStyle="Double" BorderWidth="5px" Visible ="false">
                            
                            </asp:Panel><br />
                            <br />
                            <asp:Panel ID="PanelDefects" runat="server" BorderColor="#5D7B9D" 
                                BorderStyle="Double" BorderWidth="5px" Width="465px" Visible ="false">
                                <table class="style1">
                                    <tr>
                                        <td>
                                            Clone Defects From ID:<br />
                                            <asp:DropDownList ID="DropDownListDefectCloneIDList" runat="server" 
                                                DataSourceID="SqlDataSourceAllIds" DataTextField="MainID" 
                                                DataValueField="MainID" Width="141px">
                                            </asp:DropDownList><br />
                                        </td>
                                        <td>
                                            Available Defects:&nbsp;
                                            <asp:DropDownList ID="DropDownListAvailableDefects" runat="server" 
                                                Width="161px" DataSourceID="SqlDataSourceAvailableDetects" 
                                                DataTextField="Defect" DataValueField="Defect">
                                            </asp:DropDownList><br />
                                            Type:&nbsp;
                                            <asp:DropDownList ID="DropDownListDefectType" runat="server">
                                            <asp:ListItem Selected="True">Reject</asp:ListItem>
                                            <asp:ListItem>Rework</asp:ListItem>
                                            </asp:DropDownList>&nbsp;&nbsp;
                                            Group:&nbsp;
                                            <asp:DropDownList ID="DropDownListDefectGroup" runat="server">
                                            <asp:ListItem Selected="True">Reject</asp:ListItem>
                                                    <asp:ListItem>StripEtch</asp:ListItem>
                                                    <asp:ListItem>Lap</asp:ListItem>
                                                    <asp:ListItem>Polish</asp:ListItem>
                                            </asp:DropDownList><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: center">
                                            <asp:Button ID="ButtonCloneDefects" runat="server" Text="Clone Defect" />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:Button ID="ButtonAddDefect" runat="server" Text="Add Defect" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <asp:GridView ID="GridViewDefects" runat="server" AutoGenerateColumns="False" 
                                    CellPadding="4" DataSourceID="SqlDataSourceCurrentDefects" ForeColor="#333333" 
                                    GridLines="None" Width="465px">
                                    <Columns>
                                        <asp:CommandField ShowEditButton="True" />
                                        <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" 
                                            ReadOnly="True" SortExpression="Key" />
                                        <asp:BoundField DataField="Defect" HeaderText="Defect" 
                                            SortExpression="Defect" />
                                        <asp:TemplateField HeaderText="Type" SortExpression="Type">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="DropDownList1" runat="server" 
                                                    SelectedValue='<%# Bind("Type") %>' Width="96px">
                                                    <asp:ListItem>Reject</asp:ListItem>
                                                    <asp:ListItem>Rework</asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Group" SortExpression="Group">
                                            <EditItemTemplate>
                                                <asp:DropDownList ID="DropDownList2" runat="server" 
                                                    SelectedValue='<%# Bind("Group") %>'>
                                                    <asp:ListItem Selected="True">Reject</asp:ListItem>
                                                    <asp:ListItem>StripEtch</asp:ListItem>
                                                    <asp:ListItem>Lap</asp:ListItem>
                                                    <asp:ListItem>Polish</asp:ListItem>
                                                </asp:DropDownList>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Group") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField ShowDeleteButton="True">
                                            <ItemStyle ForeColor="Red" />
                                        </asp:CommandField>
                                    </Columns>
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                    <EditRowStyle BackColor="#999999" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                </asp:GridView>
                                &nbsp;</asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
                <br />
            </asp:Panel>
            <br />
            
            <asp:SqlDataSource ID="SqlDataSourceAvailableDetects" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                SelectCommand="SELECT Defect FROM dbo.T_ID_Defects GROUP BY Defect HAVING (NOT (Defect IN (SELECT Defect FROM dbo.T_ID_Defects AS T_ID_Defects_1 WHERE (ID = '2386'))))">
            </asp:SqlDataSource>
            
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                SelectCommand="SELECT DOPE FROM dbo.MainIDSpec GROUP BY DOPE ORDER BY DOPE">
            </asp:SqlDataSource>
            
            <asp:SqlDataSource ID="SqlDataSourceShippingKeys" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                SelectCommand="SELECT dbo.MainID_Address.Address_Key FROM dbo.MainID_Address INNER JOIN dbo.MainID ON dbo.MainID_Address.MainID = dbo.MainID.MainID WHERE (dbo.MainID.CustomerID = N'') AND (dbo.MainID_Address.Address_Type = 0) GROUP BY dbo.MainID_Address.Address_Key">
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceBillingKeys" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                SelectCommand="SELECT dbo.MainID_Address.Address_Key FROM dbo.MainID_Address INNER JOIN dbo.MainID ON dbo.MainID_Address.MainID = dbo.MainID.MainID WHERE (dbo.MainID.CustomerID = N'') AND (dbo.MainID_Address.Address_Type = 1) GROUP BY dbo.MainID_Address.Address_Key">
            </asp:SqlDataSource>
            
            <asp:SqlDataSource ID="SqlDataSourceAllIds" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                SelectCommand="SELECT MainID FROM dbo.MainID WHERE (ExpirationDtd IS NULL) ORDER BY MainID">
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceOrtn" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                SelectCommand="SELECT ORTN FROM dbo.MainIDSpec GROUP BY ORTN ORDER BY ORTN">
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourcetype" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                SelectCommand="SELECT WTYPE_DOPE FROM dbo.MainIDSpec GROUP BY WTYPE_DOPE ORDER BY WTYPE_DOPE DESC">
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceSS" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                SelectCommand="SELECT SAMPLE_STANDARD FROM dbo.MainIDSpec GROUP BY SAMPLE_STANDARD ORDER BY SAMPLE_STANDARD">
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceMainPathList" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                SelectCommand="SELECT PathName FROM dbo.CannedPaths GROUP BY PathName ORDER BY PathName">
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceViewPath" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                SelectCommand="SELECT ProcessOrder AS Step, StageName FROM dbo.CannedPaths WHERE (PathName = N'') ORDER BY ProcessOrder">
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceFabIDs" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                SelectCommand="SELECT MainID, CustomerID FROM dbo.MainID WHERE (CustomerID = N'') ORDER BY MainID">
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceIDCurrentSpec" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                SelectCommand="SELECT dbo.MainID_MainIDSpec.MainID, dbo.MainIDSpec.RecordNumber, dbo.MainIDSpec.PART_NUMBER, dbo.MainIDSpec.PART_REV_NUMBER, dbo.MainIDSpec.SPEC_NUMBER, dbo.MainIDSpec.SPEC_REV_NUMBER, dbo.MainIDSpec.thk_grp, dbo.MainIDSpec.res_grp, dbo.MainIDSpec.ORTN, dbo.MainIDSpec.WTYPE_DOPE, dbo.MainIDSpec.DOPE, dbo.MainIDSpec.SAMPLE_STANDARD, dbo.MainID_MainIDSpec.Label_Comments, dbo.MainID_MainIDSpec.Label_Comments2, dbo.MainID_MainIDSpec.Label_Comments3, dbo.MainID_MainIDSpec.EffectiveDtd, dbo.MainID_MainIDSpec.ExpirationDtd FROM dbo.MainIDSpec INNER JOIN dbo.MainID_MainIDSpec ON dbo.MainIDSpec.RecordNumber = dbo.MainID_MainIDSpec.WaferSpec_Key WHERE (dbo.MainID_MainIDSpec.MainID = N'2386') AND (dbo.MainID_MainIDSpec.EffectiveDtd &lt;= { fn NOW() }) AND (dbo.MainID_MainIDSpec.ExpirationDtd IS NULL OR dbo.MainID_MainIDSpec.ExpirationDtd &gt;= { fn NOW() })">
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceSelectedID" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                DeleteCommand="DELETE FROM [MainID] WHERE [MainID] = @MainID" 
                InsertCommand="INSERT INTO [MainID] ([MainID], [CustomerID], [In-Out], [Diameter], [WAFERS_PER_CASS], [Minimum_Per_Cass], [Cassette], [PO_On_Label], [EffectiveDtd], [PackingSlip_Note], [Operator], [EventTime], [Exsil_Supplied], [Consignment], [CrossFabShip], [RFID_Enable]) VALUES (@MainID, @CustomerID, @column1, @Diameter, @WAFERS_PER_CASS, @Minimum_Per_Cass, @Cassette, @PO_On_Label, @EffectiveDtd, @PackingSlip_Note, @Operator, @EventTime, @Exsil_Supplied, @Consignment, @CrossFabShip, @RFID_Enable)" 
                SelectCommand="SELECT MainID, CustomerID, [In-Out] AS column1, Diameter, WAFERS_PER_CASS, Minimum_Per_Cass, Cassette, PO_On_Label, EffectiveDtd, PackingSlip_Note, Operator, EventTime, Exsil_Supplied, Consignment, CrossFabShip, RFID_Enable FROM dbo.MainID WHERE (MainID = N'')" 
                
                UpdateCommand="UPDATE [MainID] SET [CustomerID] = @CustomerID, [In-Out] = @column1, [Diameter] = @Diameter, [WAFERS_PER_CASS] = @WAFERS_PER_CASS, [Minimum_Per_Cass] = @Minimum_Per_Cass, [Cassette] = @Cassette, [PO_On_Label] = @PO_On_Label, [EffectiveDtd] = @EffectiveDtd, [PackingSlip_Note] = @PackingSlip_Note, [Operator] = @Operator, [EventTime] = @EventTime, [Exsil_Supplied] = @Exsil_Supplied, [Consignment] = @Consignment, [CrossFabShip] = @CrossFabShip, [RFID_Enable] = @RFID_Enable WHERE [MainID] = @MainID">
                <DeleteParameters>
                    <asp:Parameter Name="MainID" Type="String" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="CustomerID" Type="String" />
                    <asp:Parameter Name="column1" Type="Boolean" />
                    <asp:Parameter Name="Diameter" Type="Int16" />
                    <asp:Parameter Name="WAFERS_PER_CASS" Type="Int32" />
                    <asp:Parameter Name="Minimum_Per_Cass" Type="Int32" />
                    <asp:Parameter Name="Cassette" Type="Byte" />
                    <asp:Parameter Name="PO_On_Label" Type="Boolean" />
                    <asp:Parameter Name="RFID_Enable" Type="Boolean" />
                    <asp:Parameter Name="EffectiveDtd" Type="DateTime" />
                    <asp:Parameter Name="PackingSlip_Note" Type="String" />
                    <asp:Parameter Name="Operator" Type="String" />
                    <asp:Parameter Name="EventTime" Type="DateTime" />
                    <asp:Parameter Name="Exsil_Supplied" Type="Boolean" />
                    <asp:Parameter Name="Consignment" Type="Boolean" />
                    <asp:Parameter Name="CrossFabShip" Type="Boolean" />
                    <asp:Parameter Name="MainID" Type="String" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="MainID" Type="String" />
                    <asp:Parameter Name="CustomerID" Type="String" />
                    <asp:Parameter Name="column1" Type="Boolean" />
                    <asp:Parameter Name="Diameter" Type="Int16" />
                    <asp:Parameter Name="WAFERS_PER_CASS" Type="Int32" />
                    <asp:Parameter Name="Minimum_Per_Cass" Type="Int32" />
                    <asp:Parameter Name="Cassette" Type="Byte" />
                    <asp:Parameter Name="PO_On_Label" Type="Boolean" />
                    <asp:Parameter Name="RFID_Enable" Type="Boolean" />
                    <asp:Parameter Name="EffectiveDtd" Type="DateTime" />
                    <asp:Parameter Name="PackingSlip_Note" Type="String" />
                    <asp:Parameter Name="Operator" Type="String" />
                    <asp:Parameter Name="EventTime" Type="DateTime" />
                    <asp:Parameter Name="Exsil_Supplied" Type="Boolean" />
                    <asp:Parameter Name="Consignment" Type="Boolean" />
                    <asp:Parameter Name="CrossFabShip" Type="Boolean" />
                </InsertParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceCustomerlist" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                SelectCommand="SELECT Customer_Name FROM dbo.Customer GROUP BY Customer_Name HAVING (NOT (Customer_Name IS NULL)) ORDER BY Customer_Name">
            </asp:SqlDataSource>        
            <asp:SqlDataSource ID="SqlDataSourceNewCustomer" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                DeleteCommand="DELETE FROM [Customer] WHERE [CustomerID] = @CustomerID" 
                InsertCommand="INSERT INTO [Customer] ([CustomerID], [Customer_Name], [Business_Name], [PackingSlip_Note], [Operator]) VALUES (@CustomerID, @Customer_Name, @Business_Name, @PackingSlip_Note, @Operator)" 
                SelectCommand="SELECT [CustomerID], [Customer_Name], [Business_Name], [PackingSlip_Note], [Operator] FROM [Customer]" 
                UpdateCommand="UPDATE [Customer] SET [Customer_Name] = @Customer_Name, [Business_Name] = @Business_Name, [PackingSlip_Note] = @PackingSlip_Note, [Operator] = @Operator WHERE [CustomerID] = @CustomerID">
                <DeleteParameters>
                    <asp:Parameter Name="CustomerID" Type="String" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="Customer_Name" Type="String" />
                    <asp:Parameter Name="Business_Name" Type="String" />
                    <asp:Parameter Name="PackingSlip_Note" Type="String" />
                    <asp:Parameter Name="Operator" Type="String" />
                    <asp:Parameter Name="CustomerID" Type="String" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="CustomerID" Type="String" />
                    <asp:Parameter Name="Customer_Name" Type="String" />
                    <asp:Parameter Name="Business_Name" Type="String" />
                    <asp:Parameter Name="PackingSlip_Note" Type="String" />
                    <asp:Parameter Name="Operator" Type="String" />
                </InsertParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceCustomerSelected" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                DeleteCommand="DELETE FROM [Customer] WHERE [CustomerID] = @CustomerID" 
                InsertCommand="INSERT INTO dbo.Customer(CustomerID, Customer_Name, Business_Name, MacolaID, Supplier_Number, Transit_Days, Note1, Note2, PackingSlip_Note, Operator, EventTime) VALUES (@CustomerID, @Customer_Name, @Business_Name, @MacolaID, @Supplier_Number, @Transit_Days, @Note1, @Note2, @PackingSlip_Note, @Operator, @EventTime)" 
                SelectCommand="SELECT CustomerID, Customer_Name, Business_Name, MacolaID, Supplier_Number, Transit_Days, Note1, Note2, PackingSlip_Note, Operator, EventTime FROM dbo.Customer WHERE (Customer_Name = N'l') ORDER BY CustomerID" 
                UpdateCommand="UPDATE [Customer] SET [Customer_Name] = @Customer_Name, [Business_Name] = @Business_Name, [MacolaID] = @MacolaID, [Supplier_Number] = @Supplier_Number, [Transit_Days] = @Transit_Days, [Note1] = @Note1, [Note2] = @Note2, [PackingSlip_Note] = @PackingSlip_Note, [Operator] = @Operator, [EventTime] = @EventTime WHERE [CustomerID] = @CustomerID">
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
                    <asp:Parameter Name="PackingSlip_Note" Type="String" />
                    <asp:Parameter Name="Operator" Type="String" />
                    <asp:Parameter Name="EventTime" Type="DateTime" />
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
                    <asp:Parameter Name="PackingSlip_Note" Type="String" />
                    <asp:Parameter Name="Operator" Type="String" />
                    <asp:Parameter Name="EventTime" Type="DateTime" />
                </InsertParameters>
            </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceCurrentDefects" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        InsertCommand="INSERT INTO [T_ID_Defects] ([Defect], [Type], [Group]) VALUES (@ID, @Defect, @Type, @Group)"
        
                SelectCommand="SELECT [Key], Defect, Type, [Group] FROM dbo.T_ID_Defects WHERE (ID = '')" 
                ConflictDetection="CompareAllValues" 
                DeleteCommand="DELETE FROM [T_ID_Defects] WHERE [Key] = @original_Key AND [Defect] = @original_Defect AND [Type] = @original_Type AND [Group] = @original_Group" 
                OldValuesParameterFormatString="original_{0}" 
                UpdateCommand="UPDATE [T_ID_Defects] SET [Defect] = @Defect, [Type] = @Type, [Group] = @Group WHERE [Key] = @original_Key AND [Defect] = @original_Defect AND [Type] = @original_Type AND [Group] = @original_Group">
        <InsertParameters>
            <asp:Parameter Name="Defect" Type="String" />
            <asp:Parameter Name="Type" Type="String" />
            <asp:Parameter Name="Group" Type="String" />
        </InsertParameters>
        <DeleteParameters>
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_Defect" Type="String" />
            <asp:Parameter Name="original_Type" Type="String" />
            <asp:Parameter Name="original_Group" Type="String" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Defect" Type="String" />
            <asp:Parameter Name="Type" Type="String" />
            <asp:Parameter Name="Group" Type="String" />
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_Defect" Type="String" />
            <asp:Parameter Name="original_Type" Type="String" />
            <asp:Parameter Name="original_Group" Type="String" />
        </UpdateParameters>
    </asp:SqlDataSource>
        </asp:Panel>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

