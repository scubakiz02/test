<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SPxChart.aspx.vb" Inherits="Reports_SPxChart" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <asp:UpdatePanel ID="UpdatePane" runat="server">
            <ContentTemplate>
                <div>
                   
                    <h1>SPx Quick View</h1>
                    <asp:Panel ID="Panel1" runat="server">
                        Select a tool:
                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True">
                            <asp:ListItem Selected="True" Value="SP1">SP1-1</asp:ListItem>
                            <asp:ListItem Value="SP2">SP1-2</asp:ListItem>
                            <asp:ListItem>SP1-3</asp:ListItem>
                            <asp:ListItem Value="SP2-S0132">SP2</asp:ListItem>
                            <asp:ListItem Value="SP3-2110224">SP3</asp:ListItem>
                        </asp:DropDownList>   
                    <asp:Chart ID="Chart1" runat="server"  BorderlineColor="Black"  BorderlineDashStyle="Solid" ImageType="Jpeg" RightToLeft="Yes" Width="1000px">
                        <Titles> 
                            <asp:Title Text="SP3"></asp:Title>
                        </Titles>
                        <Legends> <asp:Legend Docking="Top"  Alignment="Center" LegendItemOrder="ReversedSeriesOrder" /></Legends>
                        <Series>
                            <asp:Series Name="Series1" ChartType="StackedColumn" Color="Red"  LegendText="1"  YValueMembers="1" YValueType="Auto" LabelBorderWidth="1" XValueType="DateTime" IsXValueIndexed="True" IsValueShownAsLabel="True"></asp:Series>
                            <asp:Series Name="Series2" ChartType="StackedColumn" Color="Green"  LegendText="2"  YValueMembers="1" YValueType="Auto" LabelBorderWidth="1" XValueType="DateTime" IsXValueIndexed="True" IsValueShownAsLabel="True"></asp:Series>
                            <asp:Series Name="Series3" ChartType="StackedColumn" Color="Yellow"  LegendText="3"  YValueMembers="1" YValueType="Auto" LabelBorderWidth="1" XValueType="DateTime" IsXValueIndexed="True" IsValueShownAsLabel="True"></asp:Series>
                        
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart><br />
                    <br />
                        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                            <AlternatingRowStyle BackColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>
                        

                    </asp:Panel>
                    <asp:Timer ID="MyTimer" runat="server" Interval="300000"></asp:Timer>
                </div>
            </ContentTemplate>

        </asp:UpdatePanel>

</asp:Content>

