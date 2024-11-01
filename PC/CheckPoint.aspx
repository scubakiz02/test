<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="CheckPoint.aspx.vb" Inherits="PC_CheckPoint" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="height: 200px; width: 712px;">
        <tr>
            <td style="height: 1px" bordercolor="#33ccff" colspan="5">
                Lot Number:
                <asp:Label ID="LotNumLabel" runat="server" Text="Lot Number: " Width="160px" Font-Bold="True"></asp:Label>&nbsp;
                ID:
                <asp:Label ID="IDLabel" runat="server" Text="0" Width="72px" Font-Bold="True"></asp:Label>&nbsp;
                WL:
                <asp:Label ID="WLLabel" runat="server" Text="0" Width="72px" Font-Bold="True"></asp:Label></td>
            <td style="height: 1px; width: 210px;">
            </td>
        </tr>
        <tr>
            <td style="height: 1px" colspan="5">
                Check Point Area:
                <asp:Label ID="StageLabel" runat="server" Text="Label" Width="128px" Font-Bold="True"></asp:Label>
                <asp:Button ID="CheckInButton" runat="server" Text="Check In" style="position: relative" /></td>
            <td style="height: 1px; width: 210px;">
            </td>
        </tr>
        <tr>
            <td style="height: 4px" colspan="5">
                Statuse:
                <asp:Label ID="StatLabel" runat="server" Text="Label" Width="208px" Font-Bold="True"></asp:Label>&nbsp;
                Lot Type Code:&nbsp;
                <asp:Label ID="LotTypeLabel" runat="server" Text="0" Width="88px" Font-Bold="True"></asp:Label></td>
            <td style="height: 4px; width: 210px;">
            </td>
        </tr>
        <tr>
            <td style="height: 6px" colspan="5">
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp;
            </td>
            <td style="height: 6px; width: 210px;">
            </td>
        </tr>
        <tr>
            <td colspan="3" rowspan="5" style="vertical-align: top; width: 230px; border-top-style: solid;
                border-right-style: solid; border-left-style: solid; border-bottom-style: solid">
                Start Qty:
                <asp:Label ID="StartQtyLabel" runat="server" Text="0" Width="72px" Font-Bold="True"></asp:Label><br />
                End Qty:
                <asp:Label ID="EndQtyLabel" runat="server" Text="0" Width="72px" Font-Bold="True"></asp:Label><br />
                Split Qty:
                <asp:Label ID="SplitQtyLabel" runat="server" Text="0" Width="72px" Font-Bold="True"></asp:Label><br />
                Merge Qty:
                <asp:Label ID="MergeQtyLabel" runat="server" Text="0" Width="72px" Font-Bold="True"></asp:Label></td>
            <td style="vertical-align: top; border-top-style: solid; border-right-style: solid; border-left-style: solid; border-bottom-style: solid;" colspan="2" rowspan="5">
                In Qty:
                <asp:Label ID="InQtyLabel" runat="server" Text="0" Width="80px" Font-Bold="True"></asp:Label><br />
                Out Qty:
                <asp:Label ID="OutQtyLabel" runat="server" Text="0" Width="72px" Font-Bold="True"></asp:Label><br />
                Rejects:
                <asp:Label ID="RejectQtyLabel" runat="server" Text="0" Width="80px" Font-Bold="True"></asp:Label><br />
                Reworks:
                <asp:Label ID="ReworkQtyLabel" runat="server" Text="0" Width="72px" Font-Bold="True"></asp:Label><br />
                Check Value:
                <asp:Label ID="CheckValueLabel" runat="server" Text="0" Width="40px" Font-Bold="True"></asp:Label></td>
            <td style="height: 6px; width: 210px;">
            </td>
        </tr>
        <tr>
            <td style="height: 6px; width: 210px;">
            </td>
        </tr>
        <tr>
            <td style="height: 6px; width: 210px;">
            </td>
        </tr>
        <tr>
            <td style="height: 5px; width: 210px;">
            </td>
        </tr>
        <tr>
            <td style="height: 6px; width: 210px;">
            </td>
        </tr>
        <tr>
            <td style="width: 230px; height: 6px" colspan="3">
            </td>
            <td style="height: 6px" colspan="2">
                <asp:Button ID="ContinueButton" runat="server" Text="Continue" Visible="False" Width="72px" /></td>
            <td style="height: 6px; width: 210px;">
            </td>
        </tr>
        <tr>
            <td style="height: 230px" colspan="5">
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT LotType, Stage, ID, WL, LotNumber, [In], Out, Rejects, Rework, Date, [User] FROM dbo.T_Stage_Report WHERE (LotNumber = N'12')">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    ProviderName="<%$ ConnectionStrings:ALTSConnectionString.ProviderName %>" SelectCommand="SELECT LotEntry, ProcessOrder, StageName, Complete FROM dbo.UniqueProcesses WHERE (LotEntry = N'1375-6205-3209') ORDER BY ProcessOrder">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="InOutQtysSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT dbo.WaferMover.LotEntry, dbo.UniqueProcesses.ProcessOrder, dbo.UniqueProcesses.StageName, SUM(dbo.WaferMover.InQty) AS IQ, SUM(dbo.WaferMover.OutQty) AS OQ FROM dbo.WaferMover INNER JOIN dbo.UniqueProcesses ON dbo.WaferMover.LotEntry = dbo.UniqueProcesses.LotEntry AND dbo.WaferMover.[Order] = dbo.UniqueProcesses.ProcessOrder GROUP BY dbo.WaferMover.LotEntry, dbo.UniqueProcesses.ProcessOrder, dbo.UniqueProcesses.StageName HAVING (dbo.WaferMover.LotEntry = N'2119-6467-3047')">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="RejectQtySqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT SUM(dbo.DefectTracking.Qty) AS RejectQty FROM dbo.WaferMover INNER JOIN dbo.UniqueProcesses ON dbo.WaferMover.LotEntry = dbo.UniqueProcesses.LotEntry AND dbo.WaferMover.[Order] = dbo.UniqueProcesses.ProcessOrder INNER JOIN dbo.DefectTracking ON dbo.WaferMover.MovementEntry = dbo.DefectTracking.MovementEntry WHERE (dbo.DefectTracking.Location = '-1' OR dbo.DefectTracking.Location = '-2') AND (dbo.UniqueProcesses.ProcessOrder > 2) AND (dbo.UniqueProcesses.ProcessOrder < 5) GROUP BY dbo.UniqueProcesses.LotEntry HAVING (dbo.UniqueProcesses.LotEntry = N'1750-6253-3209') AND (NOT (SUM(dbo.DefectTracking.Qty) = 0))">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="SplitQtySqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT SUM(Qty) AS SplitQty FROM dbo.ActionTracker WHERE (ParentLotNum = N'1713-6496-3069') AND (Action LIKE N'split%') AND (P_Order < 7) AND (P_Order > 2)">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="MergedQtySqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT SUM(Qty) AS SplitQty FROM dbo.ActionTracker WHERE (ParentLotNum = N'2119-6467-3047') AND (Action LIKE N'Merge%') AND (P_Order < 7) AND (P_Order > 2)">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="ReworkQtySqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT SUM(dbo.DefectTracking.Qty) AS ReworkQty FROM dbo.WaferMover INNER JOIN dbo.UniqueProcesses ON dbo.WaferMover.LotEntry = dbo.UniqueProcesses.LotEntry AND dbo.WaferMover.[Order] = dbo.UniqueProcesses.ProcessOrder INNER JOIN dbo.DefectTracking ON dbo.WaferMover.MovementEntry = dbo.DefectTracking.MovementEntry WHERE (dbo.DefectTracking.Location < - 2) AND (dbo.UniqueProcesses.ProcessOrder > 3) AND (dbo.UniqueProcesses.ProcessOrder < 5) GROUP BY dbo.UniqueProcesses.LotEntry HAVING (dbo.UniqueProcesses.LotEntry = N'2119-6467-3047') AND (NOT (SUM(dbo.DefectTracking.Qty) = 0))">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="ReworkTypsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT SUM(dbo.DefectTracking.Qty) AS ReworkQty, dbo.DefectTracking.Location FROM dbo.WaferMover INNER JOIN dbo.UniqueProcesses ON dbo.WaferMover.LotEntry = dbo.UniqueProcesses.LotEntry AND dbo.WaferMover.[Order] = dbo.UniqueProcesses.ProcessOrder INNER JOIN dbo.DefectTracking ON dbo.WaferMover.MovementEntry = dbo.DefectTracking.MovementEntry WHERE (dbo.DefectTracking.Location < - 2) AND (dbo.UniqueProcesses.ProcessOrder > 3) AND (dbo.UniqueProcesses.ProcessOrder < 5) GROUP BY dbo.UniqueProcesses.LotEntry, dbo.DefectTracking.Location HAVING (NOT (SUM(dbo.DefectTracking.Qty) = 0)) AND (dbo.UniqueProcesses.LotEntry = N'4022-4484-2902')">
                </asp:SqlDataSource>
            </td>
            <td style="height: 230px; width: 210px;">
            </td>
        </tr>
    </table>
</asp:Content>

