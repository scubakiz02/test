<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="QuickTestLogic.aspx.vb" Inherits="DBMaintenance_QuickTestLogic" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server">
               Test CB box fix. <br />
                <br />
                Call CBFullDataRecordCheck<br />
                CB &nbsp;<asp:TextBox ID="TextBoxCBFullDataRecordCheck_CB" runat="server" ToolTip="Enter CB Number" Text="211018"></asp:TextBox><br />
                Para &nbsp;<asp:TextBox ID="TextBoxCBFullDataRecordCheck_Para" runat="server" ToolTip="PreGeo, PostGeo, Partical, All" Text="PreGeo, PostGeo, Partical, All" Width="200px"></asp:TextBox><br />
                <asp:Button ID="ButtonCBFullDataRecordCheck" runat="server" Text="Test" />&nbsp;<asp:Label ID="LabelCBFullDataRecordCheck" runat="server" Text="Reply"></asp:Label><br />
                <br />
                <br />

                Call CB_CheckAndFix_Geo<br />
                CB &nbsp;<asp:TextBox ID="TextBoxCB_CheckAndFix_Geo" runat="server" ToolTip="Enter CB Number" Text="211018"></asp:TextBox><br />
                <asp:Button ID="ButtonCB_CheckAndFix_Geo" runat="server" Text="Test" />&nbsp;<asp:Label ID="LabelCB_CheckAndFix_Geo" runat="server" Text="Reply"></asp:Label><br />
                <br />
                <br />
                <asp:TextBox ID="TextBoxBoxType" runat="server" Text="CB"></asp:TextBox>
                <asp:TextBox ID="TextBoxParticalSpec" runat="server" Height="16px" Width="310px"></asp:TextBox>
                <asp:Button ID="ButtonPartical" runat="server" Text="Button" />
                <br />
                <br />
                <br />
                <asp:Button ID="Button1" runat="server" Text="Run 300mm FGI QA Scan" />
                <br />
                 <br />
                 <br />
                 <br />
                 <br />
                <asp:Button ID="Button2" runat="server" Text="Run A Data Tweak Test" />
                 <br />
                 <br />

            </asp:Panel>
            <asp:UpdateProgress id="UpdateProgress1" runat="server">
    <ProgressTemplate>
        <IMG src="../Color/Animated_LoadingBigger.gif" />Working...
    </ProgressTemplate>
</asp:UpdateProgress>
        </ContentTemplate>        
    </asp:UpdatePanel>
    
     
</asp:Content>
