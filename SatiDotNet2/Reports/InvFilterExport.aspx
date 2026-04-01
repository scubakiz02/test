<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="InvFilterExport.aspx.vb" Inherits="Reports_InvFilterExport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="1160px">
               
                
                &nbsp;<asp:CheckBox ID="CheckBox_Remove_Zero_Lines" runat="server" Text="Remove Zero Balance Lines" /><br />
                &nbsp;<asp:CheckBox ID="CheckBox_Remove_Zero_FGI" runat="server" Text="Remove Zero Balance FGI Lines" /><br />
                &nbsp;<asp:CheckBox ID="CheckBox_Group_Size" runat="server" Text="Group By Diameter" /><br />
                &nbsp;<asp:CheckBox ID="CheckBox_Sort_Customer_ID" runat="server" Text="Sort By Customer Then ID" /><br />
                &nbsp;<asp:CheckBox ID="CheckBoxEmail" runat="server" Text="Send Copy To My Email " />&nbsp; &nbsp;
                <asp:TextBox ID="TextBoxEmail" runat="server" ></asp:TextBox><asp:Label ID="Label1" runat="server" Text="@Purewafer.com" BackColor="#FFFF66"></asp:Label>
                <br />
                <br />
                <asp:Button ID="ButtonGo" runat="server" Text="Go" /> &nbsp; &nbsp; &nbsp;&nbsp;
                <asp:HyperLink ID="HyperLinkReport" runat="server" Visible="False">Download Report</asp:HyperLink>
                <br />
            <br />
            <br />
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <img src="../Color/Animated_LoadingBigger.gif" />Working...
                    </ProgressTemplate>
                </asp:UpdateProgress>
    <br />
    <br />
                <asp:Panel ID="Panel2" runat="server" BackColor="White">
                <asp:GridView ID="GridView1" runat="server" BackColor="White">
                </asp:GridView>
                </asp:Panel>
                
                &nbsp;
            </asp:Panel>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
</asp:Content>

