<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="WH_Rec_Info.aspx.vb" Inherits="PC_WH_Rec_Info" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>   
        <asp:Panel ID="PanelOut" runat="server">            
            
            <table>
                <tr>
                    <td><asp:Label ID="Label1" runat="server" Text="Edit SATI.Net Receiving Log" Font-Size="Larger"></asp:Label></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td>ID:&nbsp;<br /><asp:TextBox ID="TextBoxID" runat="server" Width="282px" ></asp:TextBox></td>
                </tr>
                <tr>
                    <td>WL:&nbsp;<br /><asp:TextBox ID="TextBoxWL" runat="server" Width="282px" ></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Qty:&nbsp;<br /><asp:TextBox ID="TextBoxQty" runat="server" Width="282px" ></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Packing Slip:&nbsp;<br /><asp:TextBox ID="TextBoxPackingSlip" runat="server" Width="282px" ></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Carrier:&nbsp;<br /><asp:TextBox ID="TextBoxCarrier" runat="server" Width="282px" ></asp:TextBox></td>                   
                </tr>
                <tr>
                    <td>Note:&nbsp;<br /><asp:TextBox ID="TextBoxNote" runat="server" Width="282px" ></asp:TextBox></td>                   
                </tr>
                <tr>
                    <td>&nbsp;</td>                    
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Sati Log Key:"></asp:Label>&nbsp;
                        <asp:Label ID="LabelSatiKey" runat="server" Text="Key"></asp:Label>
                        <br />
                        <asp:Label ID="Label3" runat="server" Text="Old Log Key:"></asp:Label>&nbsp;
                        <asp:Label ID="LabelOldKey" runat="server" Text="Key"></asp:Label>
                        <br />
                        <asp:Button ID="ButtonSave" runat="server" Text="Save" Width="282px" />
                        <br /><br />
                    </td>                                           
                </tr>
            </table>
        </asp:Panel>
        </ContentTemplate> 
    </asp:UpdatePanel>
</asp:Content>


