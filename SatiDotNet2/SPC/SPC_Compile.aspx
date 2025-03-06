<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SPC_Compile.aspx.vb" Inherits="SPC_SPC_Compile" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server">
                 <asp:Label ID="Label3" runat="server" Text="SPC Compile" Font-Bold="True" Font-Size="X-Large"></asp:Label><br />
                <br />
                &nbsp;&nbsp;Select a Department:&nbsp;
                <asp:DropDownList ID="DropDownListDepartments" runat="server" AutoPostBack="True" AppendDataBoundItems="True" DataSourceID="SqlDataSourceDepartments" DataTextField="Department" DataValueField="Department">
                    <asp:ListItem>Select...</asp:ListItem>
                 </asp:DropDownList>
                                  
                <br />
                <br />
                <asp:Panel ID="PanelTools" runat="server" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px" Visible="False">
                    Select a Tool: &nbsp;<asp:DropDownList ID="DropDownListTools" runat="server" AutoPostBack="True"></asp:DropDownList><br />                    
                    <asp:UpdateProgress id="UpdateProgress2" runat="server">
                        <ProgressTemplate>
                            &nbsp;<IMG src="../Color/Animated_LoadingBigger.gif" />Updating Records...
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <asp:Panel ID="PanelDCS" runat="server" Visible="False">
                        Select DSC (Date Shift Code)&nbsp;                        
                        <asp:DropDownList ID="DropDownList_Last_DSC" runat="server" AutoPostBack="True" Width="150px"></asp:DropDownList>&nbsp;&nbsp;
                        Last Records Using SPC Recipe:&nbsp;
                        <asp:Label ID="Label_SQLfunction" runat="server" Text="sql" style="display:none"></asp:Label>
                        <asp:Label ID="Label_OCAP_Message" runat="server" Text="" style="display:none"></asp:Label>

                    </asp:Panel>
                    
                </asp:Panel>
                <br />
                <asp:Panel ID="PanelData" runat="server" BackColor="#0099CC" BorderColor="#0099CC" BorderWidth="10px" Visible="False">
                    LCL (Lower Control Limit)   UCL (Upper Control Limit)<br />
                    <asp:GridView ID="GridViewData" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" >
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />                       
                        
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView><br />

                    <asp:Panel ID="PanelSubmit" BackColor="#33CC33" BorderColor="#33CC33" BorderWidth="10px" runat="server">
                        <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" Visible="false" />
                        &nbsp;&nbsp;
                        <asp:Label ID="Label_OCAP" runat="server" Text="Label"></asp:Label>
                    </asp:Panel>
                    <asp:Panel ID="PanelCompleted" BorderWidth="10px" runat="server" BorderColor="#0C7472" BackColor="#0C7472" Visible="False">


                    </asp:Panel>
                    <asp:GridView ID="GridView1" runat="server"></asp:GridView>
                    
                </asp:Panel>
            </asp:Panel>
            
            <br /><br />


            <asp:SqlDataSource ID="SqlDataSourceDepartments" runat="server" ConnectionString="<%$ ConnectionStrings:SATI_SPCConnectionString %>" SelectCommand="SELECT Department FROM T_SPC_Tool_Info WHERE (Enable = 1) GROUP BY Department ORDER BY Department"></asp:SqlDataSource>
                 
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

