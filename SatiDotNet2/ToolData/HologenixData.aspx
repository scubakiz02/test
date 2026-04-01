<%@ Page Language="C#" MasterPageFile="~/MasterPage1.master" AutoEventWireup="true" CodeFile="HologenixData.aspx.cs" Inherits="ToolData_HologenixData" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Hologenix Data"></asp:Label><br />
    <asp:Panel ID="Panel2" runat="server" Height="50px" Width="672px">
    </asp:Panel>
    <br />
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <img src="../Color/Animated_LoadingBigger.gif" />
            Working...
        </ProgressTemplate>
    </asp:UpdateProgress>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="912px">
                <br />
                <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1">
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
                <br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

