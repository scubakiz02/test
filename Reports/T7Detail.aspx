<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="T7Detail.aspx.vb" Inherits="Reports_T7Detail" title="Untitled Page" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <br />
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
         <asp:Button ID="Button2" runat="server" Text="Button"  style="display:none"  />
            <cc1:ModalPopupExtender ID="Button2_ModalPopupExtender" runat="server" 
                BackgroundCssClass="modalBackground" DropShadow="True" DynamicServicePath="" 
                Enabled="True" PopupControlID="MapPanel" TargetControlID="Button2" 
                OkControlID="MapCloseButton">
            </cc1:ModalPopupExtender>
            
            <asp:Button ID="Button3" runat="server" Text="Button" style="display:none"/>
            <cc1:ModalPopupExtender ID="Button3_ModalPopupExtender" runat="server" 
                BackgroundCssClass="modalBackground" DropShadow="True" DynamicServicePath="" 
                Enabled="True" TargetControlID="Button3" PopupControlID="PanelNoMap">
            </cc1:ModalPopupExtender>
            
            <asp:Panel ID="Panel1" runat="server" Width="752px">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="T7 Detail Report For "></asp:Label>
                            &nbsp;
                <asp:TextBox ID="T7TextBox" runat="server" AutoPostBack="True" BorderStyle="None"
                    Width="176px" OnTextChanged="T7TextBox_TextChanged" BackColor="#CCFFFF"></asp:TextBox>
                <asp:Label ID="InfoLabel" runat="server" Text="No Record Found"></asp:Label>
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Receive Date:
                <asp:Label ID="RecDateLabel" runat="server" Text="Label" Width="144px"></asp:Label>&nbsp;<br />
                &nbsp;Note: T7 needs to be part of a shipment<asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>
                        &nbsp;<img src="../Color/Animated_LoadingBigger.gif" />Loading...
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <table style="width: 736px">
                    <tr>
                        <td colspan="6" style="font-weight: bold; background-color: darkgray">
                            Geo Data</td>
                    </tr>
                    <tr>
                        <td style="width: 100px; text-decoration: underline">
                        </td>
                        <td style="width: 100px; text-decoration: underline">
                            Date</td>
                        <td style="width: 100px; text-decoration: underline">
                            Tool</td>
                        <td style="width: 100px; text-decoration: underline">
                            Thick</td>
                        <td style="width: 100px; text-decoration: underline">
                            Res</td>
                        <td style="width: 100px; text-decoration: underline">
                            Type</td>
                    </tr>
                    <tr>
                        <td style="width: 100px; text-decoration: underline">
                            Pre</td>
                        <td style="width: 100px">
                                <asp:Label ID="PreGeoDateLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="PreGeoToolLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="PreGeoThickLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                                <asp:Label ID="PreGeoResLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="PreGeoTypeLabel" runat="server" Text="Label"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 100px; text-decoration: underline">
                                Post</td>
                        <td style="width: 100px">
                                <asp:Label ID="PostGeoDateLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="PostGeoToolLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                                <asp:Label ID="PostGeoThickLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                                <asp:Label ID="PostGeoResLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="PostGeoTypeLabel" runat="server" Text="Label"></asp:Label></td>
                    </tr>
                </table>
                <br />
                <br />
                <table style="width: 736px">
                    <tr>
                        <td colspan="6" style="font-weight: bold; background-color: darkgray">
                Surfscan Info</td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            <asp:Button ID="MapButton" runat="server" Height="19px" Text="Map" />
                        </td>
                        <td style="width: 100px; text-decoration: underline">
                            Date</td>
                        <td style="width: 100px; text-decoration: underline">
                            Tool</td>
                        <td style="width: 100px; text-decoration: underline">
                            Scrach Count</td>
                        <td style="width: 100px; text-decoration: underline;">
                        </td>
                        <td style="width: 100px; text-decoration: underline;">
                            CMP</td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                        </td>
                        <td style="width: 100px">
                <asp:Label ID="SurfDateLabel" runat="server" Text="Label" Width="88px"></asp:Label></td>
                        <td style="width: 100px">
                <asp:Label ID="SurfToolLabel" runat="server" Text="Label" Width="88px"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="SurfScrachLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                        </td>
                        <td style="width: 100px">
                            <asp:Label ID="CMPLabel" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px; height: 12px">
                        </td>
                        <td style="width: 100px; height: 12px">
                        </td>
                        <td style="width: 100px; height: 12px">
                        </td>
                        <td style="width: 100px; height: 12px">
                        </td>
                        <td style="width: 100px; height: 12px">
                        </td>
                        <td style="width: 100px; height: 12px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px; text-decoration: underline">
                            Size</td>
                        <td style="width: 100px">
                            <asp:Label ID="Surf1SizeLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="Surf2SizeLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="Surf3SizeLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="Surf4SizeLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px; height: 21px; text-decoration: underline">
                            LPD</td>
                        <td style="width: 100px; height: 21px">
                            <asp:Label ID="Surf1LPDLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px; height: 21px">
                            <asp:Label ID="Surf2LPDLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px; height: 21px">
                            <asp:Label ID="Surf3LPDLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px; height: 21px">
                            <asp:Label ID="Surf4LPDLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px; height: 21px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px; text-decoration: underline">
                            LPDN</td>
                        <td style="width: 100px">
                            <asp:Label ID="Surf1LPDNLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="Surf2LPDNLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="Surf3LPDNLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="Surf4LPDNLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px; text-decoration: underline">
                            SOD</td>
                        <td style="width: 100px">
                            <asp:Label ID="Surf1SODLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="Surf2SODLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="Surf3SODLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="Surf4SODLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table style="width: 736px">
                    <tr>
                        <td colspan="6" style="font-weight: bold; background-color: darkgray">
                            Ship Info</td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                        </td>
                        <td style="width: 100px; text-decoration: underline">
                            Date</td>
                        <td style="width: 100px; text-decoration: underline">
                            Ship Number</td>
                        <td style="width: 100px; text-decoration: underline">
                            Carton</td>
                        <td style="width: 100px; text-decoration: underline">
                            Wafer Box</td>
                        <td style="width: 100px; text-decoration: underline">
                            Slot #</td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                        </td>
                        <td style="width: 100px">
                            <asp:Label ID="ShipDateLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="ShipNumberLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="ShipCartonLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="ShipWaferBoxLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="ShipSlotLabel" runat="server" Text="Label"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 100px; height: 10px">
                        </td>
                        <td style="width: 100px; height: 10px">
                        </td>
                        <td style="width: 100px; height: 10px">
                        </td>
                        <td style="width: 100px; height: 10px">
                        </td>
                        <td style="width: 100px; height: 10px">
                        </td>
                        <td style="width: 100px; height: 10px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px; height: 17px">
                        </td>
                        <td style="width: 100px; height: 17px">
                        </td>
                        <td style="width: 100px; height: 17px">
                        </td>
                        <td style="width: 100px; height: 17px">
                        </td>
                        <td style="width: 100px; height: 17px">
                        </td>
                        <td style="width: 100px; height: 17px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                        </td>
                        <td style="width: 100px; text-decoration: underline">
                            Spec</td>
                        <td style="width: 100px; text-decoration: underline">
                            Rev</td>
                        <td style="width: 100px; text-decoration: underline">
                            Part#</td>
                        <td colspan="2" style="text-decoration: underline">
                            PWI Lot#</td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                        </td>
                        <td style="width: 100px">
                            <asp:Label ID="ShipSpecLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="ShipSpecRevLabel" runat="server" Text="Label"></asp:Label></td>
                        <td style="width: 100px">
                            <asp:Label ID="ShipPartLabel" runat="server" Text="Label"></asp:Label></td>
                        <td colspan="2">
                            <asp:Label ID="ShipLotLabel" runat="server" Text="Label"></asp:Label></td>
                    </tr>
                </table>
                <br />
                <br />
                <br />
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT dbo.T7_WaferActionTracking.T7, dbo.T_WH_Invintory.EventTime AS RecDate, dbo.T7_WaferActionTracking.StartDate AS ProdStart, dbo.T7_GeoData.RecordDate AS PreGeoDate, dbo.T7_GeoData.Tool AS PreGeoTool, dbo.T7_GeoData.CenterThick AS PreGeoThick, dbo.T7_GeoData.CenterRes AS PreGeoRes, dbo.T7_GeoData.Type AS PreGeoType, T7_GeoData_1.RecordDate AS PostGeoDate, T7_GeoData_1.Tool AS PostGeoTool, T7_GeoData_1.CenterThick AS PostGeoThick, T7_GeoData_1.CenterRes AS PostGeoRes, T7_GeoData_1.Type AS PostGeoType, dbo.T7_ParticalData.RecordDate AS SurfDate, dbo.T7_ParticalData.Tool AS SurfTool, dbo.T7_ParticalData.ID AS SurfID, dbo.T7_ParticalData.Run AS SurfRun, dbo.T7_ParticalData.WL AS SurfWL, dbo.T7_ParticalData.SP1BinCnt1, dbo.T7_ParticalData.SP1BinCnt2, dbo.T7_ParticalData.SP1BinCnt3, dbo.T7_ParticalData.SP1BinCnt4, dbo.T7_ParticalData.SP1BinCnt5, dbo.T7_ParticalData.SP1BinCnt6, dbo.T7_ParticalData.SP1BinCnt7, dbo.T7_ParticalData.SP1BinCnt8, dbo.T7_ParticalData.SP1LPDNBinCntInSize1, dbo.T7_ParticalData.SP1LPDNBinCntInSize2, dbo.T7_ParticalData.SP1LPDNBinCntInSize3, dbo.T7_ParticalData.SP1LPDNBinCntInSize4, dbo.T7_ParticalData.SP1LPDNBinCntInSize5, dbo.T7_ParticalData.SP1LPDNBinCntInSize6, dbo.T7_ParticalData.SP1LPDNBinCntInSize7, dbo.T7_ParticalData.SP1LPDNBinCntInSize8, dbo.T7_ParticalData.SP1SOD1, dbo.T7_ParticalData.SP1SOD2, dbo.T7_ParticalData.SP1SOD3, dbo.T7_ParticalData.SP1SOD4, dbo.T7_ParticalData.SP1SOD5, dbo.T7_ParticalData.SP1SOD6, dbo.T7_ParticalData.SP1SOD7, dbo.T7_ParticalData.SP1SOD8, dbo.T7_ParticalData.ScratchCnt, dbo.CofA_Info.LPD_G1, dbo.CofA_Info.First_Bin, dbo.CofA_Info.LPD_G2, dbo.CofA_Info.Second_Bin, dbo.CofA_Info.LPD_G3, dbo.CofA_Info.Third_Bin, dbo.CofA_Info.LPD_G4, dbo.CofA_Info.Forth_Bin, dbo.T7_ParticalData.AreaCnt, dbo.T_FGI_Boxes.BoxInvNumber AS WaferBox, dbo.T7_InstanceInfo.Slot, dbo.T_FGI_Boxes.CartonNumber, dbo.ShippingInventory.PickTicket AS ShipmentNumber, dbo.ShippingInventory.Confirmed AS ShipDate, dbo.LabelsMade.Lot, dbo.MainIDSpec.PART_NUMBER, dbo.MainIDSpec.SPEC_NUMBER, dbo.MainIDSpec.SPEC_REV_NUMBER FROM dbo.T_FGI_Boxes INNER JOIN dbo.T7_InstanceInfo ON dbo.T_FGI_Boxes.InstanceKey = dbo.T7_InstanceInfo.InstanceID INNER JOIN dbo.ShippingInventory ON dbo.T_FGI_Boxes.CartonNumber = dbo.ShippingInventory.Carton_Key INNER JOIN dbo.LabelsMade ON dbo.T_FGI_Boxes.LabelsMadeKey = dbo.LabelsMade.LabelRecordNumber INNER JOIN dbo.MainIDSpec ON dbo.LabelsMade.RecordNumber = dbo.MainIDSpec.RecordNumber RIGHT OUTER JOIN dbo.T7_WaferActionTracking INNER JOIN dbo.T_WH_Invintory ON dbo.T7_WaferActionTracking.WL = dbo.T_WH_Invintory.Waferlog INNER JOIN dbo.T7_GeoData ON dbo.T7_WaferActionTracking.PreGeo_Key = dbo.T7_GeoData.Geo_Key INNER JOIN dbo.T7_GeoData AS T7_GeoData_1 ON dbo.T7_WaferActionTracking.PostGeo_Key = T7_GeoData_1.Geo_Key INNER JOIN dbo.T7_ParticalData ON dbo.T7_WaferActionTracking.Partical_Key = dbo.T7_ParticalData.Partical_Key INNER JOIN dbo.CofA_Info ON dbo.T7_ParticalData.ID = dbo.CofA_Info.ID_NUMBER ON dbo.T7_InstanceInfo.WAT_Key = dbo.T7_WaferActionTracking.WAT_Key WHERE (dbo.T7_WaferActionTracking.T7 = N'K9AXZ090TM') AND (dbo.T7_WaferActionTracking.Active = N'Yes') AND (dbo.T_WH_Invintory.Action = N'StartWL')">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT dbo.T_FGI_Boxes.BoxInvNumber, dbo.T7_InstanceInfo.Slot, dbo.T_FGI_Cartons.CartonNumber, dbo.T7_InstanceInfo.WAT_Key, dbo.T_FGI_Boxes.LabelsMadeKey, dbo.T_FGI_Cartons.ShipmentNumber FROM dbo.T_FGI_Cartons INNER JOIN dbo.T_FGI_Boxes ON dbo.T_FGI_Cartons.CartonNumber = dbo.T_FGI_Boxes.CartonNumber INNER JOIN dbo.T7_InstanceInfo ON dbo.T_FGI_Boxes.InstanceKey = dbo.T7_InstanceInfo.InstanceID">
                </asp:SqlDataSource>
            </asp:Panel>
            <asp:Panel ID="PanelNoMap" runat="server" BackColor="#FF0066" Height="96px" 
                Width="104px">
                <div style="text-align: center">
                    <asp:Button ID="ButtonClose" runat="server" Text="Close" />
                    <br />
                    <br />
                    No Map<br />
                </div>
            </asp:Panel>
            <br />
            &nbsp;<asp:Panel ID="MapPanel" runat="server" BackColor="#E0E0E0" Width="1050px">
                <asp:Button ID="MapCloseButton" runat="server" OnClick="MapCloseButton_Click" 
                    Text="Close" />
                <br />
                <asp:Image ID="MapImage" runat="server" Width="1050px" />
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

