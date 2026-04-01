<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="KPI.aspx.vb" Inherits="Reports_KPI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="Panel1" runat="server">
        
           <asp:Label ID="Label1" runat="server" Text="KPI Report" Font-Size="Larger"></asp:Label><br />
            <br />
            <table >
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Start Date"></asp:Label>
                    </td>
                    <td style="width: 135px">
                       
                    </td>
                    <td>
                         <asp:Label ID="Label3" runat="server" Text="End Date"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Calendar ID="CalendarStart" runat="server"></asp:Calendar>
                    </td>
                    <td style="width: 135px">
                        
                    </td>
                    <td>
                        <asp:Calendar ID="CalendarEnd" runat="server"></asp:Calendar>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; vertical-align: top">
                        Stage<br />
                        <asp:DropDownList ID="DropDownList1" runat="server" 
                            DataSourceID="SqlDataSource1" DataTextField="StageName" 
                            DataValueField="StageName" AutoPostBack="True">
                        </asp:DropDownList><br />
                        <asp:CheckBox ID="CheckBoxDia" runat="server" Text="300mm Only" Checked="True" /><br />
                        <asp:CheckBox ID="CheckBoxSP2" runat="server" Text="SP2 Data Only" Checked="True" />
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                            SelectCommand="SELECT StageName FROM dbo.UniqueProcesses WHERE (Complete &gt; CONVERT (DATETIME, '2013-01-01 00:00:00', 102)) GROUP BY StageName HAVING (NOT (StageName LIKE N'%Wip%')) ORDER BY StageName">
                        </asp:SqlDataSource>
                    </td>
                    <td style="width: 135px; text-align: left; vertical-align: top">
                        <asp:RadioButton ID="RadioButtonF" runat="server" Text="First Pass" GroupName="DT" Checked="True" /><br />
                        <asp:RadioButton ID="RadioButtonR" runat="server" Text="Rework" GroupName="DT" /><br />
                        <asp:RadioButton ID="RadioButtonB" runat="server" Text="Both" GroupName="DT" /><br />
                    </td>
                    <td style="text-align: left; vertical-align: top">
                        
                        
                        
                    </td>
                </tr>
                </table>
                
                <asp:Button ID="Button1" runat="server" Text="Run Report" /><br />
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <img src="../Color/Animated_LoadingBigger.gif" />Working...
                    </ProgressTemplate>
                </asp:UpdateProgress>
            <asp:HyperLink ID="HyperLinkReport" runat="server" Visible="False">View Report</asp:HyperLink><br />
            <br />
            
            
            
            
            
            
            
        
        </asp:Panel>
      
    </ContentTemplate>
    
    </asp:UpdatePanel>

</asp:Content>

