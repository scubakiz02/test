<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SelectedLotStageToWork.aspx.vb" Inherits="Production_SelectedLotStageToWork" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <span style="color: blue"><span style="font-size: 16pt"><strong>
         
        Stage Input Screen:</strong>&nbsp;<br />
    </span></span> 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="915px">
    <asp:Label ID="ModeLabel" runat="server" Font-Bold="True" Text="Label" Width="256px"></asp:Label><br />
    <table style="clear: none; overflow: auto; width: 688px; height: 160px; font-size: 12pt; color: #000000;">
        <tr>
            <td style="width: 193px; text-align: left; vertical-align: top; border-top-style: solid; border-right-style: solid; border-left-style: solid; border-bottom-style: solid; height: 205px; background-color: lightgreen;" 
                colspan="2">
                Process<strong> </strong>Lot<strong style="vertical-align: top">: </strong>
                <asp:Label ID="LotLabel" runat="server" Font-Size="14pt" Text="Label" Width="208px" Font-Bold="True"></asp:Label><br />
                <strong>S</strong>tage<strong> </strong>Name<span style="background-color: #90ee90">:
                </span>
                <asp:Label ID="StageLabel" runat="server" Font-Size="14pt" Text="Label" Width="208px" Font-Bold="True"></asp:Label><br />
                In Qty: &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:Label ID="InQtyLabel" runat="server"
                    Font-Bold="True" Style="left: 0px; top: 16px" Text="Label"></asp:Label><br />
                Good Qty: &nbsp;&nbsp;
                <asp:Label ID="GoodQtyLabel" runat="server" Font-Bold="True" Text="Label"></asp:Label><br />
                Defect Qty: &nbsp;
                <asp:Label ID="DefectQtyLabel" runat="server" Font-Bold="True" Text="Label"></asp:Label><br />
                Out Qty: &nbsp; &nbsp; &nbsp;
                <asp:Label ID="OutQtyLabel" runat="server" Font-Bold="True" Text="Label"></asp:Label><br />
                Qty Left: &nbsp; &nbsp; &nbsp;
                <asp:Label ID="LeftQtyLabel" runat="server" Font-Bold="True" Text="Label"></asp:Label>&nbsp;
    <asp:Button ID="RecButton" runat="server" Text="Reconcile Lot" Width="96px" Height="24px" Visible="False" /><br />
                <br />
                Good Qty:<asp:TextBox ID="EnterQtyTextBox" runat="server" Width="88px" Font-Bold="True" style="background-color: white"></asp:TextBox><br />
                Enter Good Qty<asp:Button ID="EnterGoodWafersButton" runat="server" Text="Go" Visible="False" Height="24px" />
                <br />
                <br />
                <asp:GridView ID="GridView6" runat="server" AutoGenerateColumns="False" DataSourceID="SplitIDSqlDataSource" Visible="False">
                    <Columns>
                        <asp:BoundField DataField="To" HeaderText="ID" SortExpression="To" />
                        <asp:TemplateField HeaderText="Partial Qty">
                            <ItemTemplate>
                                <asp:TextBox ID="PartialTextBox" runat="server" Width="70px">0</asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Label ID="PartialLabel1" runat="server" Text="Enter Partial Qty" Visible="False"></asp:Label>&nbsp;<asp:Button
                    ID="EnterPartialButton" runat="server" Text="Go" Visible="False" /></td>
            <td style="width: 262px; vertical-align: top; border-top-style: solid; border-right-style: solid; border-left-style: solid; border-bottom-style: solid; clear: none; position: static; text-align: left; color: #000000; background-color: #ffffcc;" 
                colspan="2" rowspan="2">
                Info:<br />
                <asp:TextBox ID="InfoTextBox" runat="server" Height="56px" TextMode="MultiLine" Width="248px" ForeColor="#FF0000"></asp:TextBox><br />
                <br />
                Recorded Defecs:<br />
    <asp:GridView ID="GridView3" runat="server" Style="display: inline;" Width="232px" AutoGenerateColumns="False" DataSourceID="DefectSqlDataSource" CellPadding="3" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
        <Columns>
            <asp:BoundField DataField="DefectName" HeaderText="DefectName" SortExpression="DefectName" />
            <asp:BoundField DataField="Column1" HeaderText="Qty" SortExpression="Column1" />
            <asp:BoundField DataField="Group" HeaderText="Group" SortExpression="Group" />
        </Columns>
        <FooterStyle BackColor="White" ForeColor="#000066" />
        <RowStyle ForeColor="#000066" />
        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
    </asp:GridView>
                <br />
                <br />
                <asp:Label ID="Label9" runat="server" Font-Size="14pt" Text="Split Lots:" Width="80px"></asp:Label><br />
                Select ID:<asp:DropDownList ID="SplitIDDropDownList" runat="server" DataSourceID="SplitIDSqlDataSource"
                    DataTextField="To" DataValueField="To" Width="88px" style="clear: none; position: static">
                </asp:DropDownList><br />
                Qty To Split:<asp:TextBox ID="QtyToSplitTextBox" runat="server" Width="88px" style="clear: none; position: static"></asp:TextBox><br />
                <asp:Button ID="SplitButton" runat="server" Text="SplitThis Lot" Height="24px" Width="88px" /><br />
    <asp:GridView ID="GridView1" runat="server" Height="56px" Width="200px" AutoGenerateColumns="False" DataSourceID="SplitSqlDataSource" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
        <Columns>
            <asp:BoundField DataField="ChildLotNum" HeaderText="ChildLotNum" SortExpression="ChildLotNum" />
            <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" />
        </Columns>
        <FooterStyle BackColor="White" ForeColor="#000066" />
        <RowStyle ForeColor="#000066" />
        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
    </asp:GridView>
                <br />
                <asp:Label ID="Label4" runat="server" Font-Size="14pt" Text="Merged Lots:" Width="104px"></asp:Label><br />
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataSourceID="MergedSqlDataSource"
                    Width="184px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                    <Columns>
                        <asp:BoundField DataField="ChildLotNum" HeaderText="ChildLotNum" SortExpression="ChildLotNum" />
                        <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" />
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <RowStyle ForeColor="#000066" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
                &nbsp;&nbsp;<br />
                <asp:Panel ID="MakeLabelPanel" runat="server" BackColor="#FFFF80" Visible="False">
                    <asp:Label ID="Label1" runat="server" Font-Size="14pt" Text="Make Label:" Width="168px"></asp:Label><br />
                    <br />
                    200mm and Less<br />
                    ID:&nbsp;<asp:DropDownList ID="MakeLabelDropDownList" runat="server" DataSourceID="SplitIDSqlDataSource"
                    DataTextField="To" DataValueField="To" Width="80px" style="clear: none; position: static" OnSelectedIndexChanged="MakeLabelDropDownList_SelectedIndexChanged" AppendDataBoundItems="True" AutoPostBack="True">
                        <asp:ListItem>Select ID</asp:ListItem>
                    </asp:DropDownList><br />
                    Qty:
                    <asp:DropDownList ID="LabelQtyDropDownList" runat="server" Width="72px">
                        <asp:ListItem Selected="True">25</asp:ListItem>
                        <asp:ListItem>24</asp:ListItem>
                        <asp:ListItem>23</asp:ListItem>
                        <asp:ListItem>22</asp:ListItem>
                        <asp:ListItem>21</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>19</asp:ListItem>
                        <asp:ListItem>18</asp:ListItem>
                        <asp:ListItem>17</asp:ListItem>
                        <asp:ListItem>16</asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>14</asp:ListItem>
                        <asp:ListItem>13</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>9</asp:ListItem>
                        <asp:ListItem>8</asp:ListItem>
                        <asp:ListItem>7</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList><br />
                    Printer:
                    <asp:DropDownList ID="LabelPrinterDropDownList" runat="server" Width="72px">
                        <asp:ListItem>Zebra1</asp:ListItem>
                        <asp:ListItem>Zebra2</asp:ListItem>
                        <asp:ListItem>Zebra_2B</asp:ListItem>
                        <asp:ListItem>Zebra3</asp:ListItem>
                        <asp:ListItem>Zebra4</asp:ListItem>
                        <asp:ListItem>Zebra5</asp:ListItem>
                        <asp:ListItem>Zebra9</asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:Button ID="Button1" runat="server" Text="Select ID For Label" />
                    <br />
                    300mm<br />
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Production/SurfScanLabelMaker.aspx">Make 300mm Labels</asp:HyperLink><br />
                    <br />
                </asp:Panel>
                &nbsp;<asp:Button ID="CCButton" runat="server" Text="Make Count Adj" Visible="False" />
                <asp:TextBox ID="CCTextBox" runat="server" Visible="False" Width="136px"></asp:TextBox></td>
        </tr>
        <tr style="color: #000000">
            <td colspan="2" style="vertical-align: top; width: 193px; border-top-style: solid;
                border-right-style: solid; border-left-style: solid; background-color: paleturquoise;
                text-align: left; border-bottom-style: solid">
                <strong><span style="text-decoration: underline">To Add a Defect:<br />
                </span></strong>Enter Defect Qty Here
                <asp:TextBox ID="TextBox1" runat="server" Font-Bold="True" 
                    style="background-color: white" Width="104px"></asp:TextBox>
                <br />
                <span style="color: #ff6600"><span style="color: #330000">Click Add to the 
                defect below.<br />
                </span>
                <asp:GridView ID="GridView4" runat="server" AllowSorting="True" 
                    AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" 
                    BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                    DataSourceID="DefectsForStageSqlDataSource" style="display: inline" 
                    Width="336px">
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <Columns>
                        <asp:BoundField DataField="Defect" HeaderText="Defect" 
                            SortExpression="Defect" />
                        <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                        <asp:BoundField DataField="Group" HeaderText="Group" SortExpression="Group" />
                        <asp:ButtonField ButtonType="Button" CommandName="Add" Text="Add" />
                    </Columns>
                    <RowStyle ForeColor="#000066" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
                <br />
                <asp:TextBox ID="PartialQtyTextBox" runat="server" Visible="False" Width="88px"></asp:TextBox>
                <asp:Button ID="PartialEnterButton" runat="server" Text="Enter Partial" 
                    Visible="False" />
                </span>
            </td>
        </tr>
        <tr style="color: #000000">
            <td colspan="4" rowspan="1" style="vertical-align: top; border-top-style: solid;
                border-right-style: solid; border-left-style: solid; height: 155px; background-color: lightsalmon;
                text-align: left; border-bottom-style: solid">
                <span style="font-family: 'Times New Roman'; mso-fareast-font-family: 'Times New Roman';
                    mso-ansi-language: EN-US; mso-fareast-language: EN-US; mso-bidi-language: AR-SA">
                    Available Partials to Merge with this lot: </span>
                <br />
                <br />
                <asp:GridView ID="GridView5" runat="server" AutoGenerateColumns="False" DataSourceID="PartialsSqlDataSource"
                    Visible="False">
                    <Columns>
                        <asp:BoundField DataField="LotEntry" HeaderText="LotEntry" SortExpression="LotEntry" />
                        <asp:BoundField DataField="In" HeaderText="In" SortExpression="In" />
                        <asp:TemplateField HeaderText="Check to Add">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" Text="Add" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Label ID="AddPartialLabel" runat="server" Text="Check the lots then Click" Visible="False"></asp:Label>
                <asp:Button ID="AddPartialButton" runat="server" Text="Go" Visible="False" /></td>
        </tr>
    </table>
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp;<br />
    <asp:SqlDataSource ID="PartialsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT LotEntry, SUM(InQty) AS [In] FROM dbo.WaferMover WHERE ([Order] = 0) GROUP BY LotEntry HAVING (LotEntry LIKE N'%-zzzz') AND (LotEntry LIKE N'2900-%') AND (SUM(OutQty) = 0) ORDER BY LotEntry">
    </asp:SqlDataSource>
    <br />
    <asp:SqlDataSource ID="MergedSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT ChildLotNum, Qty FROM dbo.ActionTracker WHERE (ParentLotNum = N'4320-5949-3159') AND (P_Order = 7) AND (Action LIKE N'Merge%')">
    </asp:SqlDataSource>
                <asp:SqlDataSource ID="SplitIDSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT [To] FROM dbo.TransferID_ByStage WHERE (StageName = N'Presort' OR StageName = N'All') AND ([From] = N'')">
                </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        InsertCommand="INSERT INTO dbo.T_Lot_Defect_Tracking(LotEntry, ProcessOrder, Qty, Type, Name, Stage, [User]) VALUES (,,,,,,)"
        SelectCommand="SELECT LotEntry, ProcessOrder, Qty, Type, Name, Stage, [User] FROM dbo.T_Lot_Defect_Tracking WHERE (LotEntry = '2386-6493-3242')">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="MovmentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT MIN(DISTINCT MovementEntry) AS Expr1 FROM dbo.WaferMover WHERE (LotEntry = N'4320-5949-3159') AND ([Order] = 10)">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="DefectSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT dbo.DefectTracking.DefectName, SUM(dbo.DefectTracking.Qty), dbo.T_ID_Defects.[Group] FROM dbo.WaferMover INNER JOIN dbo.DefectTracking ON dbo.WaferMover.MovementEntry = dbo.DefectTracking.MovementEntry INNER JOIN dbo.T_ID_Defects ON dbo.DefectTracking.DefectName = dbo.T_ID_Defects.Defect WHERE (dbo.WaferMover.LotEntry = N'2785-6058-3241') AND (dbo.WaferMover.[Order] = 5) AND (dbo.T_ID_Defects.ID = '2785') GROUP BY dbo.DefectTracking.DefectName, dbo.T_ID_Defects.[Group]">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="DefectSumSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT SUM(dbo.DefectTracking.Qty) AS Expr1 FROM dbo.WaferMover INNER JOIN dbo.DefectTracking ON dbo.WaferMover.MovementEntry = dbo.DefectTracking.MovementEntry WHERE (dbo.WaferMover.LotEntry = N'4320-5949-3159') AND (dbo.WaferMover.[Order] = 10)">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="DefectsForStageSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT dbo.T_ID_Defects.Defect, dbo.T_ID_Defects.Type, dbo.T_ID_Defects.[Group] FROM dbo.T_ID_Defects INNER JOIN dbo.DefectDefs ON dbo.T_ID_Defects.Defect = dbo.DefectDefs.DefectName WHERE (dbo.T_ID_Defects.ID = '2386') AND (dbo.DefectDefs.StageName = N'Presort') ORDER BY dbo.DefectDefs.StageName">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="InOutSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        DataSourceMode="DataReader" SelectCommand="SELECT SUM(InQty) AS [In], SUM(OutQty) AS Out, SUM(InQty) - SUM(OutQty) AS [left] FROM dbo.WaferMover GROUP BY LotEntry, [Order] HAVING (LotEntry = N'4320-5949-3159') AND ([Order] = 14)">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="GoodQtySqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT SUM(InQty) AS [In] FROM dbo.WaferMover GROUP BY LotEntry, [Order] HAVING (LotEntry = N'4320-5949-3159') AND ([Order] = 14)">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SplitSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT ChildLotNum, Qty FROM dbo.ActionTracker WHERE (ParentLotNum = N'4320-5949-3159') AND (P_Order = 7) AND (Action LIKE N'Split%')">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT LotEntry, ProcessOrder, StageName FROM dbo.UniqueProcesses WHERE (LotEntry = N'4269-6765L-R539')">
    </asp:SqlDataSource>
    <br />
</asp:Content>

