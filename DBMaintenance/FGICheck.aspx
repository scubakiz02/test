<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="FGICheck.aspx.vb" Inherits="DBMaintenance_FGICheck" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <script type="text/javascript">
    function RefreshUpdatePanel() {
        __doPostBack('<%= TextBox1.ClientID %>', '');
    };
    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="915px">
                FGI List<br />
                <br />Reprint printer :
                    <asp:DropDownList id="PrinterDropDownList" runat="server" Width="160px"><asp:ListItem>Select Printer...</asp:ListItem>
                    <asp:ListItem>Zebra4</asp:ListItem>
                    <asp:ListItem>Zebra5</asp:ListItem>
                    <asp:ListItem Selected="True">Zebra6</asp:ListItem>
                    <asp:ListItem>Zebra7</asp:ListItem>
                    </asp:DropDownList>
                    <br />
                <table>
                    <tr>
                        <td style="vertical-align: top; width: 100px; text-align: left">
                            <asp:Panel ID="Panel2" runat="server" Width="450px">
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SystemFGISqlDataSource" Width="424px" CellPadding="4" ForeColor="#333333" GridLines="None" AllowSorting="True">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                                        <asp:BoundField DataField="FGI" HeaderText="FGI" SortExpression="FGI" />
                                        <asp:BoundField DataField="RecordNumber" HeaderText="RCN" SortExpression="RecordNumber" />
                                        <asp:BoundField DataField="SO" HeaderText="SO" SortExpression="SO" />
                                        <asp:BoundField DataField="Carton_Key" HeaderText="CK#" SortExpression="Carton_Key" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox1" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:ButtonField ButtonType="Button" CommandName="Clear" Text="Remove" />
                                        <asp:TemplateField HeaderText="Found">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox2" runat="server" Text="Found" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:ButtonField ButtonType="Button" CommandName="reprint" Text="Reprint" />
                                    </Columns>
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <AlternatingRowStyle BackColor="LightBlue" />
                                </asp:GridView>
                                <br />
                            </asp:Panel>               
                        </td>
                       
                        <td style="vertical-align: top">
                            Scan in box to find:<br />
                            <asp:TextBox ID="TextBox1" runat="server" onkeyup="RefreshUpdatePanel();" TextMode="MultiLine" Height="100px" AutoPostBack="True" Font-Size="XX-Large" Font-Bold="True"></asp:TextBox>
                            
                        </td>
                    </tr>
                </table>

                <asp:SqlDataSource ID="SystemFGISqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"                    
                                SelectCommand="SELECT ID, FGI, RecordNumber, SO, Carton_Key FROM Q_FGI_W_KEY ORDER BY ID">
                </asp:SqlDataSource>


            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="TextBox1" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
</asp:Content>

