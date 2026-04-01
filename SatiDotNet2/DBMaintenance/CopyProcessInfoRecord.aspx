<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="CopyProcessInfoRecord.aspx.vb" Inherits="DBMaintenance_CopyProcessInfoRecord" title="Untitled Page" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdateProgress id="UpdateProgress1" runat="server">
        <progresstemplate>
Copying...
            <img src="../Color/Animated_LoadingBigger.gif" />
</progresstemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<asp:Panel id="Panel1" runat="server" Height="152px" Width="544px">Select a ID record from the Process Info Table<br /><asp:DropDownList id="From_ID_DropDownList" runat="server" DataTextField="ID_NUMBER" DataSourceID="P_Info_IDs_SqlDataSource" DataValueField="entry"></asp:DropDownList><asp:SqlDataSource id="P_Info_IDs_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT ID_NUMBER, entry, ExpirationDtd FROM dbo.PROCESS_INFO WHERE (ExpirationDtd < { fn NOW() } OR ExpirationDtd IS NULL)"></asp:SqlDataSource> <br />Next<br />Select a ID an avalible ID in SATI that is undefined in Process Info<br /><asp:DropDownList id="TO_ID_DropDownList" runat="server" DataTextField="MainID" DataSourceID="AvalibleIDsSqlDataSource" DataValueField="MainID" AutoPostBack="True"></asp:DropDownList><asp:SqlDataSource id="AvalibleIDsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT dbo.MainID.MainID FROM dbo.MainID LEFT OUTER JOIN dbo.PROCESS_INFO ON dbo.MainID.MainID = dbo.PROCESS_INFO.ID_NUMBER WHERE (dbo.PROCESS_INFO.ID_NUMBER IS NULL)"></asp:SqlDataSource> <br />Next<br />Press "Copy" to copy the ID's<br />&nbsp;<asp:Button id="Button1" onclick="Button1_Click" runat="server" Text="Copy" Visible="False"></asp:Button></asp:Panel> 
</contenttemplate>
    </asp:UpdatePanel>
    &nbsp;
    <br />
</asp:Content>

