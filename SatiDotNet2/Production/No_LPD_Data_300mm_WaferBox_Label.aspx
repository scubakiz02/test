<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="No_LPD_Data_300mm_WaferBox_Label.aspx.vb" Inherits="Production_No_LPD_Data_300mm_WaferBox_Label" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">     
       <ContentTemplate> 

       <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Make (None LPD) 300mm Labels"></asp:Label><br />
           <asp:Panel ID="Panel1" runat="server">
                <br /><br />
                <asp:TextBox ID="LotNumberTextBox" runat="server" Width="150px" BackColor="Red" AutoPostBack="True"></asp:TextBox><---Scan Runsheet Lot Number<br />
                <br />
                <asp:TextBox ID="InstanceNumberTextBox" runat="server" Width="150px" BackColor="Red" AutoPostBack="True"></asp:TextBox><---Scan Instance Number<br />
                <br />
                <asp:DropDownList id="PrinterDropDownList" runat="server" Width="150px" BackColor="lime"><asp:ListItem>Select Printer...</asp:ListItem>
                    <asp:ListItem Selected="True">Zebra1</asp:ListItem>
                    <asp:ListItem>Zebra2</asp:ListItem>
                    <asp:ListItem>Zebra_2B</asp:ListItem>
                    <asp:ListItem>Zebra9</asp:ListItem>

                </asp:DropDownList><---Select Printer<br />
               <br />
               <asp:Button ID="MakeLabelButton" runat="server" Text="Make Label" Visible="False" />
                <asp:Label ID="FeedBackLabel" runat="server" Text=""></asp:Label>
                <br />
               <br />
               <br />
               <br />

           </asp:Panel>
    
       </ContentTemplate>
    </asp:UpdatePanel>     
</asp:Content>


