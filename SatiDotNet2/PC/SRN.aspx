<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SRN.aspx.vb" Inherits="PC_SRN" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">

                
                
                <table class="style1">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="SRN Infomation" Font-Size="X-Large" Font-Bold="True"></asp:Label>&nbsp;&nbsp;
                            <asp:TextBox ID="TextBoxSRN" runat="server"  Font-Size="X-Large" BackColor="#99CCFF" BorderStyle="Solid" Width="100"></asp:TextBox>&nbsp;&nbsp;
                            <asp:Button ID="ButtonLoadSRN" runat="server" Text="Load" />
                        </td>
                        <td style="text-align: right">
                            
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table class="style1">
                    <tr>
                        <td>Shipping ID: SO# PO# &nbsp;</td>
                        <td style="text-align: right">&nbsp;CofA</td>
                        <td>&nbsp;<asp:Button ID="ButtonPrintCofA" runat="server" Text="View" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="border-style: solid; border-width: thin; text-align: right">
                            <asp:RadioButton ID="RadioButtonOffSite" runat="server" Checked="True" Text="Off Site - Printer 4&amp;5&nbsp; " AutoPostBack="True" GroupName="Printers"  />&nbsp;&nbsp;
                            <asp:RadioButton ID="RadioButtonMainBuilding" runat="server" Checked="False" Text="Main Site - Printer 6&7&nbsp; " AutoPostBack="True" GroupName="Printers"  />&nbsp;&nbsp;
                            <asp:CheckBox ID="CheckBoxPrint1of1Label" runat="server" Text="&nbsp;1/1&nbsp;" BorderStyle="Dashed" BorderWidth="2px" />&nbsp;&nbsp;Print Labels</td>
                        <td>&nbsp;<asp:Button ID="Button1" runat="server" Text="Print" />&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td style="text-align: right">Packing Slip</td>
                       
                        <td>&nbsp;<asp:Button ID="Button2" runat="server" Text="View" /></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>Boxes Bound To SRN:&nbsp;</td>
                        <td>Shipping Detail:&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    
                    <tr>
                        <td><asp:ListBox ID="ListBoxCartons" runat="server" Width="224px"></asp:ListBox></td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                <br />
                <br />
                              
                    <asp:SqlDataSource ID="CarrierSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT Name FROM dbo.Carriers GROUP BY Name ORDER BY Name"></asp:SqlDataSource>

            </asp:Panel>
        </ContentTemplate>        
    </asp:UpdatePanel>
    
     
</asp:Content>

