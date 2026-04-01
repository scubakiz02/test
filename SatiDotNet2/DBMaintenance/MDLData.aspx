<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="mdlData.aspx.vb" Inherits="DBMaintenance_EditRoles" Title="Untitled Page" %>

<%@ Import Namespace="System.Web.Security" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        window.addEventListener("keypress", function (e) {
            const tbxs = Array.from(document.querySelectorAll('input[type="text"]'))
            const currTbx = e.srcElement;
            const nextTbxIdx = tbxs.indexOf(currTbx) + 1;

            if (e.key === "Enter" && tbxs.includes(currTbx)) {
                e.preventDefault();
                if (nextTbxIdx == tbxs.length) return;

                const nextTbx = tbxs[nextTbxIdx];
                const nextTbxLength = nextTbx.value.length;

                nextTbx.focus();
                nextTbx.setSelectionRange(nextTbxLength, nextTbxLength); //set cursor to end of textbox
            }
        })
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="CreateButton" />
        </Triggers>

        <ContentTemplate>
            <div style="display: flex; align-items: center;">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="MDL Dataset:"></asp:Label>
                <asp:RadioButton ID="CurrRB" runat="server" Checked="True" GroupName="Options" Text="Current" AutoPostBack="True" OnCheckedChanged="RB_StatusChanged" />
                <asp:RadioButton ID="ArchivedRB" runat="server" GroupName="Options" Text="Archived" AutoPostBack="True" OnCheckedChanged="RB_StatusChanged" />
            </div>
            <asp:Panel ID="CurrPanel" runat="server" Width="848px">
                <asp:SqlDataSource ID="SqlDataSourceMDL2" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    DeleteCommand="UPDATE T_Metals_MDL SET ExpireDate=GETDATE() WHERE ExpireDate IS NULL"
                    InsertCommand="INSERT INTO [T_Metals_MDL] ([MDL_User], [EnterDate], [ExpireDate], [Mg], [Ca], [Ma], [Ni], [Zn], [Al], [Fe], [Cr], [Cu], [Na], [K], [Co], [Mn], [Ars], [Se], [Ag], [Pb], [Ti], [Ta], [W], [Au], [Mo], [Zr], [La], [Sr], [Ir], [Pt], [Li], [Ga], [Ba], [V], [Notes]) VALUES (@MDL_User, @EnterDate, @ExpireDate, @Mg, @Ca, @Ma, @Ni, @Zn, @Al, @Fe, @Cr, @Cu, @Na, @K, @Co, @Mn, @Ars, @Se, @Ag, @Pb, @Ti, @Ta, @W, @Au, @Mo, @Zr, @La, @Sr, @Ir, @Pt, @Li, @Ga, @Ba, @V, @Notes)"
                    SelectCommand="SELECT [Key], [MDL_User], [EnterDate], [ExpireDate], Li, Na, Mg, Al, K, Ca, Ti, V, Cr, Mn, Fe, Co, Ni, Cu, Zn, Ga, Ars, Sr, Mo, Ag, Ba, Ta, W, Au, Pb, Ma, Se, Zr, La, Ir, Pt, [Notes] FROM [T_Metals_MDL] WHERE ExpireDate IS NULL"
                    UpdateCommand="UPDATE [T_Metals_MDL] SET [MDL_User] = @MDL_User, [EnterDate] = @EnterDate, [ExpireDate] = @ExpireDate, [Mg] = @Mg, [Ca] = @Ca, [Ma] = @Ma, [Ni] = @Ni, [Zn] = @Zn, [Al] = @Al, [Fe] = @Fe, [Cr] = @Cr, [Cu] = @Cu, [Na] = @Na, [K] = @K, [Co] = @Co, [Mn] = @Mn, [Ars] = @Ars, [Se] = @Se, [Ag] = @Ag, [Pb] = @Pb, [Ti] = @Ti, [Ta] = @Ta, [W] = @W, [Au] = @Au, [Mo] = @Mo, [Zr] = @Zr, [La] = @La, [Sr] = @Sr, [Ir] = @Ir, [Pt] = @Pt, [Li] = @Li, [Ga] = @Ga, [Ba] = @Ba, [V] = @V, [Notes] = @Notes WHERE ExpireDate IS NULL">
                    <DeleteParameters>
                        <asp:Parameter Name="Key" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="MDL_User" Type="String" />
                        <asp:Parameter Name="EnterDate" Type="DateTime" />
                        <asp:Parameter Name="ExpireDate" Type="DateTime" />
                        <asp:Parameter Name="Ca" Type="Double" />
                        <asp:Parameter Name="Mg" Type="Double" />
                        <asp:Parameter Name="Ma" Type="Double" />
                        <asp:Parameter Name="Ni" Type="Double" />
                        <asp:Parameter Name="Zn" Type="Double" />
                        <asp:Parameter Name="Al" Type="Double" />
                        <asp:Parameter Name="Fe" Type="Double" />
                        <asp:Parameter Name="Cr" Type="Double" />
                        <asp:Parameter Name="Cu" Type="Double" />
                        <asp:Parameter Name="Na" Type="Double" />
                        <asp:Parameter Name="K" Type="Double" />
                        <asp:Parameter Name="Co" Type="Double" />
                        <asp:Parameter Name="Mn" Type="Double" />
                        <asp:Parameter Name="Ars" Type="Double" />
                        <asp:Parameter Name="Se" Type="Double" />
                        <asp:Parameter Name="Ag" Type="Double" />
                        <asp:Parameter Name="Pb" Type="Double" />
                        <asp:Parameter Name="Ti" Type="Double" />
                        <asp:Parameter Name="Ta" Type="Double" />
                        <asp:Parameter Name="W" Type="Double" />
                        <asp:Parameter Name="Au" Type="Double" />
                        <asp:Parameter Name="Mo" Type="Double" />
                        <asp:Parameter Name="Zr" Type="Double" />
                        <asp:Parameter Name="La" Type="Double" />
                        <asp:Parameter Name="Sr" Type="Double" />
                        <asp:Parameter Name="Ir" Type="Double" />
                        <asp:Parameter Name="Pt" Type="Double" />
                        <asp:Parameter Name="Li" Type="Double" />
                        <asp:Parameter Name="Ga" Type="Double" />
                        <asp:Parameter Name="Ba" Type="Double" />
                        <asp:Parameter Name="V" Type="Double" />
                        <asp:Parameter Name="Notes" Type="String" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="MDL_User" Type="String" />
                        <asp:Parameter Name="EnterDate" Type="DateTime" />
                        <asp:Parameter Name="ExpireDate" Type="DateTime" />
                        <asp:Parameter Name="Mg" Type="Double" />
                        <asp:Parameter Name="Ca" Type="Double" />
                        <asp:Parameter Name="Ma" Type="Double" />
                        <asp:Parameter Name="Ni" Type="Double" />
                        <asp:Parameter Name="Zn" Type="Double" />
                        <asp:Parameter Name="Al" Type="Double" />
                        <asp:Parameter Name="Fe" Type="Double" />
                        <asp:Parameter Name="Cr" Type="Double" />
                        <asp:Parameter Name="Cu" Type="Double" />
                        <asp:Parameter Name="Na" Type="Double" />
                        <asp:Parameter Name="K" Type="Double" />
                        <asp:Parameter Name="Co" Type="Double" />
                        <asp:Parameter Name="Mn" Type="Double" />
                        <asp:Parameter Name="Ars" Type="Double" />
                        <asp:Parameter Name="Se" Type="Double" />
                        <asp:Parameter Name="Ag" Type="Double" />
                        <asp:Parameter Name="Pb" Type="Double" />
                        <asp:Parameter Name="Ti" Type="Double" />
                        <asp:Parameter Name="Ta" Type="Double" />
                        <asp:Parameter Name="W" Type="Double" />
                        <asp:Parameter Name="Au" Type="Double" />
                        <asp:Parameter Name="Mo" Type="Double" />
                        <asp:Parameter Name="Zr" Type="Double" />
                        <asp:Parameter Name="La" Type="Double" />
                        <asp:Parameter Name="Sr" Type="Double" />
                        <asp:Parameter Name="Ir" Type="Double" />
                        <asp:Parameter Name="Pt" Type="Double" />
                        <asp:Parameter Name="Li" Type="Double" />
                        <asp:Parameter Name="Ga" Type="Double" />
                        <asp:Parameter Name="Ba" Type="Double" />
                        <asp:Parameter Name="V" Type="Double" />
                        <asp:Parameter Name="Notes" Type="String" />
                        <asp:Parameter Name="Key" Type="Int32" />
                    </UpdateParameters>
                </asp:SqlDataSource>

                <div style="display: flex; align-items: baseline; flex-direction: column">
                    <div>
                        <asp:FileUpload ID="Uploader" Visible="False" runat="server" autopostback="true" Width="306px" Height="25px" />
                        <asp:Button ID="CreateButton" Visible="False" runat="server" Text="Upload" Font-Bold="True" OnClick="UploadFile" />
                    </div>
                    <asp:Label ID="ErrorMessage" runat="server" Width="465px" ForeColor="Red" Font-Bold="True" Style="margin-left: 0px"></asp:Label>
                </div>

                <asp:FormView ID="LiveFormView" runat="server" DataKeyNames="Key" DataSourceID="SqlDataSourceMDL2" BorderColor="#5D7B9D"
                    BorderStyle="Double" BorderWidth="5px" CellPadding="10" ForeColor="#333333" Width="415px">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <EditItemTemplate>
                        Key:
        <asp:Label ID="KeyLabel1" runat="server" Text='<%# Eval("Key") %>' />
                        <br />
                        MDL_User:
        <asp:TextBox ID="MDL_UserTextBox" runat="server" Text='<%# Bind("MDL_User") %>' />
                        <br />
                        EnterDate:
        <asp:TextBox ID="EnterDateTextBox" runat="server" Text='<%# Bind("EnterDate") %>' />
                        <br />
                        ExpireDate:
        <asp:TextBox ID="ExpireDateTextBox" runat="server" Text='<%# Bind("ExpireDate") %>' />
                        <br />
                        Li:
        <asp:TextBox ID="LiTextBox" runat="server" Text='<%# Bind("Li") %>' />
                        <br />
                        Na:
        <asp:TextBox ID="NaTextBox" runat="server" Text='<%# Bind("Na") %>' />
                        <br />
                        Mg:
        <asp:TextBox ID="MgTextBox" runat="server" Text='<%# Bind("Mg") %>' />
                        <br />
                        Al:
        <asp:TextBox ID="AlTextBox" runat="server" Text='<%# Bind("Al") %>' />
                        <br />
                        K:
        <asp:TextBox ID="KTextBox" runat="server" Text='<%# Bind("K") %>' />
                        <br />
                        Ca:
        <asp:TextBox ID="CaTextBox" runat="server" Text='<%# Bind("Ca") %>' />
                        <br />
                        Ti:
        <asp:TextBox ID="TiTextBox" runat="server" Text='<%# Bind("Ti") %>' />
                        <br />
                        Cr:
        <asp:TextBox ID="CrTextBox" runat="server" Text='<%# Bind("Cr") %>' />
                        <br />
                        Mn:
        <asp:TextBox ID="MnTextBox" runat="server" Text='<%# Bind("Mn") %>' />
                        <br />
                        Fe:
        <asp:TextBox ID="FeTextBox" runat="server" Text='<%# Bind("Fe") %>' />
                        <br />
                        Co:
        <asp:TextBox ID="CoTextBox" runat="server" Text='<%# Bind("Co") %>' />
                        <br />
                        Ni:
        <asp:TextBox ID="NiTextBox" runat="server" Text='<%# Bind("Ni") %>' />
                        <br />
                        Cu:
        <asp:TextBox ID="CuTextBox" runat="server" Text='<%# Bind("Cu") %>' />
                        <br />
                        Zn:
        <asp:TextBox ID="ZnTextBox" runat="server" Text='<%# Bind("Zn") %>' />
                        <br />
                        Ga:
        <asp:TextBox ID="GaTextBox" runat="server" Text='<%# Bind("Ga") %>' />
                        <br />
                        As:
        <asp:TextBox ID="ArsTextBox" runat="server" Text='<%# Bind("Ars") %>' />
                        <br />
                        Sr:
        <asp:TextBox ID="SrTextBox" runat="server" Text='<%# Bind("Sr") %>' />
                        <br />
                        Mo:
        <asp:TextBox ID="MoTextBox" runat="server" Text='<%# Bind("Mo") %>' />
                        <br />
                        Ag:
        <asp:TextBox ID="AgTextBox" runat="server" Text='<%# Bind("Ag") %>' />
                        <br />
                        Ba:
        <asp:TextBox ID="BaTextBox" runat="server" Text='<%# Bind("Ba") %>' />
                        <br />
                        Ta:
        <asp:TextBox ID="TaTextBox" runat="server" Text='<%# Bind("Ta") %>' />
                        <br />
                        W:
        <asp:TextBox ID="WTextBox" runat="server" Text='<%# Bind("W") %>' />
                        <br />
                        Au:
        <asp:TextBox ID="AuTextBox" runat="server" Text='<%# Bind("Au") %>' />
                        <br />
                        Pb:
        <asp:TextBox ID="PbTextBox" runat="server" Text='<%# Bind("Pb") %>' />
                        <br />
                        Ma:
        <asp:TextBox ID="MaTextBox" runat="server" Text='<%# Bind("Ma") %>' />
                        <br />
                        V:
        <asp:TextBox ID="VTextBox" runat="server" Text='<%# Bind("V") %>' />
                        <br />
                        Se:
        <asp:TextBox ID="SeTextBox" runat="server" Text='<%# Bind("Se") %>' />
                        <br />
                        Zr:
        <asp:TextBox ID="ZrTextBox" runat="server" Text='<%# Bind("Zr") %>' />
                        <br />
                        La:
        <asp:TextBox ID="LaTextBox" runat="server" Text='<%# Bind("La") %>' />
                        <br />
                        Ir:
        <asp:TextBox ID="IrTextBox" runat="server" Text='<%# Bind("Ir") %>' />
                        <br />
                        Pt:
        <asp:TextBox ID="PtTextBox" runat="server" Text='<%# Bind("Pt") %>' />
                        <br />
                        Notes:
        <asp:TextBox ID="NotesTextBox" runat="server" Text='<%# Bind("Notes") %>' />
                        <br />
                        <asp:LinkButton ID="UpdateButton" OnClick="UpdateButton_Click" runat="server" CausesValidation="True" CommandName="Update" Text="Update" />
                        &nbsp;<asp:LinkButton ID="EditCancelButton" OnClick="EditCancelButton_Click" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <div style="display: flex; justify-content: space-between; align-items: center;">
                            <asp:Label runat="server" Font-Italic="true" Font-Size="Small" Text='*Element fields (Ca - V) default to 0.01 if left blank.*' />
                        </div>
                        MDL_User:
        <asp:TextBox ID="MDL_UserTextBox" runat="server" Text='<%# Bind("MDL_User") %>' />
                        <br />
                        EnterDate:
        <asp:TextBox ID="EnterDateTextBox" runat="server" Text='<%# Bind("EnterDate") %>' />
                        <br />
                        ExpireDate:
        <asp:TextBox ID="ExpireDateTextBox" runat="server" Text='<%# Bind("ExpireDate") %>' />
                        <br />
                        Li:
        <asp:TextBox ID="LiTextBox" runat="server" Text='<%# Bind("Li") %>' />
                        <br />
                        Na:
        <asp:TextBox ID="NaTextBox" runat="server" Text='<%# Bind("Na") %>' />
                        <br />
                        Mg:
        <asp:TextBox ID="MgTextBox" runat="server" Text='<%# Bind("Mg") %>' />
                        <br />
                        Al:
        <asp:TextBox ID="AlTextBox" runat="server" Text='<%# Bind("Al") %>' />
                        <br />
                        K:
        <asp:TextBox ID="KTextBox" runat="server" Text='<%# Bind("K") %>' />
                        <br />
                        Ca:
        <asp:TextBox ID="CaTextBox" runat="server" Text='<%# Bind("Ca") %>' />
                        <br />
                        Ti:
        <asp:TextBox ID="TiTextBox" runat="server" Text='<%# Bind("Ti") %>' />
                        <br />
                        Cr:
        <asp:TextBox ID="CrTextBox" runat="server" Text='<%# Bind("Cr") %>' />
                        <br />
                        Mn:
        <asp:TextBox ID="MnTextBox" runat="server" Text='<%# Bind("Mn") %>' />
                        <br />
                        Fe:
        <asp:TextBox ID="FeTextBox" runat="server" Text='<%# Bind("Fe") %>' />
                        <br />
                        Co:
        <asp:TextBox ID="CoTextBox" runat="server" Text='<%# Bind("Co") %>' />
                        <br />
                        Ni:
        <asp:TextBox ID="NiTextBox" runat="server" Text='<%# Bind("Ni") %>' />
                        <br />
                        Cu:
        <asp:TextBox ID="CuTextBox" runat="server" Text='<%# Bind("Cu") %>' />
                        <br />
                        Zn:
        <asp:TextBox ID="ZnTextBox" runat="server" Text='<%# Bind("Zn") %>' />
                        <br />
                        Ga:
        <asp:TextBox ID="GaTextBox" runat="server" Text='<%# Bind("Ga") %>' />
                        <br />
                        As:
        <asp:TextBox ID="ArsTextBox" runat="server" Text='<%# Bind("Ars") %>' />
                        <br />
                        Sr:
        <asp:TextBox ID="SrTextBox" runat="server" Text='<%# Bind("Sr") %>' />
                        <br />
                        Mo:
        <asp:TextBox ID="MoTextBox" runat="server" Text='<%# Bind("Mo") %>' />
                        <br />
                        Ag:
        <asp:TextBox ID="AgTextBox" runat="server" Text='<%# Bind("Ag") %>' />
                        <br />
                        Ba:
        <asp:TextBox ID="BaTextBox" runat="server" Text='<%# Bind("Ba") %>' />
                        <br />
                        Ta:
        <asp:TextBox ID="TaTextBox" runat="server" Text='<%# Bind("Ta") %>' />
                        <br />
                        W:
        <asp:TextBox ID="WTextBox" runat="server" Text='<%# Bind("W") %>' />
                        <br />
                        Au:
        <asp:TextBox ID="AuTextBox" runat="server" Text='<%# Bind("Au") %>' />
                        <br />
                        Pb:
        <asp:TextBox ID="PbTextBox" runat="server" Text='<%# Bind("Pb") %>' />
                        <br />
                        Ma:
        <asp:TextBox ID="MaTextBox" runat="server" Text='<%# Bind("Ma") %>' />
                        <br />
                        V:
        <asp:TextBox ID="VTextBox" runat="server" Text='<%# Bind("V") %>' />
                        <br />
                        Se:
        <asp:TextBox ID="SeTextBox" runat="server" Text='<%# Bind("Se") %>' />
                        <br />
                        Zr:
        <asp:TextBox ID="ZrTextBox" runat="server" Text='<%# Bind("Zr") %>' />
                        <br />
                        La:
        <asp:TextBox ID="LaTextBox" runat="server" Text='<%# Bind("La") %>' />
                        <br />
                        Ir:
        <asp:TextBox ID="IrTextBox" runat="server" Text='<%# Bind("Ir") %>' />
                        <br />
                        Pt:
        <asp:TextBox ID="PtTextBox" runat="server" Text='<%# Bind("Pt") %>' />
                        <br />
                        Notes:
        <asp:TextBox ID="NotesTextBox" runat="server" Text='<%# Bind("Notes") %>' />
                        <br />
                        <asp:LinkButton ID="InsertButton" OnClick="InsertButton_Click" runat="server" CausesValidation="True" CommandName="Insert" Text="Insert" />
                        &nbsp;<asp:LinkButton ID="InsertCancelButton" OnClick="InsertCancelButton_Click" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <div style="height: 21px">
                            Key:
        <asp:Label ID="KeyLabel" runat="server" Text='<%# Eval("Key") %>' />
                        </div>
                        <div style="height: 21px">
                            MDL_User:
        <asp:Label ID="MDL_UserLabel" runat="server" Text='<%# Bind("MDL_User") %>' />
                        </div>
                        <div style="height: 21px">
                            EnterDate:
        <asp:Label ID="EnterDateLabel" runat="server" Text='<%# Bind("EnterDate") %>' />
                        </div>
                        <div style="height: 21px">
                            ExpireDate:
        <asp:Label ID="ExpireDateLabel" runat="server" Text='<%# Bind("ExpireDate") %>' />
                        </div>
                        <div style="height: 21px">
                            Li:
        <asp:Label ID="LiLabel" runat="server" Text='<%# Bind("Li") %>' />
                        </div>
                        <div style="height: 21px">
                            Na:
        <asp:Label ID="NaLabel" runat="server" Text='<%# Bind("Na") %>' />
                        </div>
                        <div style="height: 21px">
                            Mg:
        <asp:Label ID="MgLabel" runat="server" Text='<%# Bind("Mg") %>' />
                        </div>
                        <div style="height: 21px">
                            Al:
        <asp:Label ID="AlLabel" runat="server" Text='<%# Bind("Al") %>' />
                        </div>
                        <div style="height: 21px">
                            K:
        <asp:Label ID="KLabel" runat="server" Text='<%# Bind("K") %>' />
                        </div>
                        <div style="height: 21px">
                            Ca:
        <asp:Label ID="CaLabel" runat="server" Text='<%# Bind("Ca") %>' />
                        </div>
                        <div style="height: 21px">
                            Ti:
        <asp:Label ID="TiLabel" runat="server" Text='<%# Bind("Ti") %>' />
                        </div>
                        <div style="height: 21px">
                            V:
        <asp:Label ID="VLabel" runat="server" Text='<%# Bind("V") %>' />
                        </div>
                        <div style="height: 21px">
                            Cr:
        <asp:Label ID="CrLabel" runat="server" Text='<%# Bind("Cr") %>' />
                        </div>
                        <div style="height: 21px">
                            Mn:
        <asp:Label ID="MnLabel" runat="server" Text='<%# Bind("Mn") %>' />
                        </div>
                        <div style="height: 21px">
                            Fe:
        <asp:Label ID="FeLabel" runat="server" Text='<%# Bind("Fe") %>' />
                        </div>
                        <div style="height: 21px">
                            Co:
        <asp:Label ID="CoLabel" runat="server" Text='<%# Bind("Co") %>' />
                        </div>
                        <div style="height: 21px">
                            Ni:
        <asp:Label ID="NiLabel" runat="server" Text='<%# Bind("Ni") %>' />
                        </div>
                        <div style="height: 21px">
                            Cu:
        <asp:Label ID="CuLabel" runat="server" Text='<%# Bind("Cu") %>' />
                        </div>
                        <div style="height: 21px">
                            Zn:
        <asp:Label ID="ZnLabel" runat="server" Text='<%# Bind("Zn") %>' />
                        </div>
                        <div style="height: 21px">
                            Ga:
        <asp:Label ID="GaLabel" runat="server" Text='<%# Bind("Ga") %>' />
                        </div>
                        <div style="height: 21px">
                            As:
        <asp:Label ID="ArsLabel" runat="server" Text='<%# Bind("Ars") %>' />
                        </div>
                        <div style="height: 21px">
                            Sr:
        <asp:Label ID="SrLabel" runat="server" Text='<%# Bind("Sr") %>' />
                        </div>
                        <div style="height: 21px">
                            Mo:
        <asp:Label ID="MoLabel" runat="server" Text='<%# Bind("Mo") %>' />
                        </div>
                        <div style="height: 21px">
                            Ag:
        <asp:Label ID="AgLabel" runat="server" Text='<%# Bind("Ag") %>' />
                        </div>
                        <div style="height: 21px">
                            Ba:
        <asp:Label ID="BaLabel" runat="server" Text='<%# Bind("Ba") %>' />
                        </div>
                        <div style="height: 21px">
                            Ta:
        <asp:Label ID="TaLabel" runat="server" Text='<%# Bind("Ta") %>' />
                        </div>
                        <div style="height: 21px">
                            W:
        <asp:Label ID="WLabel" runat="server" Text='<%# Bind("W") %>' />
                        </div>
                        <div style="height: 21px">
                            Au:
        <asp:Label ID="AuLabel" runat="server" Text='<%# Bind("Au") %>' />
                        </div>
                        <div style="height: 21px">
                            Pb:
        <asp:Label ID="PbLabel" runat="server" Text='<%# Bind("Pb") %>' />
                        </div>
                        <div style="height: 21px">
                            Ma:
        <asp:Label ID="MaLabel" runat="server" Text='<%# Bind("Ma") %>' />
                        </div>
                        <div style="height: 21px">
                            Se:
        <asp:Label ID="SeLabel" runat="server" Text='<%# Bind("Se") %>' />
                        </div>
                        <div style="height: 21px">
                            Zr:
        <asp:Label ID="ZrLabel" runat="server" Text='<%# Bind("Zr") %>' />
                        </div>
                        <div style="height: 21px">
                            La:
        <asp:Label ID="LaLabel" runat="server" Text='<%# Bind("La") %>' />
                        </div>

                        <div style="height: 21px">
                            Ir:
        <asp:Label ID="IrLabel" runat="server" Text='<%# Bind("Ir") %>' />
                        </div>
                        <div style="height: 21px">
                            Pt:
        <asp:Label ID="PtLabel" runat="server" Text='<%# Bind("Pt") %>' />
                        </div>
                        <div style="height: 21px">
                            Notes:
        <asp:Label ID="NotesLabel" runat="server" Text='<%# Bind("Notes") %>' />
                        </div>
                        <asp:LinkButton ID="EditButton" OnClick="EditButton_Click" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" />
                        &nbsp;<asp:LinkButton Visible="False" ID="DeleteButton" OnClick="DeleteButton_Click" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" />
                        &nbsp;<asp:LinkButton ID="NewButton" OnClick="NewButton_Click" runat="server" CausesValidation="False" CommandName="New" Text="New" />
                    </ItemTemplate>
                </asp:FormView>
                <br />
            </asp:Panel>
            <asp:Panel Visible="false" ID="ArchivedPanel" runat="server" Width="848px">
                <asp:FormView ID="FormView2" OnClick="ArchivedFormView_FormChanged" runat="server" DataKeyNames="Key" DataSourceID="ArchiveSqlDataSource" BorderColor="#5D7B9D"
                    BorderStyle="Double" BorderWidth="5px" CellPadding="10" ForeColor="#333333" Width="415px" AllowPaging="True">
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <ItemTemplate>
                        <div style="display: flex; justify-content: space-between; align-items: center;">
                            <div style="height: 21px;">
                                Key:
        <asp:Label ID="KeyLabel" runat="server" Text='<%# Eval("Key") %>' />
                            </div>
                            <asp:LinkButton ID="RestoreButton" OnClick="RestoreButton_Click" runat="server" Text="Restore" />
                        </div>
                        <div style="height: 21px">
                            MDL_User:
        <asp:Label ID="MDL_UserLabel" runat="server" Text='<%# Bind("MDL_User") %>' />
                        </div>
                        <div style="height: 21px">
                            EnterDate:
        <asp:Label ID="EnterDateLabel" runat="server" Text='<%# Bind("EnterDate") %>' />
                        </div>
                        <div style="height: 21px">
                            ExpireDate:
        <asp:Label ID="ExpireDateLabel" runat="server" Text='<%# Bind("ExpireDate") %>' />
                        </div>
                        <div style="height: 21px">
                            Li:
        <asp:Label ID="LiLabel" runat="server" Text='<%# Bind("Li") %>' />
                        </div>
                        <div style="height: 21px">
                            Na:
        <asp:Label ID="NaLabel" runat="server" Text='<%# Bind("Na") %>' />
                        </div>
                        <div style="height: 21px">
                            Mg:
        <asp:Label ID="MgLabel" runat="server" Text='<%# Bind("Mg") %>' />
                        </div>
                        <div style="height: 21px">
                            Al:
        <asp:Label ID="AlLabel" runat="server" Text='<%# Bind("Al") %>' />
                        </div>
                        <div style="height: 21px">
                            K:
        <asp:Label ID="KLabel" runat="server" Text='<%# Bind("K") %>' />
                        </div>
                        <div style="height: 21px">
                            Ca:
        <asp:Label ID="CaLabel" runat="server" Text='<%# Bind("Ca") %>' />
                        </div>
                        <div style="height: 21px">
                            Ti:
        <asp:Label ID="TiLabel" runat="server" Text='<%# Bind("Ti") %>' />
                        </div>
                        <div style="height: 21px">
                            V:
        <asp:Label ID="VLabel" runat="server" Text='<%# Bind("V") %>' />
                        </div>
                        <div style="height: 21px">
                            Cr:
        <asp:Label ID="CrLabel" runat="server" Text='<%# Bind("Cr") %>' />
                        </div>
                        <div style="height: 21px">
                            Mn:
        <asp:Label ID="MnLabel" runat="server" Text='<%# Bind("Mn") %>' />
                        </div>
                        <div style="height: 21px">
                            Fe:
        <asp:Label ID="FeLabel" runat="server" Text='<%# Bind("Fe") %>' />
                        </div>
                        <div style="height: 21px">
                            Co:
        <asp:Label ID="CoLabel" runat="server" Text='<%# Bind("Co") %>' />
                        </div>
                        <div style="height: 21px">
                            Ni:
        <asp:Label ID="NiLabel" runat="server" Text='<%# Bind("Ni") %>' />
                        </div>
                        <div style="height: 21px">
                            Cu:
        <asp:Label ID="CuLabel" runat="server" Text='<%# Bind("Cu") %>' />
                        </div>
                        <div style="height: 21px">
                            Zn:
        <asp:Label ID="ZnLabel" runat="server" Text='<%# Bind("Zn") %>' />
                        </div>
                        <div style="height: 21px">
                            Ga:
        <asp:Label ID="GaLabel" runat="server" Text='<%# Bind("Ga") %>' />
                        </div>
                        <div style="height: 21px">
                            As:
        <asp:Label ID="ArsLabel" runat="server" Text='<%# Bind("Ars") %>' />
                        </div>
                        <div style="height: 21px">
                            Sr:
        <asp:Label ID="SrLabel" runat="server" Text='<%# Bind("Sr") %>' />
                        </div>
                        <div style="height: 21px">
                            Mo:
        <asp:Label ID="MoLabel" runat="server" Text='<%# Bind("Mo") %>' />
                        </div>
                        <div style="height: 21px">
                            Ag:
        <asp:Label ID="AgLabel" runat="server" Text='<%# Bind("Ag") %>' />
                        </div>
                        <div style="height: 21px">
                            Ba:
        <asp:Label ID="BaLabel" runat="server" Text='<%# Bind("Ba") %>' />
                        </div>
                        <div style="height: 21px">
                            Ta:
        <asp:Label ID="TaLabel" runat="server" Text='<%# Bind("Ta") %>' />
                        </div>
                        <div style="height: 21px">
                            W:
        <asp:Label ID="WLabel" runat="server" Text='<%# Bind("W") %>' />
                        </div>
                        <div style="height: 21px">
                            Au:
        <asp:Label ID="AuLabel" runat="server" Text='<%# Bind("Au") %>' />
                        </div>
                        <div style="height: 21px">
                            Pb:
        <asp:Label ID="PbLabel" runat="server" Text='<%# Bind("Pb") %>' />
                        </div>
                        <div style="height: 21px">
                            Ma:
        <asp:Label ID="MaLabel" runat="server" Text='<%# Bind("Ma") %>' />
                        </div>
                        <div style="height: 21px">
                            Se:
        <asp:Label ID="SeLabel" runat="server" Text='<%# Bind("Se") %>' />
                        </div>
                        <div style="height: 21px">
                            Zr:
        <asp:Label ID="ZrLabel" runat="server" Text='<%# Bind("Zr") %>' />
                        </div>
                        <div style="height: 21px">
                            La:
        <asp:Label ID="LaLabel" runat="server" Text='<%# Bind("La") %>' />
                        </div>

                        <div style="height: 21px">
                            Ir:
        <asp:Label ID="IrLabel" runat="server" Text='<%# Bind("Ir") %>' />
                        </div>
                        <div style="height: 21px">
                            Pt:
        <asp:Label ID="PtLabel" runat="server" Text='<%# Bind("Pt") %>' />
                        </div>
                        <div style="height: 21px">
                            Notes:
        <asp:Label ID="NotesLabel" runat="server" Text='<%# Bind("Notes") %>' />
                        </div>
                    </ItemTemplate>

                    <PagerTemplate>
                        <div style="display: flex; justify-content: space-between; align-items: center;">
                            <asp:LinkButton ID="FirstButton" CommandName="Page" CommandArgument="First" Text="<<" runat="server" />
                            <asp:LinkButton ID="PrevButton" CommandName="Page" CommandArgument="Prev" Text="<" runat="server" />
                            <asp:LinkButton ID="NextButton" CommandName="Page" CommandArgument="Next" Text=">" runat="server" />
                            <asp:LinkButton ID="LastButton" CommandName="Page" CommandArgument="Last" Text=">>" runat="server" />
                        </div>
                    </PagerTemplate>

                </asp:FormView>

                <asp:SqlDataSource ID="ArchiveSqlDataSource"
                    SelectCommand="SELECT [Key], [MDL_User], [EnterDate], [ExpireDate], Li, Na, Mg, Al, K, Ca, Ti, V, Cr, Mn, Fe, Co, Ni, Cu, Zn, Ga, Ars, Sr, Mo, Ag, Ba, Ta, W, Au, Pb, Ma, Se, Zr, La, Ir, Pt, [Notes] FROM T_Metals_MDL WHERE ExpireDate IS NOT NULL ORDER BY EnterDate DESC"
                    ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

