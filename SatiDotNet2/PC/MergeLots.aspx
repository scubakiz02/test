<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MergeLots.aspx.vb" Inherits="PC_MergeLots" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="768px" Font-Bold="False">
                Select Diameter &nbsp;
                <asp:DropDownList ID="DiaDropDownList" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                    DataSourceID="DieSqlDataSource" DataTextField="Diameter" DataValueField="Diameter"
                    Width="104px" OnSelectedIndexChanged="DiaDropDownList_SelectedIndexChanged">
                    <asp:ListItem Selected="True">Select One...</asp:ListItem>
                </asp:DropDownList><br />
                <asp:CheckBox ID="FirstPassCheckBox" runat="server" Text="View First Pass Lots" OnCheckedChanged="FirstPassCheckBox_CheckedChanged" AutoPostBack="True" /><br />
                <asp:CheckBox ID="SecondPassCheckBox" runat="server" Text="View Second Pass Lots" OnCheckedChanged="SecondPassCheckBox_CheckedChanged" AutoPostBack="True" /><br />
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        Updating...<img src="../Color/Animated_LoadingBigger.gif" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <br />
                <strong>
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Red"
                        Text="Select Lot(s) To Be Merged...."></asp:Label></strong><asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataSourceID="FromLotsSqlDataSource" ForeColor="#333333" GridLines="None">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select Lot(s)">
                            <ItemTemplate>
                                <asp:CheckBox ID="MergeCheckBox" runat="server" Text="Select" AutoPostBack="True" OnCheckedChanged="MergeCheckBox_CheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="LotNumber" HeaderText="LotNumber" SortExpression="LotNumber" />
                        <asp:BoundField DataField="StageName" HeaderText="StageName" SortExpression="StageName" />
                        <asp:BoundField DataField="F Step" HeaderText="F Step" SortExpression="F Step" />
                        <asp:BoundField DataField="In To Stage" HeaderText="In Qty" SortExpression="In To Stage" />
                        <asp:BoundField DataField="Out Of Stage" HeaderText="Out Qty" SortExpression="Out Of Stage" />
                        <asp:BoundField DataField="Left" HeaderText="Left Qty" SortExpression="Left" />
                        <asp:TemplateField HeaderText="Merge Qty">
                            <ItemTemplate>
                                <asp:TextBox ID="QtyTextBox" runat="server" Width="40px" AutoPostBack="True" OnTextChanged="QtyTextBox_TextChanged"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="LightBlue" />
                </asp:GridView>
                <br />
                <asp:Panel ID="Panel3" runat="server" BackColor="#FFC0C0" BorderColor="White" Height="32px"
                    Style="vertical-align: middle; text-align: center" Width="672px">
                    You Have Selected&nbsp; &nbsp;<asp:Label ID="TotalQtyLabel" runat="server" Font-Bold="True"
                        Font-Size="X-Large" ForeColor="Blue" Text="0"></asp:Label>&nbsp; &nbsp;Wafers
                    for merging</asp:Panel>
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; .<br />
                <br />
                <asp:Panel ID="Panel2" runat="server" Height="112px" Width="232px">
                    Lots To Merge into Filter.<br />
                    <asp:CheckBox ID="SameStageCheckBox" runat="server" AutoPostBack="True" Checked="True"
                        OnCheckedChanged="CheckBox1_CheckedChanged" Text="Same Stage" /><br />
                    <asp:RadioButton ID="SameIDRadioButton" runat="server" AutoPostBack="True" Checked="True"
                        GroupName="Filter2" OnCheckedChanged="SameIDRadioButton_CheckedChanged" Text="Same ID" /><br />
                    <asp:RadioButton ID="SameCustomerRadioButton" runat="server" AutoPostBack="True"
                        GroupName="Filter2" OnCheckedChanged="SameCustomerRadioButton_CheckedChanged"
                        Text="Same Customer" /><br />
                    <asp:RadioButton ID="AllIDsRadioButton" runat="server" AutoPostBack="True" GroupName="Filter2"
                        OnCheckedChanged="AllIDsRadioButton_CheckedChanged" Text="All Ids" /></asp:Panel>
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>
                        Updating...<img src="../Color/Animated_LoadingBigger.gif" />
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <br />
                <strong>
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Red"
                        Text="Merge Selected Lot(s) From Above Into The Lot Selected Below ... " Height="64px" Width="384px"></asp:Label><br />
                </strong>
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataSourceID="ToLotsSqlDataSource" ForeColor="#333333" GridLines="None">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#EFF3FB" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <asp:RadioButton ID="SelectRadioButton" runat="server" GroupName="TheLot" Text="Merge Into" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="LotNumber" HeaderText="LotNumber" SortExpression="LotNumber" />
                        <asp:BoundField DataField="StageName" HeaderText="StageName" SortExpression="StageName" />
                        <asp:BoundField DataField="F Step" HeaderText="F Step" SortExpression="F Step" />
                        <asp:BoundField DataField="In To Stage" HeaderText="In To Stage" ReadOnly="True"
                            SortExpression="In To Stage" />
                        <asp:BoundField DataField="Out Of Stage" HeaderText="Out Of Stage" ReadOnly="True"
                            SortExpression="Out Of Stage" />
                        <asp:BoundField DataField="Left" HeaderText="Left" ReadOnly="True" SortExpression="Left" />
                    </Columns>
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="LightBlue" />
                </asp:GridView>
                <br />
                &nbsp; &nbsp;&nbsp;
                <asp:Button ID="MakeMergeButton" runat="server" OnClick="MakeMergeButton_Click" Text="Merge Lot(s)" />
                <asp:Label ID="InfoLabel" runat="server" Font-Bold="True" ForeColor="Black" Text="Merge Was Completed.."
                    Visible="False"></asp:Label><br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <asp:SqlDataSource ID="FromLotsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND (dbo.MainID.Diameter = 2) AND (NOT (dbo.UniqueProcesses.LotEntry LIKE N'%R%')) GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="ToLotsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.ProcessOrder AS [F Step], SUM(dbo.WaferMover.InQty) AS [In To Stage], SUM(dbo.WaferMover.OutQty) AS [Out Of Stage], SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) AS [Left] FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] INNER JOIN dbo.MainID ON LEFT (dbo.UniqueProcesses.LotEntry, 4) = dbo.MainID.MainID WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) AND (dbo.MainID.Diameter = 2) AND (NOT (dbo.UniqueProcesses.LotEntry LIKE N'%R%')) AND (dbo.UniqueProcesses.LotEntry LIKE N'2%') OR (dbo.UniqueProcesses.LotEntry LIKE N'269999999999%') GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.ProcessOrder HAVING (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) = 0)) AND (NOT (SUM(dbo.WaferMover.InQty) - SUM(dbo.WaferMover.OutQty) < 0)) AND (NOT (dbo.UniqueProcesses.LotEntry = N'1-134-1234')) ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName">
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="DieSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT Diameter FROM dbo.MainID GROUP BY Diameter ORDER BY Diameter DESC">
                </asp:SqlDataSource>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

