<%@ Page Title="Ship Wafers" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="Ship.aspx.vb" Inherits="PC_Ship" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server">
                
                <asp:Label ID="Label2" runat="server" Text="Make SRN (Shipping Record Number)" Font-Size="Larger"></asp:Label>
                
                &nbsp;<table class="style1">
                    <tr>
                        <td style="vertical-align: top; text-align: right">
                            
                            Scan Box >>><asp:TextBox ID="TextBoxScanIn" runat="server"  BackColor="#99CCFF" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" AutoPostBack="True"></asp:TextBox>
                             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="ButtonMakeSRN" runat="server" Text="Make SRN" Font-Size="X-Small" BackColor="#CCCCCC" />
                            
                        </td>
                        <td align="right">
                            <asp:Button ID="ButtonLoadSRN_Page" runat="server" Text="View SRN Page" Font-Size="X-Small" BackColor="#CCCCCC" Visible="False" />

                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            
                            <asp:Button ID="ButtonPrint" runat="server" Text="Print SRN Label" Font-Size="X-Small" BackColor="#CCCCCC" Visible="False" />
                            
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

                            SRN&nbsp;<asp:Label ID="LabelSRN" runat="server" Text="0"></asp:Label>
                        </td>
                    </tr>                    

                    <tr>
                        <td style="vertical-align: top; text-align: left">
                            Product Information&nbsp;
                        </td>
                        <td style="vertical-align: top">                                
                            <table class="style1">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Inventory in Shipment"></asp:Label>
                                    </td>
                                    <td  align="right">
                                        Scan Count:&nbsp;<asp:Label ID="LabelScanCount" runat="server" Text="0"></asp:Label> &nbsp;&nbsp;&nbsp;
                                        Wafer Qty:&nbsp;<asp:Label ID="LabelWaferQty" runat="server" Text="0"></asp:Label>&nbsp;&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    
                    <tr>
                        <td style="vertical-align: top; text-align: left">
                            <asp:Panel ID="PanelInfoProduct" runat="server" BorderColor="#0066CC" BorderStyle="Solid" BorderWidth="1px">
                                
                                <table class="style1">
                                    <tr>
                                        <td>
                                            ID#:&nbsp;
                                            <asp:Label ID="LabelID" runat="server" Text="XXXX"></asp:Label>,&nbsp;
                                            <asp:Label ID="LabelCustomerID" runat="server" Text="Customer-FAB"></asp:Label>
                                        </td>
                                        <td>
                                            Diameter:&nbsp;
                                            <asp:Label ID="LabelDiameter" runat="server" Text="000"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            SO#:&nbsp;
                                            <asp:Label ID="LabelSO" runat="server" Text="XXXX"></asp:Label>
                                        </td>
                                        <td>
                                             Bulk Shipment:&nbsp;
                                            <asp:Label ID="LabelBulk" runat="server" Text="Yes/No"></asp:Label>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td>
                                            PO#:&nbsp;
                                            <asp:Label ID="LabelPO" runat="server" Text="XXXXXXXXXX-XX"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td>
                                            PO on Label:&nbsp;
                                            <asp:Label ID="LabelPOonLabel" runat="server" Text="Yes/No"></asp:Label>
                                        </td>
                                        <td>
                                            Cross Fab Shipping:&nbsp;
                                            <asp:Label ID="LabelCrossFab" runat="server" Text="Yes/No"></asp:Label>
                                        </td>
                                    </tr>
                                    
                                    

                                    <tr>
                                        <td>
                                            Shipping:&nbsp;
                                            <asp:Label ID="LabelShippingTemplate" runat="server" Text="TemplateXXXXXXX"></asp:Label>
                                        </td>
                                        <td>
                                            CofA:&nbsp;
                                            <asp:Label ID="LabelCofATemplate" runat="server" Text="TemplateXXXXXXXXXXXX"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            Part#:&nbsp;
                                            <asp:Label ID="LabelPart" runat="server" Text="XXXXXXXX"></asp:Label>
                                        </td>
                                        <td>
                                            Part Rev#:&nbsp;
                                            <asp:Label ID="LabelPartRev" runat="server" Text="XX"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            Spec#:&nbsp;
                                            <asp:Label ID="LabelSpec" runat="server" Text="XXXXXXXXX"></asp:Label>
                                        </td>
                                        <td>
                                            Spec Rev#:&nbsp;
                                            <asp:Label ID="LabelSpecRev" runat="server" Text="XXXX"></asp:Label>
                                        </td>
                                    </tr>
                                                                        
                                    <tr>
                                        <td>
                                            Shipment Number:&nbsp;<asp:Label ID="LabelShipNum" runat="server" Text="XXXX-XXX"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                                                       
                                </table>
                                
                            </asp:Panel>

                             SO Info:<br />
                            <asp:Panel ID="PanelSOinfo" runat="server" BorderColor="#0066CC" BorderStyle="Solid" BorderWidth="1px">
                               

                            </asp:Panel>
                        </td>
                        <td style="vertical-align: top">
                            <asp:TextBox ID="TextBoxScanGood" runat="server" BorderColor="#0066CC" BorderStyle="Solid" TextMode="MultiLine"  width="100%" BorderWidth="1px" Wrap="False" Rows="0"></asp:TextBox>
                            
                            Scan Info:&nbsp;<br />
                            <asp:TextBox ID="TextBoxScanInfo" runat="server" BorderColor="#0066CC" BorderStyle="Solid" TextMode="MultiLine"  width="100%" BorderWidth="1px" Wrap="False" Rows="0"></asp:TextBox>
                            
                        </td>
                    </tr>
                    
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>

                </table>
                <br />
                 <asp:Button ID="ButtonShow" runat="server" Text="Show" style="display:none" />
                    
                    <asp:Panel ID="PanelMakeSRN" runat="server" Width="500" BackColor="#00cc66" BorderColor="Black" BorderStyle="Solid"  HorizontalAlign="Center" >
                       <br />
                          Pick Ticket Infomation.
                            <table style="border-style: solid; border-width: thin">
                                <tr>
                                    <td>Carrier</td>
                                    <td>Freight</td>
                                    <td>Freight Account</td>                                    
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="CarrierDropDownList" runat="server" AppendDataBoundItems="True" DataSourceID="CarrierSqlDataSource" DataTextField="Name" DataValueField="Name" AutoPostBack="True">
                                            <asp:ListItem Selected="True">Select...</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="FreightDropDownList" runat="server" AutoPostBack="True">
                                            <asp:ListItem Selected="True">Select...</asp:ListItem>
                                            <asp:ListItem>PPD</asp:ListItem>
                                            <asp:ListItem>PP&amp;C</asp:ListItem>
                                            <asp:ListItem>Coll</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="FreightAccountDropDownList" runat="server" AutoPostBack="True">
                                            <asp:ListItem Selected="True">Select...</asp:ListItem>
                                            <asp:ListItem>Our Account</asp:ListItem>
                                            <asp:ListItem>Customer Account</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>                                    
                                </tr>
                            </table>
                           
                        
                        <br />                        
                        <asp:Button ID="ButtonMakeShipmentSRN" runat="server" Text="Make Shippment" Visible="False"  />&nbsp;&nbsp;
                        <asp:Button ID="ButtonCloseSRN" runat="server" Text="Cancel2" />
                        <br />
                        <br />
                    </asp:Panel>

                    <cc1:ModalPopupExtender 
                        ID="PanelMakeSRN_ModalPopupExtender" 
                        runat="server" 
                        BehaviorID="PanelMakeSRN_ModalPopupExtender" 
                        DynamicServicePath="" 
                        TargetControlID="ButtonShow"
                        PopupControlID="PanelMakeSRN"
                        OkControlID="ButtonCloseSRN">
                    </cc1:ModalPopupExtender>

                 <asp:SqlDataSource ID="CarrierSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT Name FROM dbo.Carriers GROUP BY Name ORDER BY Name">
                 </asp:SqlDataSource>

            </asp:Panel>
        </ContentTemplate>        
    </asp:UpdatePanel>
    
     
</asp:Content>

