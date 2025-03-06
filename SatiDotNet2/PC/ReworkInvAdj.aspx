<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ReworkInvAdj.aspx.vb" Inherits="PC_ReworkInvAdj" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Rework Inv/Adj"></asp:Label>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="915px">
                Select Rework Type<br />
                <asp:RadioButton ID="StripRadioButton" runat="server" AutoPostBack="True" GroupName="type"
                    OnCheckedChanged="StripRadioButton_CheckedChanged" Text="Strip & Etch" />
                ,
                <asp:RadioButton ID="LapRadioButton" runat="server" AutoPostBack="True" GroupName="type"
                    OnCheckedChanged="LapRadioButton_CheckedChanged" Text="Lap" />
                ,
                <asp:RadioButton ID="PolishRadioButton" runat="server" AutoPostBack="True" GroupName="type"
                    OnCheckedChanged="PolishRadioButton_CheckedChanged" Text="Polish"  Checked="True" /><br />
                <br />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="RWSqlDataSource" CellPadding="4" ForeColor="#333333" GridLines="None" Width="328px">
                    <Columns>
                        <asp:BoundField DataField="MainID" HeaderText="MainID" SortExpression="MainID" />
                        <asp:BoundField DataField="Qty" HeaderText="Qty" NullDisplayText="0" SortExpression="Qty" />
                        <asp:TemplateField HeaderText="New Qty">
                            <ItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Width="70px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#EFF3FB" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <AlternatingRowStyle BackColor="LightBlue" />
                </asp:GridView>
                <asp:Button ID="SEAdjButton" runat="server" Text="ADJ New Qtys" Width="104px" /><br />
                <br />
    Info Box<br />
    <asp:TextBox ID="InfoTextBox" runat="server" Width="304px"></asp:TextBox>
    <asp:Button ID="Button2" runat="server" Text="Refresh Page" Width="96px" /></asp:Panel>
            &nbsp; &nbsp;&nbsp;
        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp; &nbsp;
    <asp:SqlDataSource ID="RWSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT dbo.MainID.MainID, dbo.Q_Inv_Rework_Polish.Qty FROM dbo.MainID LEFT OUTER JOIN dbo.Q_Inv_Rework_Polish ON dbo.MainID.MainID = dbo.Q_Inv_Rework_Polish.ID GROUP BY dbo.MainID.MainID, dbo.Q_Inv_Rework_Polish.Qty"></asp:SqlDataSource>
    <br />
    <br />
    <br />
</asp:Content>

