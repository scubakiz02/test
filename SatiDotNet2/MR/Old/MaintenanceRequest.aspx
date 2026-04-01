<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MaintenanceRequest.aspx.vb" Inherits="MR_MaintenanceRequest" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    &nbsp;<asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large"
        Text="Maintenance Request for Process Tools"></asp:Label>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="936px">
                Select Department
                <br />
                <asp:DropDownList ID="DepartmentDropDownList" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                    DataSourceID="DepartmentSqlDataSource" DataTextField="Department" DataValueField="Department"
                    OnSelectedIndexChanged="departmentDropDownList_SelectedIndexChanged" Width="168px">
                    <asp:ListItem>Select One...</asp:ListItem>
                </asp:DropDownList><asp:SqlDataSource ID="DepartmentSqlDataSource" runat="server"
                    ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT [Department] FROM [T_Departments]">
                </asp:SqlDataSource>
                <br />
                <br /> 
                Select Tool
                <br />
                <asp:DropDownList ID="ToolDropDownList" runat="server" DataSourceID="ToolSqlDataSource"
                    DataTextField="Tool" DataValueField="Key" Width="168px">
                </asp:DropDownList><asp:SqlDataSource ID="ToolSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT Tool, [Key] FROM dbo.T_Tools WHERE (Department = '0')"></asp:SqlDataSource>
                <br />
                <br />
                Is the Tool Down?
                <br />
                <asp:RadioButton ID="StatusUpRadioButton" runat="server" GroupName="Down" Text="No" ValidationGroup="Down" Checked="True" />
                Or
                <asp:RadioButton ID="StatusDownRadioButton" runat="server" GroupName="Down" Text="Yes" ValidationGroup="Down" /><br />
                <br />
                Lot Number?<br />
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                <br />
                <br />
                Instance Number?<br />
                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                <br />
                <br />
                Describe the problem<br />
                <asp:TextBox ID="ProblemTextBox" runat="server" Height="136px" TextMode="MultiLine" Width="336px"></asp:TextBox><br />
                <asp:Button ID="Button1" runat="server" Text="Submit" />
                <br />
                <asp:Label ID="infoLabel" runat="server" Width="640px"></asp:Label><br />
            </asp:Panel>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

