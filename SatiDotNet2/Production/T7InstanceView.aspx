<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="T7InstanceView.aspx.vb" Inherits="Production_T7InstanceView" title="Untitled Page" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
        
        <asp:Button ID="Button2" runat="server" Text="Button"  style="display:none"  />
            <cc1:ModalPopupExtender ID="Button2_ModalPopupExtender" runat="server" 
                BackgroundCssClass="modalBackground" DropShadow="True" DynamicServicePath="" 
                Enabled="True" PopupControlID="MapPanel" TargetControlID="Button2" 
                OkControlID="MapCloseButton" RepositionMode="RepositionOnWindowResize" Y="0">
            </cc1:ModalPopupExtender>
             
            <asp:Button ID="Button3" runat="server" Text="Button" style="display:none"/>
            <cc1:ModalPopupExtender ID="Button3_ModalPopupExtender" runat="server" 
                BackgroundCssClass="modalBackground" DropShadow="True" DynamicServicePath="" 
                Enabled="True" TargetControlID="Button3" PopupControlID="PanelNoMap" Y="0">
            </cc1:ModalPopupExtender>
        
        Instance or Box#<br />
            <asp:Panel ID="Panel1" runat="server" Width="142px">
                <asp:RadioButton ID="InstanceRadioButton" runat="server" GroupName="sel" Text="Instance" AutoPostBack="True" OnCheckedChanged="InstanceRadioButton_CheckedChanged" />
                <br />
                <asp:RadioButton ID="WaferBoxRadioButton" runat="server" GroupName="sel" Text="Wafer Box" AutoPostBack="True" OnCheckedChanged="WaferBoxRadioButton_CheckedChanged" /><br />
                <asp:CheckBox ID="FulDetailCheckBox" runat="server" Text="Full Detail" AutoPostBack="True" OnCheckedChanged="FulDetailCheckBox_CheckedChanged" /></asp:Panel>
            
            
            <br />
            <asp:TextBox id="TextBox1" runat="server" Width="96px" AutoPostBack="True" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>&nbsp;<asp:Button id="Button1" runat="server" Text="Find"></asp:Button><br />
            <asp:Panel ID="Panel2" runat="server">
                <asp:GridView id="GridView1" runat="server" AutoGenerateColumns="False" 
                    DataSourceID="SqlDataSource1" ForeColor="#333333" CellPadding="2" 
                    BorderColor="Black" CaptionAlign="Top">
<FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" Wrap="False"></FooterStyle>
<Columns>
<asp:BoundField DataField="Slot" SortExpression="Slot" HeaderText="Slot"></asp:BoundField>
    <asp:BoundField DataField="T7" HeaderText="T7" SortExpression="T7" />
       <asp:BoundField DataField="Started Production" HeaderText="Started Production"
        SortExpression="Started Production" DataFormatString="{0:d}" />
    <asp:BoundField DataField="Pre Geo Date" HeaderText="Pre Geo Date" 
        SortExpression="Pre Geo Date" DataFormatString="{0:d}" />
    <asp:BoundField DataField="Pre Geo CenterThick" HeaderText="Pre Geo CenterThick"
        SortExpression="Pre Geo CenterThick" />
    <asp:BoundField DataField="Pre Geo Tool" HeaderText="Pre Geo Tool"
        SortExpression="Pre Geo Tool" />
    <asp:BoundField DataField="Post Geo Date" HeaderText="Post Geo Date" 
        SortExpression="Post Geo Date" DataFormatString="{0:d}" />
    <asp:BoundField DataField="Post Geo Center Thick" 
        HeaderText="Post Geo Center Thick" SortExpression="Post Geo Center Thick" />
    <asp:BoundField DataField="Type" HeaderText="Type" 
        SortExpression="Type" />
    <asp:BoundField DataField="Post Geo Tool" HeaderText="Post Geo Tool"
        SortExpression="Post Geo Tool" />
    <asp:BoundField DataField="Microns Removed" HeaderText="Microns Removed" 
        SortExpression="Microns Removed" ReadOnly="True" />
    <asp:BoundField DataField="Laser Scan Date" HeaderText="Laser Scan Date" 
        SortExpression="Laser Scan Date" DataFormatString="{0:d}" />
    <asp:BoundField DataField="Laser Tool" HeaderText="Laser Tool" 
        SortExpression="Laser Tool" />
    <asp:BoundField DataField="Days In Prosess" HeaderText="Days In Prosess" 
        ReadOnly="True" SortExpression="Days In Prosess" />
    <asp:TemplateField ShowHeader="False" SortExpression="Map">
        <ItemTemplate>
            <asp:Label ID="MapfileLabel" runat="server" Text='<%# Bind("Map") %>' 
                Visible="False"></asp:Label>
        </ItemTemplate>
        <EditItemTemplate>
            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Map") %>'></asp:TextBox>
        </EditItemTemplate>
    </asp:TemplateField>
    <asp:ButtonField ButtonType="Button" CommandName="Map" Text="Map" />
</Columns>

<RowStyle BackColor="#EFF3FB" HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False"></RowStyle>

<EditRowStyle BackColor="#2461BF"></EditRowStyle>

<SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True" Wrap="False"></SelectedRowStyle>

<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" Wrap="False"></PagerStyle>

<HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False"></HeaderStyle>

<AlternatingRowStyle BackColor="White" Wrap="False"></AlternatingRowStyle>
                    <EmptyDataRowStyle Wrap="False" />
</asp:GridView>
            </asp:Panel>
            <BR />
            <asp:SqlDataSource id="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                
                
                SelectCommand="SELECT dbo.T7_InstanceInfo.Slot, dbo.T7_WaferActionTracking.T7, dbo.T7_WaferActionTracking.StartDate AS [Started Production], PreT7_GeoData.RecordDate AS [Pre Geo Date], PreT7_GeoData.CenterThick AS [Pre Geo CenterThick], PreT7_GeoData.Tool AS [Pre Geo Tool], PostT7_GeoData.RecordDate AS [Post Geo Date], PostT7_GeoData.CenterThick AS [Post Geo Center Thick], PostT7_GeoData.Type, PostT7_GeoData.Tool AS [Post Geo Tool], PreT7_GeoData.CenterThick - PostT7_GeoData.CenterThick AS [Microns Removed], dbo.T7_ParticalData.RecordDate AS [Laser Scan Date], dbo.T7_ParticalData.Tool AS [Laser Tool], DATEDIFF(dd, dbo.T7_WaferActionTracking.StartDate, dbo.T7_ParticalData.RecordDate) AS [Days In Prosess], dbo.T7_ParticalData.Map FROM dbo.T7_GeoData AS PreT7_GeoData RIGHT OUTER JOIN dbo.T7_WaferActionTracking ON PreT7_GeoData.Geo_Key = dbo.T7_WaferActionTracking.PreGeo_Key LEFT OUTER JOIN dbo.T7_ParticalData ON dbo.T7_WaferActionTracking.Partical_Key = dbo.T7_ParticalData.Partical_Key LEFT OUTER JOIN dbo.T7_GeoData AS PostT7_GeoData ON dbo.T7_WaferActionTracking.PostGeo_Key = PostT7_GeoData.Geo_Key RIGHT OUTER JOIN dbo.T7_InstanceInfo INNER JOIN dbo.T_FGI_Boxes ON dbo.T7_InstanceInfo.InstanceID = dbo.T_FGI_Boxes.InstanceKey ON dbo.T7_WaferActionTracking.WAT_Key = dbo.T7_InstanceInfo.WAT_Key WHERE (dbo.T_FGI_Boxes.BoxInvNumber = 0) ORDER BY dbo.T7_InstanceInfo.Slot DESC"></asp:SqlDataSource> 
            <asp:Panel ID="PanelNoMap" runat="server" Height="96px" Width="104px" 
                BackColor="#FF0066">
                <div style="text-align: center">
                    <asp:Button ID="ButtonClose" runat="server" Text="Close" />
                    <br />
                    <br />
                    No Map<br />
                </div>
            </asp:Panel>
            <br />
            <asp:Panel ID="MapPanel" runat="server" BackColor="#E0E0E0" Width="905px">
                <table class="style1">
                    <tr>
                        <td style="text-align: center">
                            <asp:Label ID="MapRowLabel" runat="server" Text="test" Visible="False"></asp:Label>
                            <asp:Label ID="CSlotLabel" runat="server" Text="Label"></asp:Label>
                        </td>
                       
                        <td style="text-align: right">
                            <asp:Button ID="MapCloseButton" runat="server" OnClick="MapCloseButton_Click" 
                                Text="Close" />
                        </td>
                       
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="2">
                            <asp:Image ID="MapImage" runat="server" Width="800px" />
                        </td>
                        
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: right">
                            <asp:Button ID="BackMapButton" runat="server" Text="Back" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="NextMapButton" runat="server" Height="26px" Text="Next" />
                        </td>
                        
                    </tr>
                </table>                
            </asp:Panel>
            
</contenttemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

