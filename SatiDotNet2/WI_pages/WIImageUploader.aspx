<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WIImageUploader.aspx.vb" Inherits="WI_WIImageUploader" %>

<!DOCTYPE html>
<html>
    <head runat="server">
        <title></title>
    </head>
    <body style="padding: 0px; margin: 0px;">
        <form id="form1" runat="server">
            <div>
                <table style="height: 30px">
                    <tr>
                        <td style="width: 10%; padding-right: 18px;">
                            <input type="text" id="ImgUpTextBox" runat="server" placeholder="Lot ID" style="width: 100%" onkeyup="passCheckNums(this); return false;" onchange="passCheckNums(this); return false;" />
                        </td>
                        <td style="width:5%">
                            <asp:Button runat="server" ID="cmdUpload" Text="Upload Image" style="position: relative; z-index: 1;" OnClick="cmdUpload_Click" OnClientClick="javascript:document.getElementById('lodingDiv').style.display = 'block';" AutoPostBack="false" Enabled="false"/>
                        </td>
                        <td style="width:30%">
                            <asp:FileUpload runat="server" ID="WIUploader" style="position: relative; z-index: 1; overflow: hidden;" Enabled="false"/>
                        </td>
                        <td style="width: 15%">
                            <asp:Label runat="server" ID="successMessage" style="width: 100%;"/>
                            <asp:HiddenField ID="ImgUpHiddenLotID" runat="server" value="nothing"/>
                        </td>
                        <td style="width:19.75%">
                            <asp:DropDownList ID="SelectImages" runat="server" AutoPostBack="true" Width="100%">
                            </asp:DropDownList>
                        </td>
                        <td style="width:0.25%"></td>
                        <td style="width:3%">
                            <button id="imageInsert" onclick="passInsertImage()" title="Insert Image or Textbox into selected Cell" style="width: 100%">&#x2714;</button>
                        </td>
                        <td style="width:3%">
                            <asp:Button ID="imageDelete" runat="server" Text="&#x2718;" title="Delete Image from List" style="width: 100%"/>
                        </td>
                        <td style="width: 1%">
                            <asp:Button runat="server" ID="NinjaListUpdater" Text="" style="width: 1px; visibility: hidden" AutoPostBack="false" OnClick="UpdateImageList"/>
                        </td>
                    </tr>
                </table>
            </div>
        </form>
        <script src="../scripts/WIScripts/WIImageScript.js" type="text/javascript" ></script>
    </body>
</html>