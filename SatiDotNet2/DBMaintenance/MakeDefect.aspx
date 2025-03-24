<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MakeDefect.aspx.vb" Inherits="DBMaintenance_MakeDefect" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Make a
    <asp:TextBox ID="DefectNameTextBox" runat="server" Style="z-index: 100; left: 32px;
        position: absolute; top: 240px" Width="208px"></asp:TextBox>
    <asp:Label ID="Label1" runat="server" Style="z-index: 101; left: 32px; position: absolute;
        top: 224px" Text="Defect Name:" Width="120px"></asp:Label>
    Defect:
    <asp:Button ID="Button1" runat="server" Style="z-index: 102; left: 32px; position: absolute;
        top: 288px" Text="Submit" />
    <br />
    <br />
    <asp:Label ID="Label2" runat="server" Style="z-index: 103; left: 280px; position: absolute;
        top: 224px" Text="Type:" Width="88px"></asp:Label>
    <asp:Label ID="GroupLabel" runat="server" Style="z-index: 107; left: 424px; position: absolute;
        top: 224px" Text="Group:" Width="80px"></asp:Label>
    <br />
    <asp:DropDownList ID="TypeDropDownList" runat="server" AutoPostBack="True" Style="z-index: 105;
        left: 280px; position: absolute; top: 240px" Width="112px">
        <asp:ListItem>Rework</asp:ListItem>
        <asp:ListItem>Reject</asp:ListItem>
    </asp:DropDownList>
    <asp:DropDownList ID="GroupDropDownList" runat="server" Style="z-index: 106; left: 424px;
        position: absolute; top: 240px" Width="152px">
        <asp:ListItem>StripEtch</asp:ListItem>
        <asp:ListItem>Polish</asp:ListItem>
        <asp:ListItem>Lap</asp:ListItem>
    </asp:DropDownList>
    <br />
    <br />
    <br />
    <br />
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConflictDetection="CompareAllValues"
        ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" DeleteCommand="DELETE FROM [T_ID_Defects] WHERE [Key] = @original_Key AND [ID] = @original_ID AND [Defect] = @original_Defect AND [Type] = @original_Type AND [Group] = @original_Group"
        InsertCommand="INSERT INTO [T_ID_Defects] ([ID], [Defect], [Type], [Group]) VALUES (@ID, @Defect, @Type, @Group)"
        OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [T_ID_Defects]"
        UpdateCommand="UPDATE [T_ID_Defects] SET [ID] = @ID, [Defect] = @Defect, [Type] = @Type, [Group] = @Group WHERE [Key] = @original_Key AND [ID] = @original_ID AND [Defect] = @original_Defect AND [Type] = @original_Type AND [Group] = @original_Group">
        <DeleteParameters>
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_ID" Type="String" />
            <asp:Parameter Name="original_Defect" Type="String" />
            <asp:Parameter Name="original_Type" Type="String" />
            <asp:Parameter Name="original_Group" Type="String" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="ID" Type="String" />
            <asp:Parameter Name="Defect" Type="String" />
            <asp:Parameter Name="Type" Type="String" />
            <asp:Parameter Name="Group" Type="String" />
            <asp:Parameter Name="original_Key" Type="Int32" />
            <asp:Parameter Name="original_ID" Type="String" />
            <asp:Parameter Name="original_Defect" Type="String" />
            <asp:Parameter Name="original_Type" Type="String" />
            <asp:Parameter Name="original_Group" Type="String" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="ID" Type="String" />
            <asp:Parameter Name="Defect" Type="String" />
            <asp:Parameter Name="Type" Type="String" />
            <asp:Parameter Name="Group" Type="String" />
        </InsertParameters>
    </asp:SqlDataSource>
</asp:Content>

