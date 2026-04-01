<%@ Page Title="Non-Comforming Packing" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="NonConformingPacking.aspx.vb" Inherits="PC_NonConformingPacking" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content 
    ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="TitlePanel" runat="server" HorizontalAlign="Center" Height="50px">
                <asp:Label ID="TitleLabel" runat="server" Text="Non-Conforming Box Packer" Font-Size="XX-Large" Font-Bold="true"></asp:Label>
            </asp:Panel>
            <asp:Panel ID="TempPanel" runat="server" Width="1008px" BackColor="White" Height="57px">
                <table>
                    <tr>
                        <td class="auto-style1" style="width: 1008px; height: 50px;"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel1" runat="server" BackColor="LightGray" Width="1008px">
            
                <table class="style1">
                    <tr>
                        <td style="height: 70px; width: 300px;">
                            <table style="height: 64px">
                                <tr>
                                    <td style="width: 180px">
                                        ID:&nbsp;</td>
                                    <td style="width: 120px">
                                        Pack With ID:&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 180px; height: 35px;">
                                        <asp:TextBox ID="TextBoxID" runat="server" AutoPostBack="True" Width="151px" Height="20px"></asp:TextBox>&nbsp;</td>
                                    <td style="width: 120px; height: 35px; align-content:center">
                                        &nbsp<asp:Label ID="LabelPackWith" runat="server" Text="0000" Font-Size="X-Large" BackColor="Yellow"></asp:Label>&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 15px"></td>
                        <td rowspan="3">
                            <asp:GridView ID="GridViewBoxes" runat="server" CellPadding="4" ForeColor="#333333" 
                                GridLines="None" AutoGenerateColumns="False" 
                                DataSourceID="SqlDataSourceOpenInv" Width="665px">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:ButtonField CommandName="CloseBox" Text="Close" />
                                    <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                                    <asp:BoundField DataField="NC_Inv_Box" HeaderText="NC_Inv_Box" 
                                        InsertVisible="False" ReadOnly="True" SortExpression="NC_Inv_Box" />
                                    <asp:BoundField DataField="MainID" HeaderText="MainID" 
                                        SortExpression="MainID" />
                                    <asp:BoundField DataField="Total" HeaderText="Total" ReadOnly="True" 
                                        SortExpression="Total" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:TextBox ID="TextBoxQty" runat="server" 
                                                ontextchanged="TextBoxQty_TextChanged"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:ButtonField ButtonType="Button" CommandName="AddToBox" Text="Add" />
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </td>    
                    </tr>
                    <tr>
                        <td style="height: 140px; width: 299px;">
                            Packing Notes:<br />
                            <asp:TextBox ID="TextBoxPackingNote" runat="server" Height="114px" Width="300px" 
                                TextMode="MultiLine" Font-Size="Large" Font-Bold="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 300px">
                            <table style="width:306px; height: 86px;">
                                <tr>
                                    <td style="height: 20px">
                                        Make New Box:&nbsp;<br /> 
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 26px">
                                        <asp:DropDownList ID="DropDownListType" runat="server" Width="139px">
                                            <asp:ListItem>NonFilm</asp:ListItem>
                                            <asp:ListItem>Film</asp:ListItem>
                                            <asp:ListItem>Pattern</asp:ListItem>
                                            <asp:ListItem>Copper</asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList ID="DropDownListPrinterlist" runat="server" Width="139px">
                                            <asp:ListItem>Zebra4</asp:ListItem>
                                            <asp:ListItem>Zebra6</asp:ListItem>
                                            <asp:ListItem Selected="True">Zebra8</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 26px">
                                        <asp:Button ID="ButtonNewBox" runat="server" Text="Create With Print" Width="139px" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="ButtonNewBoxNoPrint" runat="server" Text="Create Without Print" Width="139px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                    <asp:UpdateProgress id="UpdateProgress1" runat="server">
                        <progresstemplate>
                        <IMG src="../Color/Animated_LoadingBigger.gif" /> Working...
                        </progresstemplate>
                </asp:UpdateProgress>
                <br \>
                Special Request:<br \>
                <asp:GridView ID="GridViewSpecial" runat="server" CellPadding="4" ForeColor="#333333" 
                    GridLines="None">
                    <RowStyle BackColor="#EFF3FB" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="White" />
                </asp:GridView>
                </asp:Panel>
            <br />
            <asp:SqlDataSource ID="SqlDataSourceOpenInv" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT dbo.T_NC_Box.NC_Inv_Box, dbo.T_NC_Box.MainID, dbo.T_NC_Box.Type, SUM(ISNULL(dbo.T_NC_Box_Qty.Qty, 0)) AS Total FROM dbo.T_NC_Box LEFT OUTER JOIN dbo.T_NC_Box_Qty ON dbo.T_NC_Box.NC_Inv_Box = dbo.T_NC_Box_Qty.NC_Inv_Box WHERE (dbo.T_NC_Box.[Open] = 1) GROUP BY dbo.T_NC_Box.MainID, dbo.T_NC_Box.Type, dbo.T_NC_Box.NC_Inv_Box HAVING (dbo.T_NC_Box.MainID = N'0')"></asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>        
</asp:Content>

