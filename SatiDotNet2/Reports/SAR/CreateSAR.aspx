<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="CreateSAR.aspx.vb" Inherits="Reports_SAR_CreateSAR" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Select a customer then the IDs&nbsp;
    <table>
        <tr>
            <td style="width: 193px; position: static">
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="True" DataSourceID="CustomersSqlDataSource"
                    DataTextField="Customer_Name" DataValueField="Customer_Name">
                </asp:RadioButtonList><br />
            </td>
            <td style="vertical-align: top; width: 100px; position: static; text-align: left">
                <asp:CheckBoxList ID="CheckBoxList1" runat="server" DataSourceID="IDsSqlDataSource"
                    DataTextField="MainID" DataValueField="MainID" Style="vertical-align: top; position: static;
                    text-align: left">
                </asp:CheckBoxList>
     <asp:Panel ID="Panel1" runat="server" Width="440px">
                 Email is still in development.<br />
                 You can use but format is subject to change.                 
                 <br />
                 <asp:CheckBox ID="CheckBoxExport" runat="server" 
                     Text="Export Data To Email - " />
                 &nbsp;<asp:TextBox ID="TextBoxEmailAddress" runat="server"></asp:TextBox>
                     @purewaferinc.com<br />
                <asp:Button ID="GoButton" runat="server" Text="Go" Visible="False" /><br />
                   </asp:Panel>  <br />
            </td>
        </tr>
    </table>
    <br />
    <asp:SqlDataSource ID="CustomersSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT Customer_Name FROM dbo.Customer GROUP BY Customer_Name"></asp:SqlDataSource>
    <asp:SqlDataSource ID="IDsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT dbo.Customer.Customer_Name, dbo.MainID.MainID FROM dbo.Customer INNER JOIN dbo.MainID ON dbo.Customer.CustomerID = dbo.MainID.CustomerID WHERE (dbo.Customer.Customer_Name = N'F')">
    </asp:SqlDataSource>
    <br />
</asp:Content>

