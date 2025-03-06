<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="GeoViewer.aspx.vb" Inherits="Reports_GeoViewer" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <asp:Panel ID="PanelBig" runat="server">
    
        Geo Tool Search Viewer<br />
        <asp:DropDownList ID="DropDownListTools" runat="server" 
            Width="155px" AutoPostBack="True">
            <asp:ListItem Selected="True">Select Data Base</asp:ListItem>
            <asp:ListItem>ADE</asp:ListItem>
            <asp:ListItem>Hologenix</asp:ListItem>
            <asp:ListItem>Leo</asp:ListItem>
            <asp:ListItem>GigaMat</asp:ListItem>
        </asp:DropDownList>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <img src="../Color/Animated_LoadingBigger.gif" />Working...
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:Panel ID="PanelADEBuild" runat="server">
            <br />
            ID#<asp:TextBox ID="TextBoxID" runat="server" Width="50px"></asp:TextBox>
            &nbsp;Run#<asp:TextBox ID="TextBoxRun" runat="server" Width="50px"></asp:TextBox>
            &nbsp;WL#<asp:TextBox ID="TextBoxWL" runat="server" Width="50px"></asp:TextBox>
            <br />
            <br />
            Show
            <br />
            <asp:RadioButton ID="RadioButtonADEToolFinal" runat="server" Checked="True" 
                GroupName="adeToolType" Text="Final Tools" />
&nbsp;<asp:RadioButton ID="RadioButtonADEToolPresort" runat="server" 
                GroupName="adeToolType" Text="Presort Tools" />
            <br />
            <asp:RadioButton ID="RadioButtonADERecords_All" runat="server" 
                GroupName="ADERecords" Text="All Records" />
            ,
            <asp:RadioButton ID="RadioButtonADERecords_Pass" runat="server" 
                GroupName="ADERecords" Text="Passed Wafers" />
            ,
            <asp:RadioButton ID="RadioButtonADERecords_Other" runat="server" 
                GroupName="ADERecords" Text="Not Passed Wafers" Checked="True" />
            <br />
            <asp:CheckBox ID="CheckBoxADEShowCenterThick" runat="server" Checked="True" 
                Text="Center Thick" />
            ,
            <asp:CheckBox ID="CheckBoxADEShowAvgThick" runat="server" Text="AVG Thick" />
            ,
            <asp:CheckBox ID="CheckBoxADEShowRes" runat="server" Checked="True" 
                Text="Res" />
            ,
            <asp:CheckBox ID="CheckBoxADEShowRes2" runat="server" Text="Res2" />
            ,
            <asp:CheckBox ID="CheckBoxADEShowTTV" runat="server" Checked="True" 
                Text="TTV" />
            ,
            <asp:CheckBox ID="CheckBoxADEShowTIR" runat="server" Checked="True" 
                Text="TIR" />
            ,
            <br />
            <asp:CheckBox ID="CheckBoxADEShowBow" runat="server" Checked="True" 
                Text="Bow" />
            ,
            <asp:CheckBox ID="CheckBoxADEShowWarp" runat="server" Checked="True" 
                Text="Warp" />
            ,
            <asp:CheckBox ID="CheckBoxADEShowType" runat="server" Checked="True" 
                Text="Type" />
            ,
            <asp:CheckBox ID="CheckBoxADEShowDate" runat="server" Checked="True" 
                Text="Date and Time" />
            ,<br />
            <asp:CheckBox ID="CheckBoxADEShowWafer" runat="server" Text="Wafer#" />
            ,
            <asp:CheckBox ID="CheckBoxADEShowReceiver" runat="server" AutoPostBack="True" 
                Text="Receiver" />
            ,
            <asp:CheckBox ID="CheckBoxADEShowID" runat="server" Text="ID" />
            ,
            <asp:CheckBox ID="CheckBoxADEShowRun" runat="server" Text="Run" />
            ,
            <asp:CheckBox ID="CheckBoxADEShowWL" runat="server" Text="WL" />
            &nbsp;<br />
            <asp:Button ID="Button1" runat="server" Text="Get Records" />
            <br />
        </asp:Panel>
        
        <asp:Panel ID="PanelADE_DB" runat="server">
            <table class="style1">
                <tr>
                    <td>
                        ADE Data Base Results&nbsp;
                        <asp:Label ID="LabelADERecordsFound" runat="server" Text="0"></asp:Label>
                        &nbsp;:
                        <asp:Label ID="Label2" runat="server" Text="Records Found"></asp:Label>
                    </td>
                    <td>
                        &nbsp;&nbsp;
                    </td>
                </tr>
            </table>
            <asp:GridView ID="GridViewADE" runat="server" 
                AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSourceADE" 
                ForeColor="#333333" GridLines="None">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="location" HeaderText="Tool" 
                        SortExpression="location" />
                    <asp:BoundField DataField="class" HeaderText="class" SortExpression="class" />
                    <asp:BoundField DataField="cen_thk" HeaderText="Cen Thk" 
                        SortExpression="cen_thk" />
                    <asp:BoundField DataField="ave_thk" HeaderText="Avg Thk" 
                        SortExpression="ave_thk" />
                    <asp:BoundField DataField="resistivity" HeaderText="Res" 
                        SortExpression="resistivity" />
                    <asp:BoundField DataField="Res2" HeaderText="Res2" SortExpression="Res2" />
                    <asp:BoundField DataField="ttv" HeaderText="TTV" SortExpression="ttv" />
                    <asp:BoundField DataField="tir" HeaderText="TIR" SortExpression="tir" />
                    <asp:BoundField DataField="bow" HeaderText="Bow" SortExpression="bow" />
                    <asp:BoundField DataField="warp" HeaderText="Warp" SortExpression="warp" />
                    <asp:BoundField DataField="type" HeaderText="Type" SortExpression="type" />
                    <asp:BoundField DataField="EventTime" HeaderText="EventTime" 
                        SortExpression="EventTime" />
                    <asp:BoundField DataField="wafer" HeaderText="Wafer" SortExpression="wafer" />
                    <asp:BoundField DataField="Receiver" HeaderText="Receiver" 
                        SortExpression="Receiver" />
                    <asp:BoundField DataField="id#" HeaderText="ID" SortExpression="id#" />
                    <asp:BoundField DataField="run#" HeaderText="Run" SortExpression="run#" />
                    <asp:BoundField DataField="wl#" HeaderText="WL" SortExpression="wl#" />
                </Columns>
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="#CCFFFF" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSourceADE" runat="server" 
                ConnectionString="<%$ ConnectionStrings:AutoDataConnectionString %>" 
                SelectCommand="SELECT location, class, cen_thk, ave_thk, resistivity, Res2, ttv, tir, bow, warp, type, EventTime, wafer, Receiver, id#, run#, wl# FROM dbo.ADE_data WHERE (id# = N'2967') AND (run# = N'2658s') AND (wl# = N'r626') ORDER BY ENTRY DESC">
            </asp:SqlDataSource>
        </asp:Panel>
        <asp:Panel ID="PanelHoloBuild" runat="server">
            <br />
            Lot#<asp:TextBox ID="TextBoxHoloLotNumber" runat="server"></asp:TextBox>
            <br />
            <br />
            Show
            <br />
            <asp:RadioButton ID="RadioButtonHoloRecords_All" runat="server" Checked="True" 
                GroupName="HoloRecords" Text="All Records" />
            ,
            <asp:RadioButton ID="RadioButtonHoloRecordsStation6" runat="server" 
                GroupName="HoloRecords" Text="Station 6" />
            ,
            <asp:RadioButton ID="RadioButtonHoloRecordsStation5" runat="server" 
                GroupName="HoloRecords" Text="Station 5" />
            ,
            <asp:RadioButton ID="RadioButtonHoloRecordsStation4" runat="server" 
                GroupName="HoloRecords" Text="Station 4" />
            <br />
            <asp:CheckBox ID="CheckBoxHoloShowCenterThick" runat="server" Checked="True" 
                Text="Center Thick" />
            ,
            <asp:CheckBox ID="CheckBoxHoloShowAvgThick" runat="server" Text="AVG Thick" />
            ,
            <asp:CheckBox ID="CheckBoxHoloShowResCenter" runat="server" Checked="True" 
                Text="Cnt Res" />
            ,
            <asp:CheckBox ID="CheckBoxHoloShowResAvg" runat="server" Text="Avg Res" />
            ,
            <asp:CheckBox ID="CheckBoxHoloShowTTV" runat="server" Checked="True" 
                Text="TTV" />
            ,
            <asp:CheckBox ID="CheckBoxHoloShowTIR" runat="server" Checked="True" 
                Text="TIR" />
            ,
            <br />
            <asp:CheckBox ID="CheckBoxHoloShowBow" runat="server" Checked="True" 
                Text="Bow" />
            ,
            <asp:CheckBox ID="CheckBoxHoloShowWarp" runat="server" Checked="True" 
                Text="Warp" />
            ,
            <asp:CheckBox ID="CheckBoxHoloShowType" runat="server" Checked="True" 
                Text="Type" />
            ,
            <asp:CheckBox ID="CheckBoxHoloShowDate" runat="server" Checked="True" 
                Text="Date and Time" />
            ,<br />
            <asp:CheckBox ID="CheckBoxHoloShowWaferT7" runat="server" Text="Wafer T7" />
            ,
            <asp:CheckBox ID="CheckBoxHoloShowS_Slot" runat="server" AutoPostBack="True" 
                Text="S Slot" />
            ,
            <asp:CheckBox ID="CheckBoxHoloShowStation" runat="server" Text="Station" 
                Checked="True" />
            ,
            <asp:CheckBox ID="CheckBoxHoloSlotShowDSlot" runat="server" Text="D Slot" />
            ,
            <asp:CheckBox ID="CheckBoxHoloShowLot" runat="server" Text="Lot Number" />
            &nbsp;<br />
            <asp:Button ID="Button2" runat="server" Text="Get Records" />
            <br />
        </asp:Panel>
        <asp:Panel ID="PanelHologenix" runat="server">
            Hologenix Data Base Results&nbsp;
            <asp:Label ID="LabelHoloRecordsFound" runat="server" Text="0"></asp:Label>
            &nbsp;:
            <asp:Label ID="Label3" runat="server" Text="Records Found"></asp:Label>
            <br />
            <asp:GridView ID="GridViewHolo" runat="server" AutoGenerateColumns="False" 
                DataSourceID="SqlDataSourceHolo" CellPadding="4" ForeColor="#333333" 
                GridLines="None">
                <RowStyle BackColor="#EFF3FB" />
                <Columns>
                    <asp:BoundField DataField="Class" HeaderText="Class" SortExpression="Class" />
                    <asp:BoundField DataField="CntThk" HeaderText="CntThk" 
                        SortExpression="CntThk" DataFormatString="{0:F2}" />
                    <asp:BoundField DataField="AvgThk" HeaderText="AvgThk" 
                        SortExpression="AvgThk" DataFormatString="{0:F2}" />
                    <asp:BoundField DataField="CntRes" HeaderText="CntRes" 
                        SortExpression="CntRes" DataFormatString="{0:F2}" />
                    <asp:BoundField DataField="AvgRes" HeaderText="AvgRes" 
                        SortExpression="AvgRes" DataFormatString="{0:F2}" />
                    <asp:BoundField DataField="TTV" HeaderText="TTV" SortExpression="TTV" DataFormatString="{0:F2}" />
                    <asp:BoundField DataField="TIR" HeaderText="TIR" SortExpression="TIR" DataFormatString="{0:F2}" />
                    <asp:BoundField DataField="Bow" HeaderText="Bow" SortExpression="Bow" DataFormatString="{0:F2}" />
                    <asp:BoundField DataField="Warp" HeaderText="Warp" SortExpression="Warp" DataFormatString="{0:F2}" />
                    <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" DataFormatString="{0:F2}" />
                    <asp:BoundField DataField="Clock" HeaderText="Clock" SortExpression="Clock"  />
                    <asp:BoundField DataField="WaferID" HeaderText="WaferID" 
                        SortExpression="WaferID" />
                    <asp:BoundField DataField="S_Slot" HeaderText="S_Slot" 
                        SortExpression="S_Slot" />
                    <asp:BoundField DataField="Station" HeaderText="Station" 
                        SortExpression="Station" />
                    <asp:BoundField DataField="D_Slot" HeaderText="D_Slot" 
                        SortExpression="D_Slot" />
                    <asp:BoundField DataField="LotID" HeaderText="LotID" SortExpression="LotID" />
                </Columns>
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <AlternatingRowStyle BackColor="#CCFFFF" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSourceHolo" runat="server" 
                ConnectionString="<%$ ConnectionStrings:AutoDataConnectionString %>" 
                SelectCommand="SELECT WaferClassName AS Class, CntThk, AvgThk, CntRes, AvgRes, TTV, TIR, Bow, TotWarp AS Warp, Dotation AS Type, Clock, WaferID, SourceSlot AS S_Slot, DestCarrierID AS Station, DestSlot AS D_Slot, LotID FROM dbo.DC_OCR WHERE (LotID = N'') ORDER BY Clock DESC">
            </asp:SqlDataSource>
            <br />
        </asp:Panel>
        <br />
        <br />
        <asp:Panel ID="PanelGigaMat" runat="server">
            Gigamate Data Base Results<br />
            <br />
        </asp:Panel>
        <br />
        <br />
        <asp:Panel ID="PanelLeo" runat="server">
            Leo Data Base Results<br />
            <br />
        </asp:Panel>
        <br />
        <br />
    
    </asp:Panel>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

