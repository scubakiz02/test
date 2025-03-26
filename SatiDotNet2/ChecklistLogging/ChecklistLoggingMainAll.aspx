<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ChecklistLoggingMainAll.aspx.vb" Inherits="DBMaintenance_DBMaintenanceMain" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Panel ID="Panel1" runat="server">
        <span style="font-size: 14pt"><strong>
            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Checklist Logging"></asp:Label><br />
        </strong></span>
        <br />

        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/ChecklistLogging/ChecklistBuilder.aspx">Build Checklist</asp:HyperLink><br />
        <br />

        Status Board:<br />
        <asp:HyperLink ID="HyperLink12" runat="server" NavigateUrl="~/ChecklistLogging/StatusBoard.aspx?Department=All&View=Focus">All Departments, Focused View</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/ChecklistLogging/StatusBoard.aspx?Department=All&View=Full">All Departments, Full View</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/ChecklistLogging/StatusBoard.aspx?Department=Production&View=Focus">Production, Focused View</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/ChecklistLogging/StatusBoard.aspx?Department=Production&View=Full">Production, Full View</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/ChecklistLogging/StatusBoard.aspx?Department=Maintenance&View=Focus">Maintenance, Focused View</asp:HyperLink><br />
        <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/ChecklistLogging/StatusBoard.aspx?Department=Maintenance&View=Full">Maintenance, Full View</asp:HyperLink><br />
        <br />
    </asp:Panel>

</asp:Content>

