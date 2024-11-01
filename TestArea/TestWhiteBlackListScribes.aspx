<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="TestWhiteBlackListScribes.aspx.vb" Inherits="TestArea_TestWhiteBlackListScribes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
               Test Look for scribe Function<br />
               <br />
               Chr1 <asp:TextBox ID="TextBox1" runat="server" Width="20px" Text="*"></asp:TextBox>
               Chr2 <asp:TextBox ID="TextBox2" runat="server" Width="20px" Text="*"></asp:TextBox>
               Chr3 <asp:TextBox ID="TextBox3" runat="server" Width="20px" Text="*"></asp:TextBox>
               Chr4 <asp:TextBox ID="TextBox4" runat="server" Width="20px" Text="*"></asp:TextBox>
               Chr5 <asp:TextBox ID="TextBox5" runat="server" Width="20px" Text="*"></asp:TextBox>
               Chr6 <asp:TextBox ID="TextBox6" runat="server" Width="20px" Text="*"></asp:TextBox>
               Chr7 <asp:TextBox ID="TextBox7" runat="server" Width="20px" Text="*"></asp:TextBox>
               Chr8 <asp:TextBox ID="TextBox8" runat="server" Width="20px" Text="*"></asp:TextBox>
               Chr9 <asp:TextBox ID="TextBox9" runat="server" Width="20px" Text="*"></asp:TextBox>
               Chr10 <asp:TextBox ID="TextBox10" runat="server" Width="20px" Text="*"></asp:TextBox>
               <br /><br />             
               <asp:CheckBox ID="CheckBox1" runat="server" Text="(Check) for Needed or (Uncheck) for Not Wanted" />
               <br />
               <br />
                Instance Number#&nbsp;<asp:TextBox ID="TextBox11" runat="server"></asp:TextBox>
                &nbsp;<asp:Button ID="Button1" runat="server" Text="Test" />
                &nbsp;<asp:Label ID="LabelReply" runat="server" Text="Reply"></asp:Label>
               
               
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

