<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MakeShipment.aspx.vb" Inherits="PC_MakeShipment" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server" >
    
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Make Shipment"></asp:Label><br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:Label ID="Step1Label" runat="server" Font-Bold="True" Text="Step 1" Width="56px"></asp:Label><br />
            <asp:Panel ID="Panel1" runat="server" BackColor="LightBlue" Width="600px" >
                <table class="MasterPagePanelSub">
                    <tr>
                        <td>Scan Pick Ticket&nbsp;<asp:TextBox ID="PickTicketTextBox" runat="server" AutoPostBack="True" OnTextChanged="PickTicketTextBox_TextChanged"></asp:TextBox></td>
                        <td>Enter Qty you are Shipping:&nbsp;<asp:TextBox ID="TextBoxShipQty" runat="server" Width="64px" AutoPostBack="True"></asp:TextBox></td>
                    </tr>
                </table>
                </asp:Panel><br /> 
            
            <asp:Label ID="Step2Label" runat="server" Font-Bold="True" Text="Step 2" Width="56px"></asp:Label><br />
            <asp:Panel ID="Panel2" runat="server" BackColor="LightBlue" BorderColor="White" Visible="False" Width="600px">                
                <table class="MasterPagePanelSub">
                    <tr>
                        <td colspan="3">Shipment Info &amp; System Check:</td>
                    </tr>
                    <tr>
                        <td>ID# :<asp:Label ID="IDLabel" runat="server" Font-Bold="True" Text="Label" Width="96px"></asp:Label></td>                        
                        <td colspan="2">Diameter:<asp:Label ID="DiameterLabel" runat="server" Font-Bold="True" Text="Label" Width="88px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>SO#:<asp:Label ID="SoLabel" runat="server" Font-Bold="True" Text="Label" Width="112px"></asp:Label></td>
                        <td colspan="2"></td>                        
                    </tr>
                    <tr>
                        <td>PO#:<asp:Label ID="POLabel" runat="server" Font-Bold="True" Text="Label" Width="192px"></asp:Label></td>
                        <td colspan="2">PO on Label:<asp:Label ID="POonLabelLabel" runat="server" Font-Bold="True" Text="Yes / No" Width="88px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="2">Shipping Template Name:<asp:Label ID="ShippingTemplateLabel" runat="server" Font-Bold="True" Text="Label" Width="184px"></asp:Label>&nbsp;</td>                        
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2">CofA Template Name:<asp:Label ID="CofATemplateLabel" runat="server" Font-Bold="True" Text="Label" Width="168px"></asp:Label>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td><asp:Label ID="LabelOrgID" runat="server" Text="orgID "></asp:Label><asp:Label ID="LabelOrgIDID" runat="server" Text="ID"></asp:Label></td>
                        <td><asp:Label ID="Label2" runat="server" Text="CrossFab: "></asp:Label> <asp:Label ID="LabelCrossFab" runat="server" Text="yn"></asp:Label> </td>
                        <td><asp:Label ID="LabelBulk" runat="server" Text="Bulk Shippment" ForeColor="Red" Visible="false"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="3"><asp:Label ID="Label3" runat="server" Text="Dupe Check History:  "></asp:Label><asp:Label ID="LabelDupeCheckEnabel" runat="server" Text="Enable / Disable"></asp:Label></td>
                    </tr>
                </table>                 
            </asp:Panel><br />
                        
            <asp:Label ID="Step3Label" runat="server" Font-Bold="True" Text="Step 3" Width="56px"></asp:Label><br />
            <asp:HyperLink ID="HyperLinkReport" runat="server" Visible="False" BackColor="#FF3535" Font-Size="X-Large" BorderColor="#FF3535" BorderWidth="50px">Low SO Balance, View Report</asp:HyperLink>
            <asp:Panel ID="Panel3" runat="server" BackColor="LightBlue" Visible="False" Width="600px">
                
                <table class="MasterPagePanelSub">
                    <tr>
                        <td>Build</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>Scan Cartons:<asp:TextBox  ID="CartonScanTextBox" runat="server" AutoPostBack="True" OnTextChanged="CartonScanTextBox_TextChanged"></asp:TextBox></td>
                        <td><asp:Label ID="ErrorInfoLabel" runat="server" BackColor="Red" Font-Bold="True" Text="Label" Visible="False"></asp:Label></td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>Carton Count:<asp:Label ID="CartonCountLabel" runat="server" Text="0" Width="64px"></asp:Label></td>
                        <td>Qty Added:<asp:Label ID="QtyAddedLabel" runat="server" Text="0" Width="64px"></asp:Label></td>
                        <td>Qty Left:<asp:Label ID="QtyLeftLabel" runat="server" Text="0" Width="64px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Part#:<asp:Label ID="PartNumberLabel" runat="server" Width="160px"></asp:Label></td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>Spec#:<asp:Label ID="SpecNumberLabel" runat="server" Width="192px"></asp:Label></td>
                        <td colspan="2">Spec Rev#:<asp:Label ID="SpecRevLabel" runat="server" Width="96px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="3"><br />Scan Info&nbsp; <asp:Label ID="WarnLabel" runat="server" BackColor="Yellow" Text="Warning Found!" Visible="False"></asp:Label></td>                        
                    </tr>
                    <tr>
                        <td colspan="3"><asp:TextBox ID="CartonsAddedTextBox" runat="server" TextMode="MultiLine" Width="100%" Height="152px"></asp:TextBox></td>                        
                    </tr>
                    <tr>
                        <td><br />Palet Count &nbsp;
                            <asp:DropDownList ID="PalletCountDropDownList" runat="server"
                                AutoPostBack="True"
                                OnSelectedIndexChanged="PalletCountDropDownList_SelectedIndexChanged"
                                Enabled="False">
                                <asp:ListItem Value="0">Select Pallet Count...</asp:ListItem>
                                <asp:ListItem Value="0">None</asp:ListItem>
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
                            </asp:DropDownList>
                        </td>
                        <td colspan="2" style="vertical-align: bottom; text-align: left"><asp:Button ID="GOStep4Button" runat="server" OnClick="GOStep4Button_Click" Text="Next" Visible="False" /></td>                        
                    </tr>                    
                </table>
            </asp:Panel>

            <br />
            <asp:Label ID="Step4Label" runat="server" Font-Bold="True" Text="Step 4" Width="56px"></asp:Label><br />
            <asp:Panel ID="Panel4" runat="server" BackColor="LightBlue" Visible="False" Width="600px">
                Select Carrier Information:<br />
                <br />
                Carrier
                <asp:DropDownList ID="CarrierDropDownList" runat="server" AppendDataBoundItems="True"
                    DataSourceID="CarrierSqlDataSource" DataTextField="Name" DataValueField="Name"
                    Width="352px">
                    <asp:ListItem Selected="True">Select...</asp:ListItem>
                </asp:DropDownList><asp:SqlDataSource ID="CarrierSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT Name FROM dbo.Carriers GROUP BY Name ORDER BY Name"></asp:SqlDataSource>
                <br />
                <br />
                Freight
                <asp:DropDownList ID="FreightDropDownList" runat="server" Width="88px">
                    <asp:ListItem Selected="True">Select...</asp:ListItem>
                    <asp:ListItem>PPD</asp:ListItem>
                    <asp:ListItem>PP&amp;C</asp:ListItem>
                    <asp:ListItem>Coll</asp:ListItem>
                </asp:DropDownList>&nbsp; Freight Account
                <asp:DropDownList ID="FreightAccountDropDownList" runat="server" Width="160px">
                    <asp:ListItem Selected="True">Select...</asp:ListItem>
                    <asp:ListItem>Our Account</asp:ListItem>
                    <asp:ListItem>Customer Account</asp:ListItem>
                </asp:DropDownList><br />
                <br />
                <br />
                <asp:CheckBox ID="MakeLabelsCheckBox" runat="server" Checked="True" Text="Make Labels" />&nbsp;<br />
                <asp:CheckBox ID="MakePackingSlipCheckBox" runat="server" Checked="True" Text="Make Packing Slip" /><br />
                <asp:CheckBox ID="MakeCofACheckBox" runat="server" Checked="True" Text="Make CofA" /><br />
                <asp:CheckBox ID="AddToPendingShipments" runat="server" Checked="True" Text="Add To Pending Shipments" /><br />
                <br />
                <asp:RadioButton ID="RadioButtonOffSite" runat="server" Checked="True" Text="Off Site - Printer 8&amp;5" AutoPostBack="True" GroupName="Printers" /><br />
                <asp:RadioButton ID="RadioButtonMainBuilding" runat="server" Checked="False" Text="Main Site - Printer 6&7" AutoPostBack="True" GroupName="Printers" /><br />
                <br />
                <asp:Button ID="Button1" runat="server" Text="Finish" />
                <br />
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <img src="../Color/Animated_LoadingBigger.gif" />Working...
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <br />
                <asp:Panel ID="FilePanel" runat="server" Height="50px" Visible="False" Width="504px">
                    CofA: &nbsp;<asp:HyperLink ID="CofAHyperLink" runat="server" Target="_blank">Open CofA</asp:HyperLink><br />
                    <br />
                    Packing Slip: &nbsp;<asp:HyperLink ID="PSHyperLink" runat="server" Target="_blank">Open Packing Slip</asp:HyperLink><br />
                </asp:Panel>
                <br />
            </asp:Panel>
            <br />
            <br />
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

