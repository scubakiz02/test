<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ReceiveWafers.aspx.vb" Inherits="ReceiveWafers" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Receive Wafers"></asp:Label><br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="416px">
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                <asp:RadioButton ID="NormalInvRadioButton" runat="server" AutoPostBack="True" Checked="True"
                    GroupName="InvType" OnCheckedChanged="NormalInvRadioButton_CheckedChanged" Text="Normal Inv" />
                &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;<asp:RadioButton ID="SpecialInvRadioButton"
                    runat="server" AutoPostBack="True" GroupName="InvType" OnCheckedChanged="SpecialInvRadioButton_CheckedChanged"
                    Text="Special Inv" /><br />
                <asp:Panel ID="SpecialPanel" runat="server" Visible="False" Width="408px">
                    &nbsp; &nbsp; &nbsp;&nbsp;
                    <br />
                    &nbsp; &nbsp; &nbsp;&nbsp; ID Note:
                    <asp:TextBox ID="IDNoteTextBox" runat="server" Width="224px"></asp:TextBox></asp:Panel>
                <asp:Panel ID="NormalPanel" runat="server" Style="line-height: normal" Width="408px">
                    &nbsp; &nbsp; &nbsp;<br />
                    &nbsp; &nbsp; &nbsp;Customer:
    <asp:DropDownList ID="CustomerDropDownList" runat="server" DataSourceID="SqlDataSourceCustomers"
        DataTextField="Customer_Name" DataValueField="Customer_Name" Style="z-index: 104;
        left: 88px; position: static; top: 208px" Width="300px" AutoPostBack="True">
    </asp:DropDownList><br />
                    <br />
                    ID, Part, Fab:&nbsp;<asp:DropDownList ID="IDDropDownList" runat="server" DataSourceID="SqlDataSourceID"
        DataTextField="ID, Part, Fab" DataValueField="MainID" Style="z-index: 105; left: 464px;
        position: static; top: 168px" Width="300px">
    </asp:DropDownList><br />
                </asp:Panel>
                <asp:Panel ID="Panel3" runat="server" Width="408px">
                    <br />
                    Packing Slip:&nbsp;
    <asp:TextBox ID="PackingSlipTextBox" runat="server" Style="z-index: 106; left: 464px; position: static;
        top: 200px" Width="224px"></asp:TextBox><br />
                    <br />
                    &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; Carrier:&nbsp;<asp:DropDownList ID="CarrierDropDownList" runat="server" Style="z-index: 109; left: 464px;
        position: static; top: 232px" Width="232px" DataSourceID="CarrierSqlDataSource" DataTextField="Name" DataValueField="Name">
    </asp:DropDownList><br />
                    <br />
                    &nbsp; Wafer Qty: &nbsp;<asp:TextBox ID="WaferQtyTextBox" runat="server" Style="z-index: 107; left: 464px;
        position: static; top: 264px" Width="40px"></asp:TextBox>&nbsp;
                    Containment:
    <asp:DropDownList ID="ContanmentTypeDropDownList" runat="server" Style="z-index: 117;
        left: 288px; position: static; top: 616px" Width="96px">
        <asp:ListItem>Boxes</asp:ListItem>
        <asp:ListItem>Pallets</asp:ListItem>
        <asp:ListItem>Coin Rolls</asp:ListItem>
    </asp:DropDownList><br />
                    <br />
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp;Containment Qty: &nbsp;<asp:TextBox ID="ContainmentQtyTextBox" runat="server" Style="z-index: 119; left: 328px;
        position: static; top: 688px" Width="96px"></asp:TextBox><br />
                    <br />
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;Notes: &nbsp;<asp:TextBox ID="NoteTextBox" runat="server" Height="40px" Style="z-index: 115; left: 464px;
        position: static; top: 296px; vertical-align: top;" Width="224px" TextMode="MultiLine"></asp:TextBox><br />
                </asp:Panel>
                <br />
                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
    <asp:Button ID="Button1" runat="server" Style="z-index: 122; left: 368px; position: static;
        top: 536px" Text="Enter" />
    <asp:Label ID="WLLabel" runat="server" Font-Bold="True" Font-Size="16pt" Style="z-index: 121;
        left: 392px; position: static; top: 488px" Text="Label" Visible="False" Width="192px"></asp:Label><br />
            </asp:Panel>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    <img src="../Color/Animated_LoadingBigger.gif" />Working...
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp;<br />
    &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;<br />
    <br />
    <br />
    &nbsp;<br />
    <br />
    <br />
    &nbsp;&nbsp;<br />
    <br />
    &nbsp;<br />
    &nbsp; &nbsp;
    &nbsp;&nbsp;<br />
    <br />
    &nbsp;
    &nbsp;&nbsp;<br />
    <br />
    <br />
    &nbsp;<br />
    <br />
    <br />
    <asp:SqlDataSource ID="SqlDataSourceCustomers" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT DISTINCT dbo.Customer.Customer_Name FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID WHERE (dbo.MainID.[In-Out] = 1)">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="CarrierSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT DISTINCT Name FROM dbo.Carriers ORDER BY Name"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceID" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT DISTINCT dbo.Customer.Customer_Name, dbo.MainID.MainID, dbo.MainIDSpec.PART_NUMBER, dbo.MainID.CustomerID, dbo.MainID.MainID + N',  ' + dbo.MainIDSpec.PART_NUMBER + N',  ' + dbo.Customer.CustomerID AS [ID, Part, Fab] FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID INNER JOIN dbo.MainID_MainIDSpec ON dbo.MainID.MainID = dbo.MainID_MainIDSpec.MainID INNER JOIN dbo.MainIDSpec ON dbo.MainID_MainIDSpec.WaferSpec_Key = dbo.MainIDSpec.RecordNumber WHERE (dbo.MainID.[In-Out] = 1) AND (dbo.MainID_MainIDSpec.EffectiveDtd < { fn NOW() }) AND (dbo.MainID_MainIDSpec.ExpirationDtd IS NULL OR dbo.MainID_MainIDSpec.ExpirationDtd > { fn NOW() }) AND (dbo.Customer.Customer_Name = N'Exsil')">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="WaferLogSqlDataSource" runat="server" ConflictDetection="CompareAllValues"
        ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" DeleteCommand="DELETE FROM [DB_Characteristics] WHERE [FieldName] = @original_FieldName AND [Characteristic] = @original_Characteristic AND [Value] = @original_Value AND [EffectiveDtd] = @original_EffectiveDtd AND [ExpirationDtd] = @original_ExpirationDtd AND [EventTime] = @original_EventTime AND [error] = @original_error"
        InsertCommand="INSERT INTO [DB_Characteristics] ([FieldName], [Characteristic], [Value], [EffectiveDtd], [ExpirationDtd], [EventTime], [error]) VALUES (@FieldName, @Characteristic, @Value, @EffectiveDtd, @ExpirationDtd, @EventTime, @error)"
        OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [DB_Characteristics] WHERE ([FieldName] = @FieldName)"
        UpdateCommand="UPDATE [DB_Characteristics] SET [ExpirationDtd] = @ExpirationDtd, [EventTime] = @EventTime, [error] = @error WHERE [FieldName] = @original_FieldName AND [Characteristic] = @original_Characteristic AND [Value] = @original_Value AND [EffectiveDtd] = @original_EffectiveDtd AND [ExpirationDtd] = @original_ExpirationDtd AND [EventTime] = @original_EventTime AND [error] = @original_error">
        <DeleteParameters>
            <asp:Parameter Name="original_FieldName" Type="String" />
            <asp:Parameter Name="original_Characteristic" Type="String" />
            <asp:Parameter Name="original_Value" Type="String" />
            <asp:Parameter Name="original_EffectiveDtd" Type="DateTime" />
            <asp:Parameter Name="original_ExpirationDtd" Type="DateTime" />
            <asp:Parameter Name="original_EventTime" Type="DateTime" />
            <asp:Parameter Name="original_error" Type="Boolean" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="ExpirationDtd" Type="DateTime" />
            <asp:Parameter Name="EventTime" Type="DateTime" />
            <asp:Parameter Name="error" Type="Boolean" />
            <asp:Parameter Name="original_FieldName" Type="String" />
            <asp:Parameter Name="original_Characteristic" Type="String" />
            <asp:Parameter Name="original_Value" Type="String" />
            <asp:Parameter Name="original_EffectiveDtd" Type="DateTime" />
            <asp:Parameter Name="original_ExpirationDtd" Type="DateTime" />
            <asp:Parameter Name="original_EventTime" Type="DateTime" />
            <asp:Parameter Name="original_error" Type="Boolean" />
        </UpdateParameters>
        <SelectParameters>
            <asp:Parameter DefaultValue="Waferlog" Name="FieldName" Type="String" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="FieldName" Type="String" />
            <asp:Parameter Name="Characteristic" Type="String" />
            <asp:Parameter Name="Value" Type="String" />
            <asp:Parameter Name="EffectiveDtd" Type="DateTime" />
            <asp:Parameter Name="ExpirationDtd" Type="DateTime" />
            <asp:Parameter Name="EventTime" Type="DateTime" />
            <asp:Parameter Name="error" Type="Boolean" />
        </InsertParameters>
    </asp:SqlDataSource>
    &nbsp;<br />
    <br />
    &nbsp;&nbsp;<br />
    <br />
    &nbsp;<br />
    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;<br />
    &nbsp; &nbsp;&nbsp;
</asp:Content>

