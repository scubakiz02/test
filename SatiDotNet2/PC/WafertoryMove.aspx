<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="WafertoryMove.aspx.vb" Inherits="PC_WafertoryMove" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Move Lot to Wafertory"></asp:Label><br />
    <asp:Label ID="Label2" runat="server"  Font-Size="Medium" Text="This will move a lot from WIP to the Wafertory Inventory"></asp:Label><br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
           
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
                        <asp:ButtonField ButtonType="Button" CommandName="Wafertory" Text="Move To Wafertory" />
                        <asp:BoundField DataField="LotNumber" HeaderText="LotNumber" SortExpression="LotNumber" />
                        <asp:BoundField DataField="StageName" HeaderText="StageName" SortExpression="StageName" />
                        <asp:BoundField DataField="In" HeaderText="In" SortExpression="In" ReadOnly="True" />
                        <asp:BoundField DataField="Out" HeaderText="Out" SortExpression="Out" ReadOnly="True" />                        
                        <asp:BoundField DataField="Comment" HeaderText="Comment" SortExpression="Comment" />
                        <asp:BoundField DataField="LastDate" HeaderText="LastDate" SortExpression="LastDate" DataFormatString="{0:d}" />
                        
                    </Columns>
                    <FooterStyle BackColor="#990000" ForeColor="White" Font-Bold="True" />
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                    <SortedAscendingCellStyle BackColor="#FDF5AC" />
                    <SortedAscendingHeaderStyle BackColor="#4D0000" />
                    <SortedDescendingCellStyle BackColor="#FCF6C0" />
                    <SortedDescendingHeaderStyle BackColor="#820000" />
                </asp:GridView>
                <br />
            </asp:Panel>
            <br />
                   
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        &nbsp;<img src="../Color/Animated_LoadingBigger.gif" />
                        &nbsp;&nbsp;&nbsp;Loading...
                    </ProgressTemplate>
                </asp:UpdateProgress>

            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                SelectCommand="SELECT TOP 0 dbo.UniqueProcesses.LotEntry AS LotNumber, dbo.UniqueProcesses.StageName, SUM(dbo.WaferMover.InQty) AS [In], SUM(dbo.WaferMover.OutQty) AS Out, dbo.T_Sati_Lot_Comments.Comment, dbo.T_Sati_Lot_Comments.TheKey FROM dbo.UniqueProcesses INNER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] LEFT OUTER JOIN dbo.T_Sati_Lot_Comments ON dbo.WaferMover.LotEntry = dbo.T_Sati_Lot_Comments.LotNumber WHERE (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check')) GROUP BY dbo.UniqueProcesses.StageName, dbo.UniqueProcesses.LotEntry, dbo.T_Sati_Lot_Comments.Comment, dbo.T_Sati_Lot_Comments.TheKey ORDER BY dbo.UniqueProcesses.LotEntry, dbo.UniqueProcesses.StageName"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                SelectCommand="SELECT dbo.UniqueProcesses.StageName FROM dbo.UniqueProcesses LEFT OUTER JOIN dbo.WaferMover ON dbo.UniqueProcesses.LotEntry = dbo.WaferMover.LotEntry AND dbo.UniqueProcesses.ProcessOrder = dbo.WaferMover.[Order] WHERE (NOT (dbo.WaferMover.Disposition IS NULL)) GROUP BY dbo.UniqueProcesses.Complete, dbo.UniqueProcesses.StageName HAVING (dbo.UniqueProcesses.Complete IS NULL) AND (NOT (dbo.UniqueProcesses.StageName = N'Shipping WIP')) AND (NOT (dbo.UniqueProcesses.StageName = N'Final PC Check'))"></asp:SqlDataSource>

          


        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

