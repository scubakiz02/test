<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="InstanceMakerNotes.aspx.vb" Inherits="DBMaintenance_InstanceMakerNotes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="912px">
                <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Black" Text="Instance Maker Notes:"></asp:Label><br />
                <br />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="Key" DataSourceID="SqlDataSourceNotes">
                    <Columns>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True" SortExpression="Key" />
                        <asp:BoundField DataField="WaferID" HeaderText="WaferID" SortExpression="WaferID" />
                        <asp:BoundField DataField="Note" HeaderText="Note" SortExpression="Note" />
                        <asp:CommandField ShowDeleteButton="True" />
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <RowStyle ForeColor="#000066" />
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                </asp:GridView><br />
                <br />
                <asp:SqlDataSource ID="SqlDataSourceNotes" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" DeleteCommand="DELETE FROM [T_InstanceNotes] WHERE [Key] = @original_Key AND (([WaferID] = @original_WaferID) OR ([WaferID] IS NULL AND @original_WaferID IS NULL)) AND (([Note] = @original_Note) OR ([Note] IS NULL AND @original_Note IS NULL))" InsertCommand="INSERT INTO [T_InstanceNotes] ([WaferID], [Note]) VALUES (@WaferID, @Note)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [Key], [WaferID], [Note] FROM [T_InstanceNotes]" UpdateCommand="UPDATE [T_InstanceNotes] SET [WaferID] = @WaferID, [Note] = @Note WHERE [Key] = @original_Key AND (([WaferID] = @original_WaferID) OR ([WaferID] IS NULL AND @original_WaferID IS NULL)) AND (([Note] = @original_Note) OR ([Note] IS NULL AND @original_Note IS NULL))">
                    <DeleteParameters>
                        <asp:Parameter Name="original_Key" Type="Int32" />
                        <asp:Parameter Name="original_WaferID" Type="String" />
                        <asp:Parameter Name="original_Note" Type="String" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="WaferID" Type="String" />
                        <asp:Parameter Name="Note" Type="String" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="WaferID" Type="String" />
                        <asp:Parameter Name="Note" Type="String" />
                        <asp:Parameter Name="original_Key" Type="Int32" />
                        <asp:Parameter Name="original_WaferID" Type="String" />
                        <asp:Parameter Name="original_Note" Type="String" />
                    </UpdateParameters>
                </asp:SqlDataSource>
                <asp:Button ID="ButtonAdd" runat="server" Text="Add New Note" />
                <asp:Panel ID="Panel2" runat="server" BackColor="#66FF99" BorderStyle="Solid">
                    &nbsp;<asp:Label ID="Label1" runat="server" Text="Wafer ID: "></asp:Label>&nbsp;<br />
                    &nbsp;<asp:TextBox ID="TextBoxWaferID" runat="server"></asp:TextBox>&nbsp;<br />
                    <br />
                    &nbsp;<asp:Label ID="Label3" runat="server" Text="Note: "></asp:Label>&nbsp;<br />
                    &nbsp;<asp:TextBox ID="TextBoxNote" runat="server"></asp:TextBox>&nbsp;<br />
                    <br />
                    &nbsp;<asp:Button ID="ButtonSave" runat="server" Text="Save" />
                </asp:Panel>
                <cc1:ModalPopupExtender ID="Panel2_ModalPopupExtender" 
                                        runat="server" 
                                        BehaviorID="Panel2_ModalPopupExtender" 
                                        DynamicServicePath="" 
                                        TargetControlID="ButtonAdd" 
                                        PopupControlID="Panel2"
                                        OkControlID="ButtonSave">
                </cc1:ModalPopupExtender>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

