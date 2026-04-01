<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SMR_Shipping.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <script type="text/javascript">
</script>

            <style>
                :root {
                    --UWhitespace: 0.5em;
                    --UFontSize: (calc(var(--UWhitespace) * 2))
                }

                /* ============ smr-select-section ============== */
                #smr-select-section {

                }

                /* ============ ShippingGridView =============*/
                .center-align-column {
                    text-align: center;
                }

                .cell-width-cutoff {
                    max-width: 250px;
                    text-wrap: nowrap;
                    overflow: hidden;
                    text-overflow: ellipsis;
                }
                
            </style>

            <h2 id="smr-shipping-title" style="margin: var(--UWhitespace) 0;">Shipping: Parts To Order</h2>

            <section id="smr-select-section">
                <span id="smr-select-label">Select SMR:</span>

                <%--value for asp listitem 'All' is 0, for select query purposes--%>
                <asp:DropDownList ID="ShippingDropDownList" runat="server"
                    AppendDataBoundItems="True" AutoPostBack="True"
                    DataSourceID="ShippingDropDownList_SqlDataSource"
                    DataTextField="Note"
                    DataValueField="SMR_Key">
                    <asp:ListItem Text="..." Value=""/>
                    <asp:ListItem Text="All" Value="0" />
                </asp:DropDownList>

                <%-- not every SMR needs parts to be ordered
            make sure the ddl options are where maintenance tech has voiced parts need to be ordered--%>
                <asp:SqlDataSource ID="ShippingDropDownList_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT T_SMR_Tickets.SMR_Key, T_SMR_TicketNotes.Note FROM T_SMR_TicketNotes INNER JOIN
                T_SMR_Tickets ON T_SMR_TicketNotes.SMR_Key = T_SMR_Tickets.SMR_Key WHERE T_SMR_Tickets.OrderParts=1 ORDER BY T_SMR_Tickets.IssueDate"></asp:SqlDataSource>

            </section>
            <asp:GridView ID="ShippingGridView" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                CellPadding="4" DataKeyNames="PrimaryKey" DataSourceID="ShippingGridView_SqlDataSource" ForeColor="Black" Style="border-right: thin solid; border-top: thin solid; border-left: thin solid; border-bottom: thin solid" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellSpacing="2">
                <FooterStyle BackColor="#CCCCCC" />
                <RowStyle BackColor="White" />
                <Columns>
                    <asp:TemplateField HeaderText="SMR Description" SortExpression="Note">
                        <ItemTemplate>
                            <section id="smr-description-section" class="cell-width-cutoff">
                                <asp:Label ID="Note_Label" runat="server" Font-Bold="True" Text='<%# Eval("Note") %>' ToolTip='<%# Eval("Note") %>'></asp:Label>
                            </section>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Tech" HeaderText="Tech" SortExpression="Tech" ReadOnly="True" />
                    <asp:BoundField DataField="PartDescription" HeaderText="Part" SortExpression="PartDescription" ReadOnly="True" />
                    <asp:BoundField DataField="ManufacturerOrVendor" HeaderText="Manufacturer/Vendor" SortExpression="ManufacturerOrVendor" ReadOnly="True" />
                    <asp:BoundField DataField="PW_PartNum" HeaderText="PW Part #" SortExpression="PW_PartNum" ReadOnly="True" />
                    <asp:BoundField DataField="Vendor_PartNum" HeaderText="Vendor Part #" SortExpression="Vendor_PartNum" ReadOnly="True" />
                    <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" ReadOnly="True">
                        <ItemStyle CssClass="center-align-column" />
                    </asp:BoundField>

                    <asp:TemplateField HeaderText="Status" SortExpression="ShippingStatus">
                        <EditItemTemplate>
                            <asp:TextBox ID="ShippingStatus_TextBox" runat="server" Text='<%# Bind("ShippingStatus") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="ShippingStatus_Label" runat="server" Font-Bold="True" Text='<%# Eval("ShippingStatus") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="ETA" SortExpression="ExpectedDeliveryDate">
                        <EditItemTemplate>
                            <asp:TextBox ID="ExpectedDeliveryDate_TextBox" runat="server" Text='<%# Bind("ExpectedDeliveryDate") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="ExpectedDeliveryDate_Label" runat="server" Font-Bold="True" Text='<%# Eval("ExpectedDeliveryDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="PO#" SortExpression="PO_Num">
                        <EditItemTemplate>
                            <asp:TextBox ID="PO_Num_TextBox" runat="server" Text='<%# Bind("PO_Num") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="PO_Num_Label" runat="server" Font-Bold="True" Text='<%# Eval("PO_Num") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:CommandField ShowEditButton="True" />
                </Columns>
                <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#808080" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#383838" />
            </asp:GridView>
            <asp:SqlDataSource ID="ShippingGridView_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                UpdateCommand="UPDATE [ALTS].[dbo].[T_SMR_PartToOrder] SET ShippingStatus=@ShippingStatus, ExpectedDeliveryDate=@ExpectedDeliveryDate, PO_Num=@PO_Num WHERE PartToOrder_Key=@PrimaryKey">

                <UpdateParameters>
                    <asp:Parameter Name="ShippingStatus" Type="String" />
                    <asp:Parameter Name="ExpectedDeliveryDate" Type="DateTime" />
                    <asp:Parameter Name="PO_Num" Type="String" />
                </UpdateParameters>
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
