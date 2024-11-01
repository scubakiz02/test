<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ST_Test.aspx.vb" Inherits="TestArea_Email_Test" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="904px">
                Instance Key:&nbsp;
                <asp:TextBox ID="TextBoxEmailAddress" runat="server" Width="200px">570245</asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="Generate Metals Sample" />
                <br />
                <asp:GridView ID="GridView1" OnPageIndexChanging="GridView1_PageIndexChanging" runat="server" AutoGenerateColumns="False" DataKeyNames="Record_Number" DataSourceID="SqlDataSource1" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical" AllowPaging="True" AllowSorting="True" PageSize="15">
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                        <asp:BoundField DataField="column1" HeaderText="Date/Time" SortExpression="column1" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField DataField="Source" HeaderText="Source" SortExpression="Source" />
                        <asp:BoundField DataField="Test_Type" HeaderText="Test_Type" SortExpression="Test_Type" />
                        <asp:BoundField DataField="Location" HeaderText="Location" SortExpression="Location" />
                        <asp:BoundField DataField="Ca" HeaderText="Ca" SortExpression="Ca" />
                        <asp:BoundField DataField="Ma" HeaderText="Ma" SortExpression="Ma" />
                        <asp:BoundField DataField="Ni" HeaderText="Ni" SortExpression="Ni" />
                        <asp:BoundField DataField="Zn" HeaderText="Zn" SortExpression="Zn" />
                        <asp:BoundField DataField="Al" HeaderText="Al" SortExpression="Al" />
                        <asp:BoundField DataField="Fe" HeaderText="Fe" SortExpression="Fe" />
                        <asp:BoundField DataField="Cr" HeaderText="Cr" SortExpression="Cr" />
                        <asp:BoundField DataField="Cu" HeaderText="Cu" SortExpression="Cu" />
                        <asp:BoundField DataField="Na" HeaderText="Na" SortExpression="Na" />
                        <asp:BoundField DataField="K" HeaderText="K" SortExpression="K" />
                        <asp:BoundField DataField="Co" HeaderText="Co" SortExpression="Co" />
                        <asp:BoundField DataField="Mn" HeaderText="Mn" SortExpression="Mn" />
                        <asp:BoundField DataField="Ars" HeaderText="Ars" SortExpression="Ars" />
                        <asp:BoundField DataField="Se" HeaderText="Se" SortExpression="Se" />
                        <asp:BoundField DataField="Ag" HeaderText="Ag" SortExpression="Ag" />
                        <asp:BoundField DataField="Pb" HeaderText="Pb" SortExpression="Pb" />
                        <asp:BoundField DataField="Ti" HeaderText="Ti" SortExpression="Ti" />
                        <asp:BoundField DataField="Ta" HeaderText="Ta" SortExpression="Ta" />
                        <asp:BoundField DataField="W" HeaderText="W" SortExpression="W" />
                        <asp:BoundField DataField="Au" HeaderText="Au" SortExpression="Au" />
                        <asp:BoundField DataField="Mo" HeaderText="Mo" SortExpression="Mo" />
                        <asp:BoundField DataField="Zr" HeaderText="Zr" SortExpression="Zr" />
                        <asp:BoundField DataField="La" HeaderText="La" SortExpression="La" />
                        <asp:BoundField DataField="Sr" HeaderText="Sr" SortExpression="Sr" />
                        <asp:BoundField DataField="Ir" HeaderText="Ir" SortExpression="Ir" />
                        <asp:BoundField DataField="Pt" HeaderText="Pt" SortExpression="Pt" />
                        <asp:BoundField DataField="Ga" HeaderText="Ga" SortExpression="Ga" />
                        <asp:BoundField DataField="Li" HeaderText="Li" SortExpression="Li" />
                        <asp:BoundField DataField="V" HeaderText="V" SortExpression="V" />
                        <asp:BoundField DataField="Ba" HeaderText="Ba" SortExpression="Ba" />
                        <asp:BoundField DataField="Record_Number" HeaderText="Record_Number" InsertVisible="False" ReadOnly="True" SortExpression="Record_Number" />
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" />
                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#808080" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#383838" />
                    <PagerTemplate>
                        <div style="display: flex; justify-content: space-between; align-items: center;">
                            <asp:LinkButton ID="FirstButton" CommandName="Page" CommandArgument="First" Text="<<" runat="server" />
                            <asp:LinkButton ID="PrevButton" CommandName="Page" CommandArgument="Prev" Text="<" runat="server" />
                            <asp:LinkButton ID="NextButton" CommandName="Page" CommandArgument="Next" Text=">" runat="server" />
                            <asp:LinkButton ID="LastButton" CommandName="Page" CommandArgument="Last" Text=">>" runat="server" />
                        </div>
                    </PagerTemplate>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT [Date/Time] AS column1, [Source], [Test Type] AS Test_Type, [Location], [Ca], [Ma], [Ni], [Zn], [Al], [Fe], [Cr], [Cu], [Na], [K], [Co], [Mn], [Ars], [Se], [Ag], [Pb], [Ti], [Ta], [W], [Au], [Mo], [Zr], [La], [Sr], [Ir], [Pt], [Ga], [Li], [V], [Ba], [Record Number] AS Record_Number FROM [GFAAS Data] WHERE Location='Bob' Order BY [Record Number] DESC"></asp:SqlDataSource>
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

