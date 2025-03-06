<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MakeReworkLots.aspx.vb" Inherits="PC_MakeReworkLots" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="Label1" runat="server" Text="Select Customer..."></asp:Label><br />
            <asp:DropDownList ID="DropDownList1" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                DataSourceID="CustomersSqlDataSource" DataTextField="Customer_Name" DataValueField="Customer_Name"
                OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" Width="240px">
                <asp:ListItem Selected="True">Select Customer...</asp:ListItem>
            </asp:DropDownList>
            <br />
            <br />
            <asp:Label ID="Label2" runat="server" Text="Select Rework Type..."></asp:Label><br />
            <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged" Width="240px">
                <asp:ListItem Selected="True">Select One...</asp:ListItem>
                <asp:ListItem Value="SE">Strip &amp; Etch Rework</asp:ListItem>
                <asp:ListItem Value="L">Lapping Rework</asp:ListItem>
                <asp:ListItem Value="P">Polish Rework</asp:ListItem>
                <asp:ListItem Value="T7">T7 Rework</asp:ListItem>
            </asp:DropDownList><br />
            <br />
            <asp:Label ID="Label3" runat="server" Text="Select Qty from ID's to Make Lot"></asp:Label><br />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="IDsSqlDataSource" >
                <Columns>
                    <asp:BoundField DataField="CustomerID" HeaderText="CustomerID" SortExpression="CustomerID" />
                    <asp:BoundField DataField="Diameter" HeaderText="Diameter" SortExpression="Diameter" />
                    <asp:BoundField DataField="MainID" HeaderText="MainID" SortExpression="MainID" />
                    <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty" />
                    <asp:TemplateField HeaderText="Use Qty">
                        <ItemTemplate>
                            <asp:TextBox ID="SelectQtyTextBox" runat="server" OnTextChanged="SelectQtyTextBox_TextChanged" AutoPostBack="True"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            Make Lot ID
            <asp:DropDownList ID="IDDropDownList" runat="server" AppendDataBoundItems="True"
                AutoPostBack="True" BackColor="LightCoral" DataSourceID="IDbyDieSqlDataSource"
                DataTextField="MainID" DataValueField="MainID" OnSelectedIndexChanged="IDDropDownList_SelectedIndexChanged"
                Width="152px">
                <asp:ListItem Selected="True" Value="&quot;&quot;">Select ID...</asp:ListItem>
            </asp:DropDownList><br />
            <br />
            Total Wafer Selected &nbsp;&nbsp;
            <asp:Label ID="QtyLabel" runat="server" BackColor="LightCoral" Text="0"></asp:Label><br />
            <br />
            <asp:Panel ID="Panel300mm" runat="server" BackColor="LightCoral" BorderColor="White"
                Height="80px" Visible="False" Width="160px">
                Select...<br />
                <asp:RadioButton ID="DSPRadioButton" runat="server" AutoPostBack="True" GroupName="Ptype"
                    OnCheckedChanged="DSPRadioButton_CheckedChanged" Text="DSP Rework" /><br />
                <asp:RadioButton ID="CMPRadioButton" runat="server" AutoPostBack="True" GroupName="ptype"
                    OnCheckedChanged="CMPRadioButton_CheckedChanged" Text="CMP Rework" /></asp:Panel>
            &nbsp;<br />
            <asp:TextBox ID="InfoTextBox" runat="server" Height="112px" TextMode="MultiLine" Width="400px"></asp:TextBox><br />
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;<asp:Button ID="Button1" runat="server" Text="Make Lot" OnClick="Button1_Click" />
            <asp:SqlDataSource ID="IDsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                SelectCommand="SELECT dbo.MainID.CustomerID, dbo.MainID.Diameter, dbo.MainID.MainID, SUM(dbo.T_Rework_Invintory.Qty) AS Qty FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID INNER JOIN dbo.T_Rework_Invintory ON dbo.MainID.MainID = dbo.T_Rework_Invintory.ID WHERE (dbo.Customer.Customer_Name = N'Blank') GROUP BY dbo.MainID.MainID, dbo.T_Rework_Invintory.Type, dbo.MainID.CustomerID, dbo.MainID.Diameter HAVING (dbo.T_Rework_Invintory.Type = N'-5') ORDER BY dbo.MainID.CustomerID, dbo.MainID.Diameter DESC, dbo.MainID.MainID">
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="CustomersSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                SelectCommand="SELECT Customer_Name FROM dbo.Customer GROUP BY Customer_Name"></asp:SqlDataSource>
            <asp:SqlDataSource ID="IDbyDieSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                SelectCommand="SELECT dbo.MainID.CustomerID, dbo.MainID.Diameter, dbo.MainID.MainID FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID WHERE (dbo.Customer.Customer_Name = N'Blank') GROUP BY dbo.MainID.MainID, dbo.MainID.CustomerID, dbo.MainID.Diameter HAVING (dbo.MainID.Diameter = 200) ORDER BY dbo.MainID.CustomerID, dbo.MainID.Diameter DESC, dbo.MainID.MainID">
            </asp:SqlDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            Making Lot...<img src="../Color/Animated_LoadingBigger.gif" />
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>

