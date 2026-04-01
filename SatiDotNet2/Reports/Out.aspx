<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="Out.aspx.vb" Inherits="Reports_Out" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePane" runat="server">
        <ContentTemplate>
            <div>

                <h1>Surfscan Out</h1>
                <asp:Panel ID="Panel1" runat="server">
                    Select a Range:
                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True">

                            <asp:ListItem Selected="True">Daily</asp:ListItem>
                            <asp:ListItem>Weekly</asp:ListItem>

                        </asp:DropDownList>

                    <asp:Chart ID="Chart1" runat="server" BorderlineColor="Black" BorderlineDashStyle="Solid" ImageType="Jpeg" Width="1000px" Height="600px">
                        <Titles>
                            <asp:Title Text="Wafers Out" Font="Microsoft Sans Serif, 20pt"></asp:Title>
                        </Titles>
                        <Legends>
                            <asp:Legend Docking="Top" Alignment="Center" LegendItemOrder="ReversedSeriesOrder" />
                        </Legends>
                        <Series>
                            <asp:Series Name="Series200mmTarget" ChartType="Line" Color="Blue" LegendText="200mmTarget" YValueMembers="1" YValueType="Auto" LabelBorderWidth="5" XValueType="DateTime" IsXValueIndexed="True" IsValueShownAsLabel="false" BorderDashStyle="Dash" BorderWidth="3"></asp:Series>
                            <asp:Series Name="Series200mmOut" ChartType="Line" Color="Orange" LegendText="200mmOut" YValueMembers="1" YValueType="Auto" LabelBorderWidth="1" XValueType="DateTime" IsXValueIndexed="True" IsValueShownAsLabel="True" BorderWidth="3" MarkerStyle="Circle" MarkerSize="10" LabelBackColor="#E8E8E8"></asp:Series>
                            <asp:Series Name="Series300mmTarget" ChartType="Line" Color="Purple" LegendText="300mmTarget" YValueMembers="1" YValueType="Auto" LabelBorderWidth="1" XValueType="DateTime" IsXValueIndexed="True" IsValueShownAsLabel="false" BorderDashStyle="Dash" BorderWidth="3"></asp:Series>
                            <asp:Series Name="Series300mmOut" ChartType="Line" Color="SeaGreen" LegendText="300mmOut" YValueMembers="1" YValueType="Auto" LabelBorderWidth="1" XValueType="DateTime" IsXValueIndexed="True" IsValueShownAsLabel="True" BorderWidth="3" MarkerStyle="Circle" MarkerSize="10" MarkerColor="SeaGreen" LabelBackColor="#E8E8E8"></asp:Series>

                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                    <br />
                    <br />



                </asp:Panel>
                <asp:Timer ID="MyTimer" runat="server" Interval="300000"></asp:Timer>
            </div>
        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>

