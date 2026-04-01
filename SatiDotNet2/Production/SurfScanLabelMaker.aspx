<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SurfScanLabelMaker.aspx.vb" Inherits="Production_SurfScanLabelMaker" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">     
       <ContentTemplate> 

       <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Make 300mm Labels"></asp:Label>
      <br />
    
        <table style="width: 648px; vertical-align: top; text-align: center; border-right: black thin solid; border-top: black thin solid; border-left: black thin solid; border-bottom: black thin solid; height: 376px;">
            <tr> 
                <td style="width: 91px; height: 96px; vertical-align: top; text-align: left;">
                    <asp:Panel ID="Panel1" runat="server" Height="50px" Width="125px">
                        Select SPx<br />
                        <asp:ListBox ID="ToolListBox" runat="server" Height="72px" Width="120px" 
                            AutoPostBack="True" OnSelectedIndexChanged="ToolListBox_SelectedIndexChanged">
                            <asp:ListItem Value="SP1">SP1 (T2)</asp:ListItem>
                            <asp:ListItem Value="SP2-S0132">SP2 (T6)</asp:ListItem>
                            <asp:ListItem Value="SP3-2110224">SP3</asp:ListItem>
                        </asp:ListBox></asp:Panel>
                </td>
                <td style="width: 100px; height: 96px; vertical-align: top; text-align: left;">
                    <asp:Panel ID="Panel2" runat="server" Height="50px" Width="125px">
                        Select Station<br />
                        <asp:ListBox ID="StationListBox" runat="server" Height="56px" Width="120px" AutoPostBack="True" OnSelectedIndexChanged="StationListBox_SelectedIndexChanged">
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                        </asp:ListBox></asp:Panel>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 91px; vertical-align: top; text-align: left;">
                    <asp:Panel ID="Panel3" runat="server" Height="50px" Width="200px">
                        Enter Last M12 Laser Mark<br />
                        <asp:TextBox ID="M12LaserMarkTextBox" runat="server"></asp:TextBox><br />
                        <asp:Button ID="VerButton" runat="server" Text="Confirm" OnClick="VerButton_Click" Visible="False" /><br />
                    </asp:Panel>
                </td>
                <td style="width: 100px; vertical-align: top; text-align: left;">
                    Information:<br />
                    <asp:TextBox ID="InfoTextBox" runat="server"  TextMode="MultiLine" Height="114px" Width="199px"></asp:TextBox><br />
                </td>
            </tr>
            <tr>
                <td style="width: 91px; vertical-align: top; text-align: left;">
                    Select Printer<br />
    <asp:DropDownList id="PrinterDropDownList" runat="server"><asp:ListItem>Select Printer...</asp:ListItem>
        <asp:ListItem Selected="True">Zebra1</asp:ListItem>
        <asp:ListItem>Zebra2</asp:ListItem>
        <asp:ListItem>Zebra_2B</asp:ListItem>
        <asp:ListItem>Zebra9</asp:ListItem>
    </asp:DropDownList><br />
                    <asp:Button ID="Button1" runat="server" Text="Make Label" />
                </td>
                <td style="width: 100px; vertical-align: top; text-align: left;">
                    <asp:UpdateProgress id="UpdateProgress2" runat="server">
                        <ProgressTemplate>
                            &nbsp;<IMG src="../Color/Animated_LoadingBigger.gif" />Loading...
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
        </table>
                <asp:SqlDataSource 
                    ID="SP1SqlDataSource" 
                    runat="server" 
                    ConnectionString="<%$ ConnectionStrings:AutoDataConnectionString %>"
                    SelectCommand="SELECT TOP 25 Entry, Machine, CreationDate, SPSessionName, SPRecipeName, SessionDate, ID#, RUN#, Wafer_log, Comment1, Comment2, ChannelID, SourceSlotID, DispositionName, SumAllDefects, FailedLimit, AreaCnt, TotalArea, ScratchCnt, ScratchTotalLength, ScratchAveLength, ScratchMaxLength, ClusterAreaCnt, LPDECnt, LPDSCnt, PosCnt, NegCnt, WaferPosAvgDensity, WaferPosMean, WaferPosStdDev, WaferNegAvgDensity, WaferNegMean, WaferNegStdDev, BinCnt1, BinCnt2, BinCnt3, BinCnt4, BinCnt5, BinCnt6, BinCnt7, BinCnt8, BinCnt18, RangeMin, RangeMax, TotalNCDefectsCount, LPDNBinCntInSize1, LPDNBinCntInSize2, LPDNBinCntInSize3, LPDNBinCntInSize4, LPDNBinCntInSize5, LPDNBinCntInSize6, LPDNBinCntInSize7, LPDNBinCntInSize8, LPDNBinCntInSize18, SOD1, SOD2, SOD3, SOD4, SOD5, SOD6, SOD7, SOD8, SOD18, Average, Peak, Median, StdDeviation, Thruput, WaferDia, EdgeExclusion, DestinationStationID, DestinationSlotID, WaferIdLabel, Comment FROM dbo.SP1_Data WHERE (DestinationStationID = 2) AND (Machine = N'SP1') ORDER BY CreationDate DESC">
                </asp:SqlDataSource>
                <span style="font-size: 12pt; font-family: 'Times New Roman'; mso-fareast-font-family: 'Times New Roman'; mso-ansi-language: EN-US; mso-fareast-language: EN-US; mso-bidi-language: AR-SA">
                
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Production/SPxT7DupeCompatibilityCheck.aspx">Check For Dupe Compatibility</asp:HyperLink><br /><br />
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/PC/FGICartonDetail.aspx">Sati.Net "View Wafer Box or Carton Detail"</asp:HyperLink><br /><br />
                    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="http://PWI-40:82/AdvInstanceViewer.aspx">Sati.Net Pulse "Advanced Instance Viewer"</asp:HyperLink></span>
        
            <br /><br />
       </ContentTemplate>
    </asp:UpdatePanel>     
</asp:Content>


