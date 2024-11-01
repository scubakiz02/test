<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="MR_Group_List_Managment.aspx.vb" Inherits="MR_MR_Group_List_Managment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                <asp:Label ID="Label1" runat="server" Text="Maintenance Request List and Grouping Managment"></asp:Label><br />
                <br /> 
                Select Type:<br />
                <asp:RadioButton ID="RadioButtonNormal" GroupName="Type" Text="Normal" runat="server" AutoPostBack="true" />
                <asp:RadioButton ID="RadioButtonReport" GroupName="Type" Text="Reports" runat="server" AutoPostBack="true" />
                <asp:RadioButton ID="RadioButtonAddNew" GroupName="Type" Text="Add New" runat="server" AutoPostBack="true" /><br/>
                <br />                
                <asp:Panel ID="PanelNew" runat="server" Visible="False">
                    <asp:Label ID="Label2" runat="server" Text="Type Name Of New Group Name:"></asp:Label><br />
                    <asp:TextBox ID="TextBoxNewGroupListName" runat="server" Width="250"></asp:TextBox><asp:CheckBox ID="CheckBoxNewGroupList_ReportOrNot" runat="server" Text="Is This Report Group?" /> <br />
                    <br />                    
                    <asp:Label ID="Label3" runat="server" Text="Select The Tool For The Group:"></asp:Label>
                    <asp:CheckBoxList ID="CheckBoxListNewGroupTools" runat="server">
                    </asp:CheckBoxList><br />
                    <asp:Button ID="ButtonMakeNewGroup" runat="server" Text="Make New Group" />
                    <asp:Label ID="LabelMakeNewGroupFeedBack" runat="server" Text=""></asp:Label>
                </asp:Panel>
                
                <asp:Panel ID="PanelView" runat="server" Visible="False">
                    <asp:DropDownList ID="DropDownListGroups" runat="server" Width="150px" AutoPostBack="True"></asp:DropDownList><br />
                    <asp:CheckBoxList ID="CheckBoxListViewedGroup" runat="server"></asp:CheckBoxList><br />
                    <asp:Button ID="ButtonSaveChange" runat="server" Text="Save Changes" Visible="False" /><br />
                
                </asp:Panel>
            
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

