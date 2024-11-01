<%@ Page Title="Non-Conforming Skid Builder" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="NonConformingSkidBuilder.aspx.vb" Inherits="PC_NonConformingSkidBuilder" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content
    ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="MainUpdatePanel" runat="server">
        <ContentTemplate>
            <asp:Panel ID="TitlePanel" runat="server" HorizontalAlign="Center" Height="50px">
                <asp:Label ID="TitleLabel" runat="server" Text="Non-Conforming Skid Builder" Font-Size="XX-Large" Font-Bold="true"></asp:Label>
            </asp:Panel>
            <asp:Panel ID="GenPanel" runat="server" Width="1008px" BackColor="White" Height="57px">
                <table>
                    <tr>
                        <td class="auto-style1" style="width: 1008px; height: 50px;">
                            <asp:Panel ID="GenLabelPanel" runat="server" Width="1008px">
                                <table>
                                    <tr>
                                        <td style="width: 442px">
                                            <asp:Panel ID="GenLabelLabelPanel" runat="server" HorizontalAlign="Center" Width="500px">
                                                <asp:Label ID="GenLabel" runat="server" Text="Generate a new Skid?" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                            </asp:Panel>
                                        </td>
                                        <td style="width: 500px">
                                            <asp:Panel ID="GenLabelButtonPanel" runat="server" HorizontalAlign="Center" Width="420px" DefaultButton="GenButton" AutoPostBack="True">
                                                <asp:DropDownList ID="GenPrinterlist" runat="server" Width="100px">
                                                    <asp:ListItem>Zebra4</asp:ListItem>
                                                    <asp:ListItem>Zebra6</asp:ListItem>
                                                    <asp:ListItem Selected="True">Zebra8</asp:ListItem>
                                                </asp:DropDownList>
                                                &nbsp;&nbsp;
                                                <asp:Button ID="GenButton" runat="server" Text="Generate New Skid" Width="250px" Height="35px" Font-Bold="true" Font-Size="Large" Font-Family="Times New Roman" BackColor="#00cc00" AutoPostBack="True" />
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="SkidBuilderPanel" runat="server" BackColor="LightGray" Width="1008px" Height="350px">
                <table style="width: 1000px; height: 348px;">
                    <tr>
                        <td style="width: 440px; height: 360px;" class="auto-style1">
                            <asp:Panel ID="LeftPanel" runat="server" Height="350px">
                                <asp:Panel ID="OtherwisePanel" runat="server" Height="50px" Width="430px">
                                    <table>
                                        <tr>
                                            <td style="padding-left: 25px; height: 50px;">
                                                <asp:Label ID="OtherwiseLabel0" runat="server" Text="Otherwise, scan the current SKID ID  " Font-Bold="true" Font-Size="Large"></asp:Label>
                                                <asp:Label ID="OtherwiseLAbel1" runat="server" Text="below." Font-Bold="true" Font-Size="Large" ForeColor="Goldenrod"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>

                                </asp:Panel>
                                <asp:Panel ID="OtherwisePanel1" runat="server" Height="53px" Width="430px" HorizontalAlign="Center">
                                    <asp:Label ID="ArrowErrorLabel" runat="server" Text="&#x2193;" Font-Bold="true" Font-Size="XX-Large" ForeColor="Goldenrod"></asp:Label>
                                </asp:Panel>
                                <asp:Panel ID="SkidInputPanel" runat="server" Height="160px" DefaultButton="SkidSearch" Width="430px">
                                    <table>
                                        <tr>
                                            <td style="padding-left: 25px; width: 386px;">
                                                <asp:Label ID="ScanSkidLabel" runat="server" Text="Skid Input:" Width="120px" Font-Bold="True" Font-Size="Large"></asp:Label>
                                                <asp:TextBox ID="ScanSkid" runat="server" Width="115px"></asp:TextBox>
                                                &nbsp;&nbsp;
                                                <asp:Button ID="SkidSearch" runat="server" Text="Search" Height="22px" Width="101px" AutoPostBack="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-left: 25px; width: 386px;">
                                                <asp:Label ID="ActSkid" runat="server" Text="Active Skid:" Width="121px" Font-Size="Large"></asp:Label>
                                                <asp:Label ID="SkidNum" runat="server" Text="--" Font-Size="Large" Width="121px" BackColor="Yellow"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="ExcelPanel" runat="server" Height="70px" Width="430px">
                                    <table style="width: 420px; height: 50px; padding-left: 25px">
                                        <tr>
                                            <td style="width: 247px; height: 30px;">
                                                <asp:Label ID="ExportLabel" runat="server" Text="Export Table To Excel Document?"></asp:Label>
                                                &nbsp;&nbsp;
                                            </td>
                                            <td style="width: 130px; height: 30px;">
                                                <asp:DropDownList ID="SizeSheet" runat="server" Height="22px" Width="60px">
                                                    <asp:ListItem Value="Size?">Size?</asp:ListItem>
                                                    <asp:ListItem>300mm</asp:ListItem>
                                                    <asp:ListItem>200mm</asp:ListItem>
                                                </asp:DropDownList>&nbsp;
                                                <asp:Button ID="ExportButton" runat="server" Text="Export" Height="22px" Width="60px" Style="margin-left: 0px" AutoPostBack="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 247px; height: 50px;">
                                                <asp:HyperLink ID="ViewExcelFile" runat="server" Visible="false" AutoPostBack="true" ForeColor="Blue">Download Excel File Here</asp:HyperLink>
                                            </td>
                                            <td style="width: 130px; height: 50px;">
                                                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                                    <ProgressTemplate>
                                                        &nbsp;<img src="../Color/Animated_LoadingBigger.gif" />
                                                        &nbsp;&nbsp;&nbsp;Loading...
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:Panel>
                        </td>
                        <td style="padding-left: 0px; width: 511px; height: 360px;">
                            <asp:Panel ID="GridHeaderPanel" runat="server" Style="padding-top: 3px" BackColor="#507CD1" Width="526px">
                                <asp:Label ID="GridHeader1" runat="server" Text="NC_Skid" Font-Bold="True" BackColor="#507CD1" ForeColor="White" Width="105px" Height="23px"></asp:Label>
                                <asp:Label ID="GridHeader2" runat="server" Text="NC_Inv_Box" Font-Bold="True" BackColor="#507CD1" ForeColor="White" Width="145px" Height="23px"></asp:Label>
                                <asp:Label ID="GridHeader3" runat="server" Text="Qty" Font-Bold="True" BackColor="#507CD1" ForeColor="White" Width="80px" Height="23px"></asp:Label>
                                <asp:Label ID="GridHeader4" runat="server" Text="TimeStamp" Font-Bold="True" BackColor="#507CD1" ForeColor="White" Width="115px" Height="23px"></asp:Label>
                            </asp:Panel>
                            <asp:Panel ID="GridViewPanel" runat="server" Height="260px" ScrollBars="Auto" Width="525px" AutoPostBack="true">
                                <asp:GridView ID="SkidViewer" runat="server" AutoGenerateColumns="False" DataKeyNames="Key" DataSourceID="SqlDataSourceSkidBox" Width="508px" CellPadding="4" ForeColor="#333333" GridLines="None" ShowHeader="False">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="NC_Skid" HeaderText="NC_Skid" SortExpression="NC_Skid" ReadOnly="true">
                                            <ItemStyle Width="125px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="NC_Inv_Box" HeaderText="NC_Inv_Box" SortExpression="NC_Inv_Box">
                                            <HeaderStyle Width="75px" />
                                            <ItemStyle Width="125px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" ReadOnly="true">
                                            <ItemStyle Width="50px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TimeStamp" HeaderText="TimeStamp" SortExpression="TimeStamp" DataFormatString="{0:g}">
                                            <ItemStyle Width="150px" />
                                        </asp:BoundField>
                                        <asp:CommandField CancelText="X" DeleteText="X" EditText="&amp;#128393" UpdateText="&amp;#10003" ShowCancelButton="True" ShowDeleteButton="True" ShowEditButton="true">
                                            <ItemStyle Width="10px" ForeColor="Red" />
                                        </asp:CommandField>
                                    </Columns>
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
                                <asp:Panel ID="BoxInputPanel" runat="server" DefaultButton="Addbox">
                                    <table style="height: 34px; background-color: #507CD1; width: 508px;">
                                        <tr>
                                            <td style="width: 260px; height: 30px;">
                                                <asp:Label ID="ScanBoxLabel" runat="server" Text="Box Input:" Width="90px" Font-Bold="True" Font-Size="Medium"></asp:Label>
                                                <asp:TextBox ID="ScanBox" runat="server" Width="90px"></asp:TextBox>
                                                &nbsp;&nbsp;
                                                <asp:Button ID="AddBox" runat="server" Text="ADD" Height="20px" AutoPostBack="true" />
                                            </td>
                                            <td style="width: 200px; height: 30px;">
                                                <asp:Label ID="LastBox" runat="server" Text="Last Box Scan:" Width="115px" Font-Size="Medium"></asp:Label>
                                                <asp:Label ID="BoxNum" runat="server" Text="--" Font-Size="Large" BackColor="LightBlue" Width="80px"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:SqlDataSource ID="SqlDataSourceSkidBox" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT [Key], NC_Skid, NC_Inv_Box, (SELECT SUM(Qty) AS Expr1 FROM T_NC_Box_Qty WHERE (NC_Inv_Box = T_NC_Skid_BoxInv.NC_Inv_Box)) AS Qty, TimeStamp FROM T_NC_Skid_BoxInv WHERE (NC_Skid = 1)" DeleteCommand="DELETE FROM [T_NC_Skid_BoxInv] WHERE [Key] = @Key" InsertCommand="INSERT INTO [T_NC_Skid_BoxInv] ([NC_Skid], [NC_Inv_Box], [TimeStamp]) VALUES (@NC_Skid, @NC_Inv_Box, @TimeStamp)" UpdateCommand="UPDATE [T_NC_Skid_BoxInv] SET [NC_Inv_Box] = @NC_Inv_Box, [TimeStamp] = @TimeStamp WHERE [Key] = @Key">
                <DeleteParameters>
                    <asp:Parameter Name="Key" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="NC_Skid" Type="Int32" />
                    <asp:Parameter Name="NC_Inv_Box" Type="Int32" />
                    <asp:Parameter Name="TimeStamp" Type="DateTime" />
                </InsertParameters>
                <SelectParameters>
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="NC_Skid" Type="Int32" />
                    <asp:Parameter Name="NC_Inv_Box" Type="Int32" />
                    <asp:Parameter Name="TimeStamp" Type="DateTime" />
                    <asp:Parameter Name="Key" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
