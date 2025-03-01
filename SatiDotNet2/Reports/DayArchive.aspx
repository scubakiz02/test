<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="DayArchive.aspx.vb" Inherits="Reports_DayArchive" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Style="clear: none; position: static" Width="915px">
                Select A Day<br />
                <table>
                    <tr>
                        <td style="vertical-align: top; width: 100px; text-align: left">
                <asp:GridView ID="GridView1" runat="server" CellPadding="4" DataSourceID="SqlDataSource1"
                    ForeColor="#333333" GridLines="None" Width="544px">
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#EFF3FB" />
                    <EditRowStyle BackColor="#2461BF" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="LightBlue" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT ID, WH, WIP, Rework, FGI FROM dbo.fctn_Sati_HistorySlice('4/1/2007') AS fctn_Sati_HistorySlice_1">
                </asp:SqlDataSource>
                        </td>
                        <td style="vertical-align: top; width: 100px; text-align: left">
                <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                <ProgressTemplate>
                                    <img src="../Color/Animated_LoadingBigger.gif" />
                                    Updating...
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                &nbsp;</asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    &nbsp;
</asp:Content>

