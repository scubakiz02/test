<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="FGICartonDetail.aspx.vb" Inherits="PC_FGICartonDetail" title="Carton Detail View" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            Scan Wafer Box or Carton
            <asp:TextBox ID="CartonTextBox" runat="server" OnTextChanged="CartonTextBox_TextChanged"></asp:TextBox>
            <asp:Label ID="InfoLabel" runat="server"></asp:Label><br />
            <asp:Panel ID="Panel1" runat="server" Height="24px" Width="648px">
                <asp:CheckBox ID="CheckBox3" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox3_CheckedChanged"
                    Text="ViewLot Number" />&nbsp; &nbsp;<asp:CheckBox ID="CheckBox4" runat="server"
                        AutoPostBack="True" OnCheckedChanged="CheckBox4_CheckedChanged" Text="View T7" />
                &nbsp;
                <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged"
                    Text="View Geo Data" />
                &nbsp;
                <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox2_CheckedChanged"
                    Text="View LPD Data" /></asp:Panel>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                DataSourceID="CartonDetailSqlDataSource" ForeColor="#333333" GridLines="None" AllowSorting="True"
                >
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="WaferBoxNumber" HeaderText="WB Num" InsertVisible="False"
                        SortExpression="WaferBoxNumber" />
                    <asp:BoundField DataField="Slot" HeaderText="Slot" SortExpression="Slot" />
                    <asp:BoundField DataField="Lot" HeaderText="Lot" SortExpression="Lot" />
                    <asp:BoundField DataField="T7" HeaderText="T7" SortExpression="T7" />
                    <asp:BoundField DataField="SpecThick" HeaderText="SpecThick" SortExpression="SpecThick" />
                    <asp:BoundField DataField="CenterThick" HeaderText="CenterThick" SortExpression="CenterThick" />
                    <asp:BoundField DataField="SpecRes" HeaderText="SpecRes" SortExpression="SpecRes" />
                    <asp:BoundField DataField="CenterRes" HeaderText="CenterRes" SortExpression="CenterRes" />
                    <asp:BoundField DataField="SpecType" HeaderText="SpecType" SortExpression="SpecType" />
                    <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                    <asp:BoundField DataField="SpecBow" HeaderText="SpecBow" SortExpression="SpecBow" />
                    <asp:BoundField DataField="Bow" HeaderText="Bow" SortExpression="Bow" />
                    <asp:BoundField DataField="SpecWarp" HeaderText="SpecWarp" SortExpression="SpecWarp" />
                    <asp:BoundField DataField="Warp" HeaderText="Warp" SortExpression="Warp" />
                    <asp:BoundField DataField="SpecTTV" HeaderText="SpecTTV" SortExpression="SpecTTV" />
                    <asp:BoundField DataField="TTV" HeaderText="TTV" SortExpression="TTV" />
                    <asp:BoundField DataField="First_Bin" HeaderText="First_Bin" SortExpression="First_Bin" />
                    <asp:BoundField DataField="First_BinSpec" HeaderText="First_BinSpec" SortExpression="First_BinSpec" />
                    <asp:BoundField DataField="Second_Bin" HeaderText="Second_Bin" SortExpression="Second_Bin" />
                    <asp:BoundField DataField="Second_BinSpec" HeaderText="Second_BinSpec" SortExpression="Second_BinSpec" />
                    <asp:BoundField DataField="Third_Bin" HeaderText="Third_Bin" SortExpression="Third_Bin" />
                    <asp:BoundField DataField="Third_BinBin" HeaderText="Third_BinBin" SortExpression="Third_BinBin" />
                    <asp:BoundField DataField="Forth_Bin" HeaderText="Forth_Bin" SortExpression="Forth_Bin" />
                    <asp:BoundField DataField="Forth_BinSpec" HeaderText="Forth_BinSpec" SortExpression="Forth_BinSpec" />
                    <asp:BoundField DataField="Bin1" HeaderText="Bin1" SortExpression="Bin1" />
                    <asp:BoundField DataField="Bin2" HeaderText="Bin2" SortExpression="Bin2" />
                    <asp:BoundField DataField="Bin3" HeaderText="Bin3" SortExpression="Bin3" />
                    <asp:BoundField DataField="Bin4" HeaderText="Bin4" SortExpression="Bin4" />
                    <asp:BoundField DataField="Bin5" HeaderText="Bin5" SortExpression="Bin5" />
                    <asp:BoundField DataField="Bin6" HeaderText="Bin6" SortExpression="Bin6" />
                    <asp:BoundField DataField="Bin7" HeaderText="Bin7" SortExpression="Bin7" />
                    <asp:BoundField DataField="Bin8" HeaderText="Bin8" SortExpression="Bin8" />
                </Columns>
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="LightBlue" />
            </asp:GridView>
            <asp:SqlDataSource ID="CartonDetailSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                SelectCommand="SELECT TOP 100 PERCENT dbo.T_FGI_Boxes.CartonNumber, dbo.T_FGI_Boxes.BoxInvNumber AS WaferBoxNumber, dbo.LabelsMade.Lot, dbo.T7_InstanceInfo.Slot, dbo.T7_WaferActionTracking.T7, dbo.MainIDSpec.thk_grp AS SpecThick, dbo.T7_GeoData.CenterThick, dbo.MainIDSpec.res_grp AS SpecRes, dbo.T7_GeoData.CenterRes, dbo.MainIDSpec.WTYPE_DOPE AS SpecType, dbo.T7_GeoData.Type, dbo.PROCESS_INFO.BOW AS SpecBow, dbo.T7_GeoData.Bow, dbo.PROCESS_INFO.WARP AS SpecWarp, dbo.T7_GeoData.TotWarp AS Warp, dbo.PROCESS_INFO.FINAL_TTV AS SpecTTV, dbo.T7_GeoData.TTV, dbo.CofA_Info.First_Bin, dbo.PROCESS_INFO.PARTICLE_SPEC_1 AS First_BinSpec, dbo.CofA_Info.Second_Bin, dbo.PROCESS_INFO.PARTICLE_SPEC_2 AS Second_BinSpec, dbo.CofA_Info.Third_Bin, dbo.PROCESS_INFO.PARTICLE_SPEC_3 AS Third_BinBin, dbo.CofA_Info.Forth_Bin, dbo.PROCESS_INFO.PARTICLE_SPEC_4 AS Forth_BinSpec, dbo.T7_ParticalData.SP1BinCnt1 AS Bin1, dbo.T7_ParticalData.SP1BinCnt2 AS Bin2, dbo.T7_ParticalData.SP1BinCnt3 AS Bin3, dbo.T7_ParticalData.SP1BinCnt4 AS Bin4, dbo.T7_ParticalData.SP1BinCnt5 AS Bin5, dbo.T7_ParticalData.SP1BinCnt6 AS Bin6, dbo.T7_ParticalData.SP1BinCnt7 AS Bin7, dbo.T7_ParticalData.SP1BinCnt8 AS Bin8 FROM dbo.T_FGI_Boxes LEFT OUTER JOIN dbo.T7_ParticalData INNER JOIN dbo.T7_GeoData INNER JOIN dbo.T7_WaferActionTracking INNER JOIN dbo.T7_InstanceInfo ON dbo.T7_WaferActionTracking.WAT_Key = dbo.T7_InstanceInfo.WAT_Key ON dbo.T7_GeoData.Geo_Key = dbo.T7_WaferActionTracking.PostGeo_Key ON dbo.T7_ParticalData.Partical_Key = dbo.T7_WaferActionTracking.Partical_Key ON dbo.T_FGI_Boxes.InstanceKey = dbo.T7_InstanceInfo.InstanceID LEFT OUTER JOIN dbo.MainID_MainIDSpec INNER JOIN dbo.LabelsMade INNER JOIN dbo.MainIDSpec ON dbo.LabelsMade.RecordNumber = dbo.MainIDSpec.RecordNumber ON dbo.MainID_MainIDSpec.WaferSpec_Key = dbo.MainIDSpec.RecordNumber INNER JOIN dbo.CofA_Info ON dbo.MainID_MainIDSpec.MainID = dbo.CofA_Info.ID_NUMBER INNER JOIN dbo.PROCESS_INFO ON dbo.MainID_MainIDSpec.MainID = dbo.PROCESS_INFO.ID_NUMBER ON dbo.T_FGI_Boxes.LabelsMadeKey = dbo.LabelsMade.LabelRecordNumber WHERE (dbo.T_FGI_Boxes.CartonNumber = 3) ORDER BY dbo.T7_InstanceInfo.Slot">
            </asp:SqlDataSource>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

