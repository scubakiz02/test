<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ProcessWafers.aspx.vb" Inherits="Production_ProcessWafers" title="Untitled Page" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <span style="font-size: 16pt; color: #009900">

        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Black"
            Text="Lot Process / History Screen:"></asp:Label><br />
        <br />
        <asp:Panel ID="Panel1" runat="server" Width="915px">
            <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Black"
                Style="left: 16px; top: 0px" Text="Lot Number:"></asp:Label>&nbsp;
    <asp:TextBox ID="LotNumberTextBox" runat="server" Font-Size="16pt" AutoPostBack="True"></asp:TextBox><asp:CheckBox ID="CheckBoxNewWork" runat="server" />
        </asp:Panel>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
               <%-- Pop up stuff for panels--%>
                <asp:Button ID="Button200" runat="server" Text="Button" Style="display: none" />
                <cc1:ModalPopupExtender ID="Button200_ModalPopupExtender" runat="server"
                    BackgroundCssClass="modalBackground" DropShadow="True" DynamicServicePath=""
                    Enabled="True" PopupControlID="MapPanel" TargetControlID="Button200"
                    OkControlID="ButtonClose" RepositionMode="RepositionOnWindowResize">
                </cc1:ModalPopupExtender>

                <asp:Button ID="Button300" runat="server" Text="Button" Style="display: none" />
                <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
                    BackgroundCssClass="modalBackground" DropShadow="True" DynamicServicePath=""
                    Enabled="True" PopupControlID="PanelBigInfo" TargetControlID="Button300"
                    OkControlID="ButtonInfoClose" RepositionMode="RepositionOnWindowResize">
                </cc1:ModalPopupExtender>
                 <%-- End of Pop up stuff for panels--%>

                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp;
                <asp:Button ID="RefreshButton" runat="server" Text="Refresh" Visible="False" />
                <asp:Button ID="ClearButton" runat="server" Text="Clear" />
                <br />
                <br />

                
                <asp:Panel ID="Panel2" runat="server" Width="915px">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None" Font-Size="Medium" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <Columns>
                            <asp:BoundField DataField="LotEntry" HeaderText="LotEntry" SortExpression="LotEntry" />
                            <asp:BoundField DataField="ProcessOrder" HeaderText="Step" SortExpression="ProcessOrder" />
                            <asp:BoundField DataField="StageName" HeaderText="Area" SortExpression="StageName" />
                            <asp:BoundField DataField="In" HeaderText="In" SortExpression="In" NullDisplayText="N/A" />
                            <asp:BoundField DataField="Out" HeaderText="Out" SortExpression="Out" />
                            <asp:BoundField DataField="Complete" HeaderText="Complete" SortExpression="Complete" NullDisplayText="Not Complete" DataFormatString="{0:d}" />
                            <asp:ButtonField ButtonType="Button" Text="Select" CommandName="Select" />
                            <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" ItemStyle-ForeColor="Red" ItemStyle-BackColor="Black" ItemStyle-Font-Size="X-Large" />
                            <asp:ButtonField ButtonType="Button" Text="" CommandName="EditNote" ItemStyle-BackColor="Black" />
                        </Columns>
                        <RowStyle BackColor="#EFF3FB" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="LightBlue" />
                    </asp:GridView>
                </asp:Panel>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder, dbo.UniqueProcesses.StageName, SUM(dbo.WaferMover.InQty) AS [In], SUM(dbo.WaferMover.OutQty) AS Out, dbo.UniqueProcesses.Complete, dbo.UniqueProcesses.Notes AS Notes FROM dbo.UniqueProcesses LEFT OUTER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] GROUP BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.Complete, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder, dbo.UniqueProcesses.Notes HAVING (dbo.UniqueProcesses.LotEntry = N'') AND (NOT (SUM(dbo.WaferMover.OutQty) IS NULL))"></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT Stage, LotNumber FROM dbo.T_Stage_Report WHERE (Stage = N'WIP 2') AND (LotNumber = N'1713-6496-3069')"></asp:SqlDataSource>
                
                <asp:Panel ID="MapPanel" runat="server" BackColor="#E0E0E0" Width="511px" HorizontalAlign="Center">
                    <br />
                    Edit Stage:
                    <asp:Label ID="LabelStage" runat="server" Text="Label"></asp:Label>&nbsp;, Lot#
                    <asp:Label ID="LabelLotNumber" runat="server" Text="0"></asp:Label>
                    &nbsp;Notes<br />
                    <asp:TextBox ID="TextBoxComment" runat="server" Width="448px"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Button ID="ButtonSaveComment" runat="server" Text="Save" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="ButtonClose" runat="server" Text="Cancel" />
                    <br />
                </asp:Panel>

                <asp:Panel ID="PanelBigInfo" runat="server" HorizontalAlign="Center">
                    <asp:Label ID="Label3" runat="server" Text="This Lot Has Data Issues. Please contact Engineering or IT." ForeColor="Black" Font-Size="X-Large" BackColor="#CC3300" BorderColor="Black" BorderStyle="Solid" BorderWidth="5px" Font-Bold="True"></asp:Label><br />

                    <asp:TextBox ID="TextBoxInfo" runat="server" TextMode="MultiLine" Wrap="False" Width="1250px" Height="750px" Font-Bold="True" Font-Size="Larger" BackColor="#FFFFCC"></asp:TextBox>
                    <br />
                    <asp:Button ID="ButtonInfoClose" runat="server" Text="Close" />
                    
                </asp:Panel>

            </ContentTemplate>
        </asp:UpdatePanel>
    </span>
</asp:Content>

