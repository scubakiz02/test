<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MakeCartonNew.aspx.vb" Inherits="PC_MakeCartonNew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
            <asp:Panel ID="Panel1" runat="server">
                <asp:Label ID="Label2" runat="server" Text="Make Wafer Box Carton" Font-Size="Larger"></asp:Label> <br />
                <table class="style1">
                    <tr>
                        <td width ="300" valign="top">
                            
                                Scan Wafer Box Label &nbsp;&nbsp; <br />
                                
                                <asp:TextBox ID="TextBoxScanInput" runat="server" AutoPostBack="True" BackColor="#99FFCC" Width="200px" ></asp:TextBox> <br />
                           
                                <asp:CheckBox ID="CheckBoxReprint" runat="server" Text="Reprint Carboard Box Label" /> <br />
                                <br />
                                Select Printer <br />
                                <asp:DropDownList id="PrinterDropDownList" runat="server" Width="160px"><asp:ListItem>Select Printer...</asp:ListItem>
                                    <asp:ListItem>Zebra4</asp:ListItem>
                                    <asp:ListItem>Zebra5</asp:ListItem>
                                    <asp:ListItem Selected="True">Zebra6</asp:ListItem>
                                    <asp:ListItem>Zebra7</asp:ListItem>
                                    <asp:ListItem>Zebra8</asp:ListItem>
                                </asp:DropDownList><br />
                                <br />
                                <br />
                                Boxes added =
                                <asp:Label ID="LabelBoxQty" runat="server" Text="0"></asp:Label>
                                &nbsp; of &nbsp; 
                                <asp:Label ID="LabelBoxQtyMax" runat="server" Text="0"></asp:Label><br />
                                <asp:TextBox id="WaferBoxTextBox" runat="server" TextMode="MultiLine" Height="176px" AutoPostBack="True" ></asp:TextBox> <br />
                                <asp:Button id="Button1" runat="server" Text="Make Carton Label" OnClick="Button1_Click"></asp:Button> <br />
                            

                        </td>
                        <td  valign="top">
                            
                                                              
                                Lot ID: &nbsp; <asp:Label ID="Label_Lot_ID" runat="server" Text="0"></asp:Label> <br />
                                Units/Carton: &nbsp; <asp:Label ID="LabelUnits" runat="server" Text="0"></asp:Label> <br />
                                <br />
                                <strong>Spec Key:</strong> &nbsp; <asp:Label ID="Label_Spec_Key" runat="server" Text="0" Font-Bold="true"></asp:Label> <br />                                
                                Spec #: &nbsp; <asp:Label ID="Label_Spec" runat="server" Text="0"></asp:Label> <br />
                                Spec Rev: &nbsp; <asp:Label ID="Label_Spec_Rev" runat="server" Text="0"></asp:Label> <br />                                
                                Part #: &nbsp; <asp:Label ID="Label_Part" runat="server" Text="0"></asp:Label> <br />
                                Part Rev: &nbsp; <asp:Label ID="Label_Part_Rev" runat="server" Text="0"></asp:Label> <br />
                                <br />                                
                                <strong>SO Key:</strong>  &nbsp; <asp:Label ID="Label_SO_Key" runat="server" Text="0" Font-Bold="true"></asp:Label> <br />                                
                                SO #: &nbsp; <asp:Label ID="Label_SO" runat="server" Text="0"></asp:Label> <br />
                                PO #: &nbsp; <asp:Label ID="Label_PO" runat="server" Text="0"></asp:Label> <br />
                                <br />
                                Scan Info:<asp:Label ID="LabelCarton" runat="server" Text="0"></asp:Label><br />
                                <asp:TextBox ID="TextBoxScanInfo" TextMode="MultiLine" Height="100px" Width="400" BackColor="White" runat="server" ></asp:TextBox>


                            

                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="300">&nbsp;</td>
                        <td valign="top">&nbsp;</td>
                    </tr>
                </table>
                <br />

                
    



                <asp:UpdateProgress id="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <IMG src="../Color/Animated_LoadingBigger.gif" />Working...
                    </ProgressTemplate>
                </asp:UpdateProgress>


            </asp:Panel>

        </contenttemplate>
    </asp:UpdatePanel>    
</asp:Content>

