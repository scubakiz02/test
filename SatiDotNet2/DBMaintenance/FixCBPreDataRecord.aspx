<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="FixCBPreDataRecord.aspx.vb" Inherits="DBMaintenance_FixCBPreDataRecord" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:Panel ID="Panel3" runat="server" BackColor="LightBlue" Height="80px" Width="328px">
    This for 300mm Cardboard Boxes Only!!!!<br />
        Edits the Pre Geo data<br />
        If there is no data it copy the post data record and
        <br />
        mods the thick value.</asp:Panel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <img src="../Color/Animated_LoadingBigger.gif" />Working...
        </ProgressTemplate>
    </asp:UpdateProgress>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="912px">
                Carton to Work With
                <asp:TextBox ID="CartonTextBox" runat="server" Font-Bold="True" OnTextChanged="CartonTextBox_TextChanged"></asp:TextBox>&nbsp;<br />
                <asp:Panel ID="Panel2" runat="server" Width="904px">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None"
                        Width="896px" >
                        <RowStyle BackColor="#EFF3FB" />
                        <Columns>
                            <asp:BoundField DataField="Slot" HeaderText="Slot" SortExpression="Slot" />
                            <asp:TemplateField HeaderText="New Val">
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Width="56px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PreThick" HeaderText="PreThick" SortExpression="PreThick" NullDisplayText="None" />
                            <asp:BoundField DataField="PostThick" HeaderText="PostThick" SortExpression="PostThick" />
                            <asp:BoundField DataField="Removal" HeaderText="Removal" ReadOnly="True" SortExpression="Removal" />
                            <asp:BoundField DataField="WAT_Key" HeaderText="WAT_Key" SortExpression="WAT_Key" />
                            <asp:BoundField DataField="PreGeo_Key" HeaderText="PreGeo_Key" SortExpression="PreGeo_Key" />
                            <asp:BoundField DataField="PostGeo_Key" HeaderText="PostGeo_Key" SortExpression="PostGeo_Key" />
                        </Columns>
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <AlternatingRowStyle BackColor="LightSkyBlue" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                        SelectCommand="SELECT dbo.T7_InstanceInfo.Slot, dbo.T7_InstanceInfo.WAT_Key, dbo.T7_WaferActionTracking.PreGeo_Key, dbo.T7_GeoData.CenterThick AS PreThick, dbo.T7_WaferActionTracking.PostGeo_Key, T7_GeoData_1.CenterThick AS PostThick, dbo.T7_GeoData.CenterThick - T7_GeoData_1.CenterThick AS Removal FROM dbo.T7_GeoData AS T7_GeoData_1 RIGHT OUTER JOIN dbo.T7_WaferActionTracking INNER JOIN dbo.T7_InstanceInfo ON dbo.T7_WaferActionTracking.WAT_Key = dbo.T7_InstanceInfo.WAT_Key INNER JOIN dbo.T_FGI_Boxes ON dbo.T7_InstanceInfo.InstanceID = dbo.T_FGI_Boxes.InstanceKey ON T7_GeoData_1.Geo_Key = dbo.T7_WaferActionTracking.PostGeo_Key LEFT OUTER JOIN dbo.T7_GeoData ON dbo.T7_WaferActionTracking.PreGeo_Key = dbo.T7_GeoData.Geo_Key WHERE (dbo.T_FGI_Boxes.CartonNumber = 1) ORDER BY dbo.T7_InstanceInfo.Slot">
                    </asp:SqlDataSource>
                    <br />
                    <asp:Button ID="ChangeButton" runat="server" Text="Change Values" OnClick="ChangeButton_Click" /><br />
                </asp:Panel>
                <br />
                <br />
                &nbsp;<br />
                <br />
                <br />
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

