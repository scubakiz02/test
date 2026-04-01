<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ViewLots2.aspx.vb" Inherits="PC_ViewLots2" Title="View Lots" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="View Lots In Process"></asp:Label><br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <asp:Button ID="Button2" runat="server" Text="Button" Style="display: none" />
            <cc1:ModalPopupExtender ID="Button2_ModalPopupExtender" runat="server"
                BackgroundCssClass="modalBackground" DropShadow="True" DynamicServicePath=""
                Enabled="True" PopupControlID="MapPanel" TargetControlID="Button2"
                OkControlID="ButtonClose" RepositionMode="RepositionOnWindowResize">
            </cc1:ModalPopupExtender>


            <asp:Panel ID="Panel1" runat="server" Width="915px">
                Select Stage:
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2"
                    DataTextField="StageName" DataValueField="StageName" Width="200px" AppendDataBoundItems="True">
                    <asp:ListItem Selected="True">Select One...</asp:ListItem>
                    <asp:ListItem Selected="False">All Stages</asp:ListItem>
                </asp:DropDownList>

                &nbsp; &nbsp;Lot Filter:&nbsp;<asp:TextBox ID="TextBoxLotFilter" runat="server"
                    AutoPostBack="True"></asp:TextBox>


                <br />
            </asp:Panel>
            <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                <ProgressTemplate>
                    &nbsp;<img src="../Color/Animated_LoadingBigger.gif" />
                    &nbsp;&nbsp;&nbsp;Loading...
                </ProgressTemplate>
            </asp:UpdateProgress>
            <br />
            <asp:Panel ID="Panel2" runat="server" Width="915px">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                    DataSourceID="SqlDataSource1" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <Columns>
                        <asp:BoundField DataField="LotNumber" HeaderText="LotNumber" SortExpression="LotNumber" />
                        <asp:BoundField DataField="StageName" HeaderText="StageName" SortExpression="StageName" />
                        <asp:BoundField DataField="In" HeaderText="In" SortExpression="In" ReadOnly="True" />
                        <asp:BoundField DataField="Out" HeaderText="Out" SortExpression="Out" ReadOnly="True" />
                        <asp:ButtonField ButtonType="Button" CommandName="EditComment" Text="Edit" />
                        <asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="Comment" />
                        <asp:BoundField DataField="LastDate" HeaderText="LastDate" SortExpression="LastDate" DataFormatString="{0:d}" />
                    </Columns>
                    <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="LightBlue" />
                </asp:GridView>
                <br />
            </asp:Panel>
            <br />
            <asp:Panel ID="PanelInvPrint" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="ExportLabel" runat="server" Text="Create This Table As An Excel Document?"></asp:Label>
                            &nbsp; &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="ExportButton" runat="server" Text="Export To Excel" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink ID="ViewExcelFile" runat="server" Visible="false" ForeColor="Blue">Download Excel File Here</asp:HyperLink>
                        </td>
                    </tr>
                </table>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        &nbsp;<img src="../Color/Animated_LoadingBigger.gif" />
                        &nbsp;&nbsp;&nbsp;Loading...
                    </ProgressTemplate>
                </asp:UpdateProgress>

            </asp:Panel>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                SelectCommand="SELECT TOP 0 dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, SUM(dbo.WaferMover.InQty) AS [In], SUM(dbo.WaferMover.OutQty) AS Out, dbo.T_Sati_Lot_Comments.Comment, dbo.T_Sati_Lot_Comments.TheKey FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] LEFT OUTER JOIN dbo.T_Sati_Lot_Comments ON dbo.WaferMover.LotEntry = dbo.T_Sati_Lot_Comments.LotNumber WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.T_Sati_Lot_Comments.Comment, dbo.T_Sati_Lot_Comments.TheKey ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                SelectCommand="SELECT dbo.UniqueProcesses.StageName FROM dbo.UniqueProcesses LEFT OUTER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] WHERE (NOT (dbo.WaferMover.Disposition IS NULL)) GROUP BY dbo.UniqueProcesses.Complete, dbo.UniqueProcesses.StageName HAVING (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check'))"></asp:SqlDataSource>

            <asp:Panel ID="MapPanel" runat="server" BackColor="#E0E0E0" Width="511px" HorizontalAlign="Center">
                <br />
                Edit Lot#
        <asp:Label ID="LabelLotNumber" runat="server" Text="0"></asp:Label>
                &nbsp;Comment<br />
                <asp:TextBox ID="TextBoxComment" runat="server" Width="448px"></asp:TextBox>
                <br />
                <br />
                <asp:Button ID="ButtonSaveComment" runat="server" Text="Save" />
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="ButtonClose" runat="server" Text="Cancel" />
                <br />
            </asp:Panel>


        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

