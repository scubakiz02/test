<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="CreateSARMain.aspx.vb" Inherits="Reports_SAR_CreatSARMain" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <asp:Panel ID="Panel4" runat="server" Width="1072px">
    Select Month
    <asp:DropDownList ID="DateDropDownList" runat="server" AutoPostBack="True" DataSourceID="DatesSqlDataSource"
        DataTextField="ReportKey" DataValueField="ReportKey" AppendDataBoundItems="True">
        <asp:ListItem>Select Date...</asp:ListItem>
    </asp:DropDownList>&nbsp; - Filter By Site
    <asp:DropDownList ID="SiteDropDownList" runat="server" AppendDataBoundItems="True"
        AutoPostBack="True" DataSourceID="SitesSqlDataSource" DataTextField="CustomerID"
        DataValueField="CustomerID">
        <asp:ListItem>Select Site...</asp:ListItem>
    </asp:DropDownList>&nbsp; - Filter By Customer &nbsp;<asp:DropDownList ID="CustomerDropDownList"
        runat="server" AppendDataBoundItems="True" AutoPostBack="True" DataSourceID="CustomersSqlDataSource"
        DataTextField="Customer_Name" DataValueField="Customer_Name">
        <asp:ListItem>Select Customer...</asp:ListItem>
    </asp:DropDownList>&nbsp; -
    <asp:Button ID="ClearFilterButton" runat="server" Text="Clear Filter" />&nbsp;<asp:Button
        ID="RefreshButton" runat="server" Text="Refresh" />
                <br />
            </asp:Panel>
            <br />
            <asp:Panel ID="Panel3" runat="server" Width="125px">
                <br />
    <asp:Panel ID="Panel2" runat="server" BorderStyle="Solid" Height="24px" Style="border-top-width: thin;
        table-layout: fixed; border-left-width: thin; border-bottom-width: thin; overflow: hidden;
        position: static; border-right-width: thin" Width="1080px">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
            CellPadding="4" DataSourceID="IniSqlDataSource" ForeColor="#333333" GridLines="None"
            Width="1056px" style="table-layout: fixed; overflow: scroll" Height="160px">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:ButtonField CommandName="OpenDetail" ShowHeader="True" Text="Edit" />
                <asp:BoundField DataField="Bal" HeaderText="Bal" SortExpression="Bal" />
                <asp:BoundField DataField="MainID" HeaderText="MainID" SortExpression="MainID" />
                <asp:BoundField DataField="Stite" HeaderText="Site" ReadOnly="True" SortExpression="Stite" />
                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer"
                    Visible="False" />
                <asp:BoundField DataField="Start" HeaderText="Start" SortExpression="Start" />
                <asp:BoundField DataField="S_FGI" HeaderText="S_FGI" SortExpression="S_FGI" />
                <asp:BoundField DataField="Rec" HeaderText="Rec" SortExpression="Rec" />
                <asp:BoundField DataField="WH" HeaderText="WH" SortExpression="WH" />
                <asp:BoundField DataField="WL_Adj" HeaderText="WL_Adj" SortExpression="WL_Adj" />
                <asp:BoundField DataField="WIP" HeaderText="WIP" SortExpression="WIP" />
                <asp:BoundField DataField="FGI" HeaderText="FGI" SortExpression="FGI" />
                <asp:BoundField DataField="Rework" HeaderText="Rework" SortExpression="Rework" />
                <asp:BoundField DataField="Rejects" HeaderText="Rejects" SortExpression="Rejects" />
                <asp:BoundField DataField="CR_Par" HeaderText="CR_Par" SortExpression="CR_Par" />
                <asp:BoundField DataField="P_Par" HeaderText="P_Par" SortExpression="P_Par" />
                <asp:BoundField DataField="Split_Out" HeaderText="Split_Out" SortExpression="Split_Out" />
                <asp:BoundField DataField="Split_In" HeaderText="Split_In" SortExpression="Split_In" />
                <asp:BoundField DataField="Shipped" HeaderText="Shipped" SortExpression="Shipped" />
                <asp:BoundField DataField="Scrapped" HeaderText="Scrapped" SortExpression="Scrapped" />
            </Columns>
            <RowStyle BackColor="#EFF3FB" />
            <EditRowStyle BackColor="#2461BF" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#507CD1" BorderStyle="None" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="Panel1" runat="server" BorderStyle="Solid" Height="200px" Style="border-right: thin solid;
        table-layout: fixed; border-top: thin solid; overflow: auto; border-left: thin solid;
        border-bottom: thin solid; position: static" Width="1080px">
        <asp:GridView ID="MonthDataGridView" runat="server" AutoGenerateColumns="False"
            CellPadding="4" DataSourceID="IniSqlDataSource" ForeColor="#333333" GridLines="None"
            Width="1056px" style="table-layout: fixed; overflow: scroll" Height="160px">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:ButtonField CommandName="OpenDetail" ShowHeader="True" Text="Edit" />
                <asp:BoundField DataField="Bal" HeaderText="Bal" SortExpression="Bal" />
                <asp:BoundField DataField="MainID" HeaderText="MainID" SortExpression="MainID" />
                <asp:BoundField DataField="Stite" HeaderText="Site" ReadOnly="True" SortExpression="Stite" />
                <asp:BoundField DataField="Customer" HeaderText="Customer" SortExpression="Customer"
                    Visible="False" />
                <asp:BoundField DataField="Start" HeaderText="Start" SortExpression="Start" />
                <asp:BoundField DataField="S_FGI" HeaderText="S_FGI" SortExpression="S_FGI" />
                <asp:BoundField DataField="Rec" HeaderText="Rec" SortExpression="Rec" />
                <asp:BoundField DataField="WH" HeaderText="WH" SortExpression="WH" />
                <asp:BoundField DataField="WL_Adj" HeaderText="WL_Adj" SortExpression="WL_Adj" />
                <asp:BoundField DataField="WIP" HeaderText="WIP" SortExpression="WIP" />
                <asp:BoundField DataField="FGI" HeaderText="FGI" SortExpression="FGI" />
                <asp:BoundField DataField="Rework" HeaderText="Rework" SortExpression="Rework" />
                <asp:BoundField DataField="Rejects" HeaderText="Rejects" SortExpression="Rejects" />
                <asp:BoundField DataField="CR_Par" HeaderText="CR_Par" SortExpression="CR_Par" />
                <asp:BoundField DataField="P_Par" HeaderText="P_Par" SortExpression="P_Par" />
                <asp:BoundField DataField="Split_Out" HeaderText="Split_Out" SortExpression="Split_Out" />
                <asp:BoundField DataField="Split_In" HeaderText="Split_In" SortExpression="Split_In" />
                <asp:BoundField DataField="Shipped" HeaderText="Shipped" SortExpression="Shipped" />
                <asp:BoundField DataField="Scrapped" HeaderText="Scrapped" SortExpression="Scrapped" />
            </Columns>
            <RowStyle BackColor="#EFF3FB" />
            <EditRowStyle BackColor="#2461BF" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#507CD1" BorderStyle="None" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </asp:Panel>
    <table style="border-right: black thin solid; border-top: black thin solid; border-left: black thin solid;
        border-bottom: black thin solid" id="EditTABLE" runat="server" visible="false">
        <tr>
            <td nowrap="nowrap" style="border-right: black thin solid; border-top: black thin solid;
                border-left: black thin solid; width: 100px; border-bottom: black thin solid;
                background-color: #ccffff" valign="top">
                <strong>MainID </strong>
                <asp:Label ID="IDLabel" runat="server" Style="position: static" Text="ID"></asp:Label><br />
                    <asp:GridView ID="IDDataGridView" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        DataKeyNames="Key" DataSourceID="IDDataSqlDataSource" ForeColor="#333333" GridLines="None"
                        Width="392px">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <Columns>
                            <asp:CommandField ShowEditButton="True" />
                            <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True"
                                SortExpression="Key" Visible="False" />
                            <asp:TemplateField HeaderText="WH" SortExpression="WH">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" BackColor="#FFFF80" Text='<%# Bind("WH") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("WH") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="WIP" SortExpression="WIP">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server" BackColor="#FFFF80" Text='<%# Bind("WIP") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("WIP") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="RW" SortExpression="RW">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox3" runat="server" BackColor="#FFFF80" Text='<%# Bind("RW") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("RW") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FGI" SortExpression="FGI">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox4" runat="server" BackColor="#FFFF80" Text='<%# Bind("FGI") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("FGI") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Par_CR" SortExpression="Par_CR">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox5" runat="server" BackColor="#FFFF80" Text='<%# Bind("Par_CR") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("Par_CR") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Par_Polish" SortExpression="Par_Polish">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox6" runat="server" BackColor="#FFFF80" Text='<%# Bind("Par_Polish") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("Par_Polish") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle BackColor="#EFF3FB" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
            </td>
            <td nowrap="nowrap" rowspan="6" style="border-right: black thin solid; border-top: black thin solid;
                border-left: black thin solid; width: 100px; border-bottom: black thin solid"
                valign="top">
                <strong>Rejects</strong><br />
                <asp:GridView ID="RejGridView" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    CellPadding="4" DataKeyNames="Key" DataSourceID="RejSqlDataSource" ForeColor="#333333"
                    GridLines="None" Width="264px">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True"
                            SortExpression="Key" Visible="False" />
                        <asp:BoundField DataField="Defect" HeaderText="Defect" ReadOnly="True" SortExpression="Defect" />
                        <asp:TemplateField HeaderText="Qty" SortExpression="Qty">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" BackColor="#FFFF80" Text='<%# Bind("Qty") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td nowrap="nowrap" style="border-right: black thin solid; border-top: black thin solid;
                border-left: black thin solid; width: 100px; border-bottom: black thin solid"
                valign="top">
                <strong>
                    Received Shipments</strong><br />
                <asp:GridView ID="RecGridView" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataKeyNames="Key" DataSourceID="RecSqlDataSource" ForeColor="#333333" GridLines="None"
                    Width="392px">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True"
                            SortExpression="Key" Visible="False" />
                        <asp:BoundField DataField="EventTime" HeaderText="Received Date" SortExpression="EventTime" />
                        <asp:BoundField DataField="Track_ID" HeaderText="WL#" SortExpression="Track_ID" />
                        <asp:TemplateField HeaderText="Qty" SortExpression="Qty">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" BackColor="#FFFF80" Text='<%# Bind("Qty") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td nowrap="nowrap" style="border-right: black thin solid; border-top: black thin solid;
                border-left: black thin solid; width: 100px; border-bottom: black thin solid;
                background-color: #ccffff" valign="top">
                <strong>Shipped</strong><br />
                <asp:GridView ID="ShippedGridView" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataKeyNames="Key" DataSourceID="ShippedSqlDataSource" ForeColor="#333333" GridLines="None"
                    Width="392px">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True"
                            SortExpression="Key" Visible="False" />
                        <asp:BoundField DataField="EventTime" HeaderText="Shipped Date" SortExpression="EventTime" />
                        <asp:BoundField DataField="Track_ID" HeaderText="Invoice#" ReadOnly="True" SortExpression="Track_ID" />
                        <asp:TemplateField HeaderText="Qty" SortExpression="Qty">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" BackColor="#FFFF80" Text='<%# Bind("Qty") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td nowrap="nowrap" style="border-right: black thin solid; border-top: black thin solid;
                border-left: black thin solid; width: 100px; border-bottom: black thin solid"
                valign="top">
                &nbsp;<strong>Incoming Adj<br />
                <asp:GridView ID="IncAdjGridView" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataKeyNames="Key" DataSourceID="IncAdjSqlDataSource" ForeColor="#333333" GridLines="None"
                    Width="232px">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True"
                            SortExpression="Key" Visible="False" />
                        <asp:BoundField DataField="Track_ID" HeaderText="WL#" SortExpression="Track_ID" />
                        <asp:TemplateField HeaderText="Qty" SortExpression="Qty">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" BackColor="#FFFF80" Text='<%# Bind("Qty") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                </strong>
            </td>
        </tr>
        <tr>
            <td nowrap="nowrap" style="border-right: black thin solid; border-top: black thin solid;
                border-left: black thin solid; width: 100px; border-bottom: black thin solid;
                background-color: #ccffff" valign="top">
                &nbsp;<strong>Split Out</strong><br />
                <asp:GridView ID="Split_OutGridView" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataKeyNames="Key" DataSourceID="Split_OutSqlDataSource" ForeColor="#333333"
                    GridLines="None" Width="232px">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True"
                            SortExpression="Key" Visible="False" />
                        <asp:BoundField DataField="INID" HeaderText="To ID" ReadOnly="True" SortExpression="INID" />
                        <asp:TemplateField HeaderText="Qty" SortExpression="Qty">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" BackColor="#FFFF80" Text='<%# Bind("Qty") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td nowrap="nowrap" style="border-right: black thin solid; border-top: black thin solid;
                border-left: black thin solid; width: 100px; border-bottom: black thin solid"
                valign="top">
                &nbsp;<strong>Split In</strong><br />
                <asp:GridView ID="Split_InGridView" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataKeyNames="Key" DataSourceID="Split_InSqlDataSource" ForeColor="#333333" GridLines="None"
                    Width="232px">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True"
                            SortExpression="Key" Visible="False" />
                        <asp:BoundField DataField="OutID" HeaderText="From ID" ReadOnly="True" SortExpression="OutID" />
                        <asp:TemplateField HeaderText="Qty" SortExpression="Qty">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" BackColor="#FFFF80" Text='<%# Bind("Qty") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td nowrap="nowrap" style="border-right: black thin solid; border-top: black thin solid;
                vertical-align: top; border-left: black thin solid; width: 100px; border-bottom: black thin solid;
                position: static; text-align: left" valign="top">
                <strong>Return\Scrap Inv Line</strong><br />
                <asp:GridView ID="ScrapGridView" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataKeyNames="Key" DataSourceID="ScrapSqlDataSource" ForeColor="#333333" GridLines="None">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                        <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True"
                            SortExpression="Key" Visible="False" />
                        <asp:TemplateField HeaderText="Track_ID" SortExpression="Track_ID">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Track_ID") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Track_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Qty" SortExpression="Qty">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Qty") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                Add Record<br />
                <asp:Label ID="ScrapTrackIDLabel" runat="server" Text="Track ID"></asp:Label>
                <asp:TextBox ID="ScrapTrackIDTextBox" runat="server"></asp:TextBox><br />
                <asp:Label ID="ScrapQtyLabel" runat="server" Text="Qty"></asp:Label>
                &nbsp; &nbsp; &nbsp; &nbsp;
                <asp:TextBox ID="ScrapQtyTextBox" runat="server"></asp:TextBox>&nbsp;<br />
                <asp:Button ID="ScrapAddButton" runat="server" Text="Add Record" /></td>
            <td nowrap="nowrap" rowspan="1" style="border-right: black thin solid; border-top: black thin solid;
                border-left: black thin solid; width: 100px; border-bottom: black thin solid"
                valign="top">
                Notes:<br />
                <asp:GridView ID="NotesGridView" runat="server" AutoGenerateColumns="False" CellPadding="4"
                    DataKeyNames="key" DataSourceID="NotesSqlDataSource" ForeColor="#333333" GridLines="None"
                    Width="256px">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                        <asp:BoundField DataField="key" HeaderText="key" InsertVisible="False" ReadOnly="True"
                            SortExpression="key" Visible="False" />
                        <asp:TemplateField HeaderText="Note" SortExpression="Note">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Note") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Note") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                <br />
                <asp:TextBox ID="NotesTextBox" runat="server" Height="104px" TextMode="MultiLine"
                    Width="248px"></asp:TextBox><br />
                <asp:Button ID="NotesSaveButton" runat="server" Text="Add Note" /></td>
        </tr>
    </table>
    <br />
            </asp:Panel>
    <asp:SqlDataSource ID="ScrapSqlDataSource" runat="server" ConflictDetection="CompareAllValues"
        ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" DeleteCommand="DELETE FROM dbo.T_SAR_Action WHERE ([Key] = @original_Key) AND (Track_ID = @original_Track_ID) AND (Qty = @original_Qty)"
        InsertCommand="INSERT INTO dbo.T_SAR_Action(ReportKey, ID, Adj_Item, Track_ID, Qty, EventTime) VALUES (@ReportKey, @ID, @Adj_Item, @Track_ID, @Qty, @EventTime)"
        OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [Key], ReportKey, ID, Adj_Item, Track_ID, Qty, EventTime FROM dbo.T_SAR_Action WHERE (ReportKey = '6/1/2007') AND (ID = N'2386') AND (Adj_Item = 'scrap')"
        UpdateCommand="UPDATE dbo.T_SAR_Action SET Track_ID = @Track_ID, Qty = @Qty WHERE ([Key] = @original_Key) AND (Track_ID = @original_Track_ID) AND (Qty = @original_Qty)">
        <DeleteParameters>
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_Track_ID" Type="String" />
            <asp:Parameter Name="original_Qty" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Track_ID" Type="String" />
            <asp:Parameter Name="Qty" Type="Int32" />
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_Track_ID" Type="String" />
            <asp:Parameter Name="original_Qty" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="ReportKey" Type="DateTime" />
            <asp:Parameter Name="ID" Type="String" />
            <asp:Parameter Name="Adj_Item" Type="String" />
            <asp:Parameter Name="Track_ID" Type="String" />
            <asp:Parameter Name="Qty" Type="Int32" />
            <asp:Parameter Name="EventTime" Type="DateTime" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="IniSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT fctn_SAR_Ini_Population_1.ID AS MainID, dbo.Customer.CustomerID AS Stite, dbo.Customer.Customer_Name AS Customer, fctn_SAR_Ini_Population_2.WH_Inv + fctn_SAR_Ini_Population_2.WIP + fctn_SAR_Ini_Population_2.Rework + fctn_SAR_Ini_Population_2.FGI + fctn_SAR_Ini_Population_2.CR_Par + fctn_SAR_Ini_Population_2.P_Par AS Start, fctn_SAR_Ini_Population_2.FGI AS S_FGI, fctn_SAR_Ini_Population_1.ReceivedQty AS Rec, fctn_SAR_Ini_Population_1.WH_Inv AS WH, fctn_SAR_Ini_Population_1.Icn_Adj AS WL_Adj, fctn_SAR_Ini_Population_1.WIP, fctn_SAR_Ini_Population_1.FGI, fctn_SAR_Ini_Population_1.Rework, fctn_SAR_Ini_Population_1.Recects AS Rejects, fctn_SAR_Ini_Population_1.CR_Par, fctn_SAR_Ini_Population_1.P_Par, fctn_SAR_Ini_Population_1.Split_Out, fctn_SAR_Ini_Population_1.Split_In, fctn_SAR_Ini_Population_1.ShippedQty AS Shipped, (fctn_SAR_Ini_Population_2.WH_Inv + fctn_SAR_Ini_Population_2.WIP + fctn_SAR_Ini_Population_2.Rework + fctn_SAR_Ini_Population_2.FGI + fctn_SAR_Ini_Population_2.CR_Par + fctn_SAR_Ini_Population_2.P_Par + fctn_SAR_Ini_Population_1.Split_In + fctn_SAR_Ini_Population_1.Icn_Adj + fctn_SAR_Ini_Population_1.ReceivedQty) - (fctn_SAR_Ini_Population_1.WH_Inv + fctn_SAR_Ini_Population_1.WIP + fctn_SAR_Ini_Population_1.Rework + fctn_SAR_Ini_Population_1.FGI + fctn_SAR_Ini_Population_1.CR_Par + fctn_SAR_Ini_Population_1.P_Par + fctn_SAR_Ini_Population_1.ShippedQty + fctn_SAR_Ini_Population_1.Recects + fctn_SAR_Ini_Population_1.Split_Out + fctn_SAR_Ini_Population_1.Scrapped) AS Bal, fctn_SAR_Ini_Population_1.Scrapped FROM dbo.fctn_SAR_Ini_Population('6/1/2006') AS fctn_SAR_Ini_Population_1 INNER JOIN dbo.MainID ON fctn_SAR_Ini_Population_1.ID = dbo.MainID.MainID INNER JOIN dbo.fctn_SAR_Ini_Population('5/1/2006') AS fctn_SAR_Ini_Population_2 ON dbo.MainID.MainID = fctn_SAR_Ini_Population_2.ID INNER JOIN dbo.Customer ON dbo.MainID.CustomerID = dbo.Customer.CustomerID WHERE (dbo.Customer.Customer_Name = N'blank')">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="DatesSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT ReportKey FROM dbo.T_SAR_End_Inv GROUP BY ReportKey ORDER BY ReportKey">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="RecSqlDataSource" runat="server" ConflictDetection="CompareAllValues"
        ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" DeleteCommand="DELETE FROM [T_SAR_Action] WHERE [Key] = @original_Key AND [ReportKey] = @original_ReportKey AND [ID] = @original_ID AND [Adj_Item] = @original_Adj_Item AND [Track_ID] = @original_Track_ID AND [Qty] = @original_Qty AND [EventTime] = @original_EventTime"
        InsertCommand="INSERT INTO [T_SAR_Action] ([ReportKey], [ID], [Adj_Item], [Track_ID], [Qty], [EventTime]) VALUES (@ReportKey, @ID, @Adj_Item, @Track_ID, @Qty, @EventTime)"
        OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [Key], ReportKey, ID, Adj_Item, Track_ID, Qty, EventTime FROM dbo.T_SAR_Action WHERE (ReportKey = CONVERT (DATETIME, '2006-01-01 00:00:00', 102)) AND (ID = N'2386') AND (Adj_Item = 'Received')"
        UpdateCommand="UPDATE dbo.T_SAR_Action SET Qty = @Qty WHERE ([Key] = @original_Key) AND (Qty = @original_Qty)">
        <DeleteParameters>
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_ReportKey" Type="DateTime" />
            <asp:Parameter Name="original_ID" Type="String" />
            <asp:Parameter Name="original_Adj_Item" Type="String" />
            <asp:Parameter Name="original_Track_ID" Type="String" />
            <asp:Parameter Name="original_Qty" Type="Int32" />
            <asp:Parameter Name="original_EventTime" Type="DateTime" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Qty" Type="Int32" />
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_Qty" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="ReportKey" Type="DateTime" />
            <asp:Parameter Name="ID" Type="String" />
            <asp:Parameter Name="Adj_Item" Type="String" />
            <asp:Parameter Name="Track_ID" Type="String" />
            <asp:Parameter Name="Qty" Type="Int32" />
            <asp:Parameter Name="EventTime" Type="DateTime" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="RejSqlDataSource" runat="server" ConflictDetection="CompareAllValues"
        ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [Key], ReportKey, ID, Defect, Qty FROM dbo.T_SAR_Defects WHERE (ReportKey = CONVERT (DATETIME, '2006-01-01 00:00:00', 102)) AND (ID = N'2386') ORDER BY Defect"
        UpdateCommand="UPDATE dbo.T_SAR_Defects SET Qty = @Qty WHERE ([Key] = @original_Key) AND (Qty = @original_Qty)">
        <UpdateParameters>
            <asp:Parameter Name="Qty" />
            <asp:Parameter Name="original_Key" />
            <asp:Parameter Name="original_Qty" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="Split_OutSqlDataSource" runat="server" ConflictDetection="CompareAllValues"
        ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" DeleteCommand="DELETE FROM [T_SAR_ID_Transfer] WHERE [Key] = @original_Key AND [ReportKey] = @original_ReportKey AND [OutID] = @original_OutID AND [INID] = @original_INID AND [Qty] = @original_Qty"
        InsertCommand="INSERT INTO [T_SAR_ID_Transfer] ([ReportKey], [OutID], [INID], [Qty]) VALUES (@ReportKey, @OutID, @INID, @Qty)"
        OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [Key], ReportKey, OutID, INID, Qty FROM dbo.T_SAR_ID_Transfer WHERE (OutID = N'2386') AND (ReportKey = CONVERT (DATETIME, '2006-01-01 00:00:00', 102))"
        UpdateCommand="UPDATE dbo.T_SAR_ID_Transfer SET Qty = @Qty WHERE ([Key] = @original_Key) AND (Qty = @original_Qty)">
        <DeleteParameters>
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_ReportKey" Type="DateTime" />
            <asp:Parameter Name="original_OutID" Type="String" />
            <asp:Parameter Name="original_INID" Type="String" />
            <asp:Parameter Name="original_Qty" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Qty" Type="Int32" />
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_Qty" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="ReportKey" Type="DateTime" />
            <asp:Parameter Name="OutID" Type="String" />
            <asp:Parameter Name="INID" Type="String" />
            <asp:Parameter Name="Qty" Type="Int32" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="Split_InSqlDataSource" runat="server" ConflictDetection="CompareAllValues"
        ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" DeleteCommand="DELETE FROM [T_SAR_ID_Transfer] WHERE [Key] = @original_Key AND [ReportKey] = @original_ReportKey AND [OutID] = @original_OutID AND [INID] = @original_INID AND [Qty] = @original_Qty"
        InsertCommand="INSERT INTO [T_SAR_ID_Transfer] ([ReportKey], [OutID], [INID], [Qty]) VALUES (@ReportKey, @OutID, @INID, @Qty)"
        OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [Key], ReportKey, OutID, INID, Qty FROM dbo.T_SAR_ID_Transfer WHERE (ReportKey = CONVERT (DATETIME, '2006-01-01 00:00:00', 102)) AND (INID = N'2386')"
        UpdateCommand="UPDATE dbo.T_SAR_ID_Transfer SET Qty = @Qty WHERE ([Key] = @original_Key) AND (Qty = @original_Qty)">
        <DeleteParameters>
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_ReportKey" Type="DateTime" />
            <asp:Parameter Name="original_OutID" Type="String" />
            <asp:Parameter Name="original_INID" Type="String" />
            <asp:Parameter Name="original_Qty" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Qty" Type="Int32" />
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_Qty" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="ReportKey" Type="DateTime" />
            <asp:Parameter Name="OutID" Type="String" />
            <asp:Parameter Name="INID" Type="String" />
            <asp:Parameter Name="Qty" Type="Int32" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="ShippedSqlDataSource" runat="server" ConflictDetection="CompareAllValues"
        ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" DeleteCommand="DELETE FROM [T_SAR_Action] WHERE [Key] = @original_Key AND [ReportKey] = @original_ReportKey AND [ID] = @original_ID AND [Adj_Item] = @original_Adj_Item AND [Track_ID] = @original_Track_ID AND [Qty] = @original_Qty AND [EventTime] = @original_EventTime"
        InsertCommand="INSERT INTO [T_SAR_Action] ([ReportKey], [ID], [Adj_Item], [Track_ID], [Qty], [EventTime]) VALUES (@ReportKey, @ID, @Adj_Item, @Track_ID, @Qty, @EventTime)"
        OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [Key], ReportKey, ID, Adj_Item, Track_ID, Qty, EventTime FROM dbo.T_SAR_Action WHERE (ReportKey = CONVERT (DATETIME, '2006-01-01 00:00:00', 102)) AND (ID = N'2386') AND (Adj_Item = 'Shipped')"
        UpdateCommand="UPDATE dbo.T_SAR_Action SET Qty = @Qty WHERE ([Key] = @original_Key) AND (Qty = @original_Qty)">
        <DeleteParameters>
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_ReportKey" Type="DateTime" />
            <asp:Parameter Name="original_ID" Type="String" />
            <asp:Parameter Name="original_Adj_Item" Type="String" />
            <asp:Parameter Name="original_Track_ID" Type="String" />
            <asp:Parameter Name="original_Qty" Type="Int32" />
            <asp:Parameter Name="original_EventTime" Type="DateTime" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Qty" Type="Int32" />
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_Qty" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="ReportKey" Type="DateTime" />
            <asp:Parameter Name="ID" Type="String" />
            <asp:Parameter Name="Adj_Item" Type="String" />
            <asp:Parameter Name="Track_ID" Type="String" />
            <asp:Parameter Name="Qty" Type="Int32" />
            <asp:Parameter Name="EventTime" Type="DateTime" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="IncAdjSqlDataSource" runat="server" ConflictDetection="CompareAllValues"
        ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" DeleteCommand="DELETE FROM [T_SAR_Action] WHERE [Key] = @original_Key AND [ReportKey] = @original_ReportKey AND [ID] = @original_ID AND [Adj_Item] = @original_Adj_Item AND [Track_ID] = @original_Track_ID AND [Qty] = @original_Qty AND [EventTime] = @original_EventTime"
        InsertCommand="INSERT INTO [T_SAR_Action] ([ReportKey], [ID], [Adj_Item], [Track_ID], [Qty], [EventTime]) VALUES (@ReportKey, @ID, @Adj_Item, @Track_ID, @Qty, @EventTime)"
        OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [Key], ReportKey, ID, Adj_Item, Track_ID, Qty, EventTime FROM dbo.T_SAR_Action WHERE (ReportKey = CONVERT (DATETIME, '2006-01-01 00:00:00', 102)) AND (ID = N'2386') AND (Adj_Item = 'WL_Adj')"
        UpdateCommand="UPDATE dbo.T_SAR_Action SET Qty = @Qty WHERE ([Key] = @original_Key) AND (Qty = @original_Qty)">
        <DeleteParameters>
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_ReportKey" Type="DateTime" />
            <asp:Parameter Name="original_ID" Type="String" />
            <asp:Parameter Name="original_Adj_Item" Type="String" />
            <asp:Parameter Name="original_Track_ID" Type="String" />
            <asp:Parameter Name="original_Qty" Type="Int32" />
            <asp:Parameter Name="original_EventTime" Type="DateTime" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Qty" Type="Int32" />
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_Qty" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="ReportKey" Type="DateTime" />
            <asp:Parameter Name="ID" Type="String" />
            <asp:Parameter Name="Adj_Item" Type="String" />
            <asp:Parameter Name="Track_ID" Type="String" />
            <asp:Parameter Name="Qty" Type="Int32" />
            <asp:Parameter Name="EventTime" Type="DateTime" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SitesSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT CustomerID FROM dbo.Customer GROUP BY CustomerID"></asp:SqlDataSource>
    <asp:SqlDataSource ID="CustomersSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT Customer_Name FROM dbo.Customer GROUP BY Customer_Name ORDER BY Customer_Name">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="IDDataSqlDataSource" runat="server" ConflictDetection="CompareAllValues"
        ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" DeleteCommand="DELETE FROM [T_SAR_End_Inv] WHERE [Key] = @original_Key AND [ReportKey] = @original_ReportKey AND [ID] = @original_ID AND [WH] = @original_WH AND [WIP] = @original_WIP AND [RW] = @original_RW AND [FGI] = @original_FGI AND [Par_CR] = @original_Par_CR AND [Par_Polish] = @original_Par_Polish"
        InsertCommand="INSERT INTO [T_SAR_End_Inv] ([ReportKey], [ID], [WH], [WIP], [RW], [FGI], [Par_CR], [Par_Polish]) VALUES (@ReportKey, @ID, @WH, @WIP, @RW, @FGI, @Par_CR, @Par_Polish)"
        OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [Key], ReportKey, ID, WH, WIP, RW, FGI, Par_CR, Par_Polish FROM dbo.T_SAR_End_Inv WHERE (ReportKey = CONVERT (DATETIME, '2006-01-01 00:00:00', 102)) AND (ID = N'2386')"
        UpdateCommand="UPDATE dbo.T_SAR_End_Inv SET WH = @WH, WIP = @WIP, RW = @RW, FGI = @FGI, Par_CR = @Par_CR, Par_Polish = @Par_Polish WHERE ([Key] = @original_Key) AND (WH = @original_WH) AND (WIP = @original_WIP) AND (RW = @original_RW) AND (FGI = @original_FGI) AND (Par_CR = @original_Par_CR) AND (Par_Polish = @original_Par_Polish)">
        <DeleteParameters>
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_ReportKey" Type="DateTime" />
            <asp:Parameter Name="original_ID" Type="String" />
            <asp:Parameter Name="original_WH" Type="Int32" />
            <asp:Parameter Name="original_WIP" Type="Int32" />
            <asp:Parameter Name="original_RW" Type="Int32" />
            <asp:Parameter Name="original_FGI" Type="Int32" />
            <asp:Parameter Name="original_Par_CR" Type="Int32" />
            <asp:Parameter Name="original_Par_Polish" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="WH" Type="Int32" />
            <asp:Parameter Name="WIP" Type="Int32" />
            <asp:Parameter Name="RW" Type="Int32" />
            <asp:Parameter Name="FGI" Type="Int32" />
            <asp:Parameter Name="Par_CR" Type="Int32" />
            <asp:Parameter Name="Par_Polish" Type="Int32" />
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_WH" Type="Int32" />
            <asp:Parameter Name="original_WIP" Type="Int32" />
            <asp:Parameter Name="original_RW" Type="Int32" />
            <asp:Parameter Name="original_FGI" Type="Int32" />
            <asp:Parameter Name="original_Par_CR" Type="Int32" />
            <asp:Parameter Name="original_Par_Polish" Type="Int32" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="ReportKey" Type="DateTime" />
            <asp:Parameter Name="ID" Type="String" />
            <asp:Parameter Name="WH" Type="Int32" />
            <asp:Parameter Name="WIP" Type="Int32" />
            <asp:Parameter Name="RW" Type="Int32" />
            <asp:Parameter Name="FGI" Type="Int32" />
            <asp:Parameter Name="Par_CR" Type="Int32" />
            <asp:Parameter Name="Par_Polish" Type="Int32" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="NotesSqlDataSource" runat="server" ConflictDetection="CompareAllValues"
        ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" DeleteCommand="DELETE FROM dbo.T_SAR_Notes WHERE ([key] = @original_key) AND (Note = @original_Note)"
        InsertCommand="INSERT INTO [T_SAR_Notes] ([ReportKey], [ID], [Note]) VALUES (@ReportKey, @ID, @Note)"
        OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [key], ReportKey, ID, Note FROM dbo.T_SAR_Notes WHERE (ReportKey = CONVERT (DATETIME, '2007-06-01 00:00:00', 102)) AND (ID = '')"
        UpdateCommand="UPDATE dbo.T_SAR_Notes SET Note = @Note WHERE ([key] = @original_key) AND (Note = @original_Note)">
        <DeleteParameters>
            <asp:Parameter Name="original_key" Type="Int32" />
            <asp:Parameter Name="original_Note" Type="String" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Note" Type="String" />
            <asp:Parameter Name="original_key" Type="Int32" />
            <asp:Parameter Name="original_Note" Type="String" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="ReportKey" Type="DateTime" />
            <asp:Parameter Name="ID" Type="String" />
            <asp:Parameter Name="Note" Type="String" />
        </InsertParameters>
    </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
</asp:Content>

