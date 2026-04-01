<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ReprintLabel.aspx.vb" Inherits="Production_ReprintLabel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text='Wafer Box Label Reprint'></asp:Label><br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" BackColor="#C0FFC0" Font-Bold="True" 
                Width="485px">
                <br />
                This tool will take a 300mm Wafer Box and reprint the label Just Like It Was<br />
                <br />
                
            </asp:Panel>
            <asp:Panel ID="Panel2" runat="server" Width="481px">
                Select Printer<br />
                <asp:DropDownList ID="PrinterDropDownList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="PrinterDropDownList_SelectedIndexChanged">
                    <asp:ListItem Selected="True">Select Printer...</asp:ListItem>
                    <asp:ListItem>Zebra1</asp:ListItem>
                    <asp:ListItem>Zebra2</asp:ListItem>
                    <asp:ListItem>Zebra_2B</asp:ListItem>
                    <asp:ListItem>Zebra3</asp:ListItem>
                    <asp:ListItem>Zebra4</asp:ListItem>
                    <asp:ListItem>Zebra6</asp:ListItem>          
                    <asp:ListItem>Zebra9</asp:ListItem>     
                </asp:DropDownList><br />
                <br />
                <asp:Panel ID="WBScanPanel" runat="server" Visible="False" Width="240px">
                    Scan 300mm Wafer Box label<br />
                    <asp:TextBox ID="WBScanTextBox" runat="server" AutoPostBack="True" OnTextChanged="TextBox1_TextChanged"></asp:TextBox></asp:Panel>
                <br />
                Info
                <br />
                <asp:TextBox ID="InfoTextBox" runat="server" Height="136px" TextMode="MultiLine"
                    Width="476px"></asp:TextBox><br />
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        Making...
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

