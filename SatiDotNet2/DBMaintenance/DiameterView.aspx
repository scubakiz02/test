<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="DiameterView.aspx.vb" Inherits="DBMaintenance_DiameterView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
            <asp:Panel ID="Panel1" runat="server">
                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Diameter View"></asp:Label><br />
                <br />
                      Enter 
                <asp:RadioButton ID="RadioButtonWB" runat="server" Text="WaferBox" GroupName="thetype"  Checked="True" /> &nbsp; or 
                <asp:RadioButton ID="RadioButtonI" runat="server" Text="Instance" GroupName="thetype" />&nbsp; 
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>&nbsp;
                <asp:Button ID="Button1" runat="server" Text="View" /><br />

                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
                    <Columns>
                        <asp:BoundField DataField="Diameter" HeaderText="Diameter" SortExpression="Diameter" ReadOnly="True" />
                        <asp:BoundField DataField="T7" HeaderText="T7" SortExpression="T7" />
                        <asp:BoundField DataField="InstanceKey" HeaderText="InstanceKey" SortExpression="InstanceKey" />
                        <asp:BoundField DataField="BoxInvNumber" HeaderText="BoxInvNumber" SortExpression="BoxInvNumber" InsertVisible="False" ReadOnly="True" />
                        <asp:BoundField DataField="CustomerID" HeaderText="CustomerID" SortExpression="CustomerID" />
                        <asp:BoundField DataField="MainID" HeaderText="MainID" SortExpression="MainID" />
                        <asp:BoundField DataField="Slot" HeaderText="Slot" SortExpression="Slot" />
                        <asp:ButtonField CommandName="Fix" Text="Fix" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT TOP (100) PERCENT ISNULL(Q_Diameter_T7_Active.Diameter, 300) AS Diameter, T7_WaferActionTracking.T7, T_FGI_Boxes.InstanceKey, T_FGI_Boxes.BoxInvNumber, MainID.CustomerID, MainID.MainID, T7_InstanceInfo.Slot FROM T7_WaferActionTracking INNER JOIN T7_InstanceInfo ON T7_WaferActionTracking.WAT_Key = T7_InstanceInfo.WAT_Key LEFT OUTER JOIN Q_Diameter_T7_Active ON T7_WaferActionTracking.WAT_Key = Q_Diameter_T7_Active.WAT_Key RIGHT OUTER JOIN T_FGI_Boxes LEFT OUTER JOIN LabelsMade INNER JOIN MainID ON LEFT (LabelsMade.Lot, 4) = MainID.MainID ON T_FGI_Boxes.LabelsMadeKey = LabelsMade.LabelRecordNumber ON T7_InstanceInfo.InstanceID = T_FGI_Boxes.InstanceKey WHERE (T_FGI_Boxes.BoxInvNumber = 1) ORDER BY Diameter">
                </asp:SqlDataSource>
            </asp:Panel>

        <asp:UpdateProgress id="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <IMG src="../Color/Animated_LoadingBigger.gif" />Working...
            </ProgressTemplate>
        </asp:UpdateProgress>
        </contenttemplate>
    </asp:UpdatePanel>    
</asp:Content>

