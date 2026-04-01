<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SMR_Parts.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <script type="text/javascript">
                document.body.style.visibility = "hidden";
                document.body.style.background = "white";

                window.addEventListener("load", function () {
                    iframePrep();
                })

                function iframePrep() {
                    document.getElementById("ctl00_MasterPagePanelTop").style.display = "none"; //hide header
                    document.getElementById("ctl00_MasterPagePanelBottom").style.display = "none"; //hide footer
                    document.getElementById("ctl00_MasterPagePanel").style.minWidth = "unset"; //prevent min-width on div with id of 'ctl00_MasterPagePanel'

                    //modify styles placed on html body
                    document.body.style.background = "none";
                    document.body.style.margin = "0";

                    document.body.style.visibility = "visible";
                }

            </script>

            <style>
                :root {
                    --UWhitespace: 0.5em;
                    --UFontSize: (calc(var(--UWhitespace) * 2))
                }

                #add-part-and-part-cart-split {
                    display: flex;
                    justify-content: center;
                }

                #create-and-view-parts-section {
                    display: flex;
                    flex-direction: column;
                    align-items: anchor-center;
                }

                /* =========== add-part-section ============*/
                .add-part-section {
                    width: fit-content;
                    display: flex;
                    gap: var(--UWhitespace);
                    flex-direction: column;
                    border: 4px solid #80BEFD;
                }

                #add-part-body-section {
                    width: fit-content;
                    display: flex;
                    gap: var(--UWhitespace);
                    flex-direction: column;
                    padding: var(--UWhitespace);
                }

                #add-part-textbox-section {
                    display: grid;
                    grid-template-columns: 1fr 1fr 1fr;
                    gap: var(--UWhitespace);
                }

                    #add-part-textbox-section input[type="text"], .add-part-procured-select {
                        border: 2px solid black;
                    }

                .add-part-section-container {
                    display: flex;
                    flex-direction: column;
                }

                #add-part-footer-section {
                    text-align: right;
                }

                #add-part-header-section {
                    background-color: #80BEFD;
                    color: white;
                }

                #add-part-header-label {
                    margin: var(--UWhitespace);
                }

                .ClearCreatePartSection_Button, .AddCreatePartSection_Button {
                    border: 2px solid #80BEFD;
                    padding: var(--UWhitespace);
                    cursor: pointer;
                }

                .ClearCreatePartSection_Button {
                    background-color: white;
                    color: black;
                }

                .AddCreatePartSection_Button {
                    background-color: #80BEFD;
                    color: white;
                }

                .CreatePartError_Label {
                    color: red;
                }

                /*========== parts-tables-section ============*/
                #parts-tables-section {
                    display: flex;
                    align-items: center;
                    gap: var(--UWhitespace);
                }

                .parts-table-title {
                    margin: var(--UWhitespace) 0;
                }

                .maint-table-edit-mode-qty-textbox {
                    width: 25px;
                }
            </style>

            <section id="add-part-and-part-cart-split">

                <section id="create-and-view-parts-section">
                    <asp:Panel runat="server" ID="AddPartPanel" CssClass="add-part-section">
                        <section id="add-part-header-section">
                            <h3 id="add-part-header-label">Add Part To Order</h3>
                        </section>
                        <section id="add-part-body-section">
                            <section id="add-part-textbox-section">
                                <div id="add-part-vendor-or-manuf-container" class="add-part-section-container">
                                    <span id="add-part-vendor-or-manuf-label">Vendor Or Manufacturer</span>
                                    <asp:TextBox ID="VendorOrManufacturer_TextBox" runat="server" />
                                </div>
                                <div id="add-part-part-name-container" class="add-part-section-container">
                                    <span id="add-part-part-name-label">Part Name</span>
                                    <asp:TextBox ID="PartDescription_TextBox" runat="server" />
                                </div>
                                <div id="add-part-qty-container" class="add-part-section-container">
                                    <span id="add-part-qty-label">Quantity</span>
                                    <asp:TextBox ID="Qty_TextBox" runat="server" />
                                </div>
                                <div id="add-part-pw-part-num-container" class="add-part-section-container">
                                    <span id="add-part-pw-part-num-label">Pure Wafer Part #</span>
                                    <asp:TextBox ID="PW_PartNum_TextBox" runat="server" />
                                </div>
                                <div id="add-part-vendor-part-num-container" class="add-part-section-container">
                                    <span id="add-part-vendor-part-num-label">Vendor Part #</span>
                                    <asp:TextBox ID="Vendor_PartNum_TextBox" runat="server" />
                                </div>
                                <div id="add-part-procured-container" class="add-part-section-container">
                                    <span id="add-part-procured-label">Procured</span>
                                    <asp:DropDownList CssClass="add-part-procured-select" ID="Procured_TextBox" runat="server">
                                        <asp:ListItem Text="True" Value="True" />
                                        <asp:ListItem Text="False" Value="False" Selected="True" />
                                    </asp:DropDownList>
                                </div>
                            </section>
                            <section id="add-part-footer-section">
                                <asp:Label ID="CreatePartError_Label" CssClass="CreatePartError_Label" Text="" runat="server" />
                                <asp:Button ID="ClearCreatePartSection_Button" CssClass="ClearCreatePartSection_Button" Text="Clear" runat="server" />
                                <asp:Button ID="AddCreatePartSection_Button" CssClass="AddCreatePartSection_Button" Text="Add" runat="server" />
                            </section>
                        </section>
                    </asp:Panel>

                    <section id="parts-tables-section">
                        <div id="maint-table-container">
                            <h3 id="maint-table-title" class="parts-table-title">Parts To Order</h3>
                            <asp:GridView ID="Parts_GridView" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                CellPadding="4" DataKeyNames="PartToOrder_Key,SMR_Key" DataSourceID="PartsSqlDataSource" ForeColor="Black" Style="border-right: thin solid; border-top: thin solid; border-left: thin solid; border-bottom: thin solid" BackColor="#CCCCCC" BorderColor="#999999" BorderStyle="Solid" BorderWidth="3px" CellSpacing="2">
                                <FooterStyle BackColor="#CCCCCC" />
                                <RowStyle BackColor="White" />
                                <Columns>
                                    <%--<asp:BoundField DataField="ManufacturerOrVendor" HeaderText="Manufacturer/Vendor" SortExpression="Manufacturer/Vendor" ReadOnly="True" />--%>
                                    <asp:TemplateField HeaderText="Manufacturer/Vendor" SortExpression="ManufacturerOrVendor">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="ManufacturerOrVendor_TextBox" runat="server" Text='<%# Bind("ManufacturerOrVendor") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="ManufacturerOrVendor_Label" runat="server" Font-Bold="True" Text='<%# Eval("ManufacturerOrVendor") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Part Description" SortExpression="PartDescription">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="PartDescription_TextBox" runat="server" Text='<%# Bind("PartDescription") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="PartDescription_Label" runat="server" Font-Bold="True" Text='<%# Eval("PartDescription") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Qty" SortExpression="Qty">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="Qty_TextBox" CssClass="maint-table-edit-mode-qty-textbox" runat="server" Text='<%# Bind("Qty") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Qty_Label" runat="server" Font-Bold="True" Text='<%# Eval("Qty") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="PW Part #" SortExpression="PW_PartNum">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="PW_PartNum_TextBox" runat="server" Text='<%# Bind("PW_PartNum") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="PW_PartNum_Label" runat="server" Font-Bold="True" Text='<%# Eval("PW_PartNum") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Vendor Part #" SortExpression="Vendor_PartNum">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="Vendor_PartNum_TextBox" runat="server" Text='<%# Bind("Vendor_PartNum") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Vendor_PartNum_Label" runat="server" Font-Bold="True" Text='<%# Eval("Vendor_PartNum") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Procured" SortExpression="Procured">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="Procured_DropDownList" SelectedValue='<%# Bind("Procured") %>' runat="server">
                                                <asp:ListItem Text="True" Value="True" />
                                                <asp:ListItem Text="False" Value="False" />
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Procured_Label" runat="server" Font-Bold="True" Text='<%# Eval("Procured") %>'></asp:Label>
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
                            <asp:SqlDataSource ID="PartsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                                UpdateCommand="UPDATE T_SMR_PartToOrder SET ManufacturerOrVendor=@ManufacturerOrVendor, PartDescription=@PartDescription, Qty=@Qty, PW_PartNum=@PW_PartNum, Vendor_PartNum=@Vendor_PartNum, Procured=@Procured WHERE PartToOrder_Key=@PartToOrder_Key">

                                <UpdateParameters>
                                    <asp:Parameter Name="ManufacturerOrVendor" Type="String" />
                                    <asp:Parameter Name="PartDescription" Type="String" />
                                    <asp:Parameter Name="Qty" Type="Int16" />
                                    <asp:Parameter Name="Procured" Type="Boolean" />
                                    <asp:Parameter Name="PW_PartNum" Type="String" />
                                    <asp:Parameter Name="Vendor_PartNum" Type="String" />
                                </UpdateParameters>
                            </asp:SqlDataSource>
                        </div>
                    </section>
                </section>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
