<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SpecialCaseTool.aspx.vb" Inherits="DBMaintenance_SpecialCaseTool" title="SATI.Net Special Case Tool" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <br />
    IBM wafers from email subject : TDH<br />
    <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" Width="1024px">
                    <br />
                    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                        BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                        CellPadding="3" DataSourceID="SqlDataSource1" ForeColor="Black" GridLines="Vertical">
                        <FooterStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:BoundField DataField="WAT_Key" HeaderText="WAT_Key" InsertVisible="False" SortExpression="WAT_Key" />
                            <asp:BoundField DataField="T7" HeaderText="T7" SortExpression="T7" />
                            <asp:BoundField DataField="Active" HeaderText="Active" SortExpression="Active" />
                            <asp:BoundField DataField="PreThick" HeaderText="PreThick" SortExpression="PreThick" />
                            <asp:BoundField DataField="PostThick" HeaderText="PostThick" SortExpression="PostThick" />
                            <asp:BoundField DataField="Removal" HeaderText="Removal" SortExpression="Removal" />
                            <asp:BoundField DataField="0.065" HeaderText="0.065" SortExpression="0.065" />
                            <asp:BoundField DataField="0.12" HeaderText="0.12" SortExpression="0.12" />
                            <asp:BoundField DataField="Lot" HeaderText="Lot" SortExpression="Lot" />
                            <asp:BoundField DataField="BoxInvNumber" HeaderText="BoxInvNumber" InsertVisible="False"
                                SortExpression="BoxInvNumber" />
                            <asp:BoundField DataField="Slot" HeaderText="Slot" SortExpression="Slot" />
                        </Columns>
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                        SelectCommand="SELECT TOP 100 PERCENT dbo.T7_WaferActionTracking.WAT_Key, dbo.T7_WaferActionTracking.T7, dbo.T7_WaferActionTracking.Active, T7_GeoData_1.CenterThick AS PreThick, dbo.T7_GeoData.CenterThick AS PostThick, T7_GeoData_1.CenterThick - dbo.T7_GeoData.CenterThick AS Removal, dbo.T7_ParticalData.SP1BinCnt3 AS [0.065], dbo.T7_ParticalData.SP1BinCnt6 AS [0.12], dbo.LabelsMade.Lot, dbo.T7_InstanceInfo.InstanceID, dbo.T_FGI_Boxes.BoxInvNumber, dbo.T7_InstanceInfo.Slot, dbo.T_FGI_Boxes.CartonNumber FROM dbo.T7_InstanceInfo INNER JOIN dbo.T7_WaferActionTracking INNER JOIN dbo.T7_ParticalData ON dbo.T7_WaferActionTracking.Partical_Key = dbo.T7_ParticalData.Partical_Key INNER JOIN dbo.T7_GeoData AS T7_GeoData_1 ON dbo.T7_WaferActionTracking.PreGeo_Key = T7_GeoData_1.Geo_Key INNER JOIN dbo.T7_GeoData ON dbo.T7_WaferActionTracking.PostGeo_Key = dbo.T7_GeoData.Geo_Key ON dbo.T7_InstanceInfo.WAT_Key = dbo.T7_WaferActionTracking.WAT_Key INNER JOIN dbo.T_FGI_Boxes ON dbo.T7_InstanceInfo.InstanceID = dbo.T_FGI_Boxes.InstanceKey INNER JOIN dbo.LabelsMade ON dbo.T_FGI_Boxes.LabelsMadeKey = dbo.LabelsMade.LabelRecordNumber WHERE (NOT (dbo.T_FGI_Boxes.CartonNumber IS NULL)) GROUP BY dbo.T7_WaferActionTracking.WAT_Key, dbo.T7_WaferActionTracking.T7, dbo.T7_WaferActionTracking.Active, T7_GeoData_1.CenterThick, dbo.T7_GeoData.CenterThick, T7_GeoData_1.CenterThick - dbo.T7_GeoData.CenterThick, dbo.T7_ParticalData.SP1BinCnt3, dbo.T7_ParticalData.SP1BinCnt6, dbo.LabelsMade.Lot, dbo.T7_InstanceInfo.InstanceID, dbo.T7_InstanceInfo.Slot, dbo.T_FGI_Boxes.CartonNumber, dbo.T_FGI_Boxes.BoxInvNumber HAVING (dbo.T7_WaferActionTracking.T7 = N'46700I45SE') OR (dbo.T7_WaferActionTracking.T7 = N'46DHF079SJ') OR (dbo.T7_WaferActionTracking.T7 = N'46DDZ036SJ') OR (dbo.T7_WaferActionTracking.T7 = N'46K2I9APKO') OR (dbo.T7_WaferActionTracking.T7 = N'4601D13SEH') OR (dbo.T7_WaferActionTracking.T7 = N'46CLD061SJ') OR (dbo.T7_WaferActionTracking.T7 = N'46K2IL6YKO') OR (dbo.T7_WaferActionTracking.T7 = N'BBOCR169SJ') OR (dbo.T7_WaferActionTracking.T7 = N'462IL6YKOF') OR (dbo.T7_WaferActionTracking.T7 = N'46CTD010SJ') OR (dbo.T7_WaferActionTracking.T7 = N'46DGY078SJ') OR (dbo.T7_WaferActionTracking.T7 = N'46K2IID5KO') OR (dbo.T7_WaferActionTracking.T7 = N'46K2IEV1KO') OR (dbo.T7_WaferActionTracking.T7 = N'46CXI039SJ') OR (dbo.T7_WaferActionTracking.T7 = N'46K2I4WZKO') OR (dbo.T7_WaferActionTracking.T7 = N'46DEU007SJ') OR (dbo.T7_WaferActionTracking.T7 = N'46701D13SE') OR (dbo.T7_WaferActionTracking.T7 = N'46DGH132SJ') OR (dbo.T7_WaferActionTracking.T7 = N'46BLE181SJ') OR (dbo.T7_WaferActionTracking.T7 = N'46700I45SE') OR (dbo.T7_WaferActionTracking.T7 = N'46K1IZMSKO') OR (dbo.T7_WaferActionTracking.T7 = N'46DHF079SJ') OR (dbo.T7_WaferActionTracking.T7 = N'46DGE097SJ') OR (dbo.T7_WaferActionTracking.T7 = N'46K2IEUWKO') OR (dbo.T7_WaferActionTracking.T7 = N'46CLD061SJ') OR (dbo.T7_WaferActionTracking.T7 = N'BBOCR169SJ') OR (dbo.T7_WaferActionTracking.T7 = N'46CTD010SJ') OR (dbo.T7_WaferActionTracking.T7 = N'46DDZ036SJ') OR (dbo.T7_WaferActionTracking.T7 = N'46DGY078SJ') OR (dbo.T7_WaferActionTracking.T7 = N'46CXI039SJ') OR (dbo.T7_WaferActionTracking.T7 = N'46EU007SJC') OR (dbo.T7_WaferActionTracking.T7 = N'46K2IID5KO') OR (dbo.T7_WaferActionTracking.T7 = N'46K2IEV1KO') OR (dbo.T7_WaferActionTracking.T7 = N'46DGH132SJ') OR (dbo.T7_WaferActionTracking.T7 = N'46K219APKO') OR (dbo.T7_WaferActionTracking.T7 = N'46K2I4WZKO') OR (dbo.T7_WaferActionTracking.T7 = N'46K1IZMSKO') OR (dbo.T7_WaferActionTracking.T7 = N'46DGE097SJ') OR (dbo.T7_WaferActionTracking.T7 = N'46K2IEUWKO') OR (dbo.T7_WaferActionTracking.T7 = N'46BLE181SJ') ORDER BY dbo.T7_InstanceInfo.Slot, dbo.T7_WaferActionTracking.T7">
                    </asp:SqlDataSource>
                    <br />
                    <br />
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>

