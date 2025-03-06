<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MakeLabels.aspx.vb" Inherits="Production_MakeLabels" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Production Labels"></asp:Label><br />
    <br />
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="\\PWI-40\docshare\Labels\SmallBoxLabel.xls">Small Box Label</asp:HyperLink><br />
    <br />
    
    <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/Production/MakeSurfScanWaferBoxLabel.aspx">New 300mm Surf Scan Waferbox Labels</asp:HyperLink>
    <br />
    <br />
    <br />
    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/Production/No_LPD_Data_300mm_WaferBox_Label.aspx">No LPD Data, Waferbox Labels (300mm Only)</asp:HyperLink><br />
    <br />
    <br />
    Reprints:<br />
    <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/Production/LabelRemakeWithCurrent.aspx">Reprint 300mm Label With Current Info</asp:HyperLink><br />
    <br />
    <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="~/Production/LabelRemakeWCurrentOldStyle.aspx">Reprint 200mm Or < Labels With Current Info</asp:HyperLink><br />
    <br />
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Production/ReprintLabel.aspx">Reprint Wafer Box Labels (300mm) </asp:HyperLink><br />
    <br />
</asp:Content>

