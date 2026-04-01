<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="PO_SO_Managment.aspx.vb" Inherits="Sales_PO_SO_Managment" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelPageFill" runat="server">              
                
                <asp:Panel ID="PanelHeader" runat="server">                    
                    &nbsp;&nbsp;<asp:Label ID="Label1" runat="server" Text="PO SO Managment" Font-Bold="True" Font-Size="Large"></asp:Label>                    
                    <cc1:RoundedCornersExtender ID="PanelMain_RoundedCornersExtender" runat="server" BehaviorID="PanelMain_RoundedCornersExtender" TargetControlID="PanelHeader" BorderColor="Black" Color="SlateGray" Radius="10">
                    </cc1:RoundedCornersExtender>
                </asp:Panel>                
                 <br />
                <!--************** Main Panel ************************* --> 
                <asp:Panel ID="PanelMain" runat="server"> 
                    <cc1:RoundedCornersExtender ID="PanelMain_RoundedCornersExtender2" runat="server" BehaviorID="PanelMain_RoundedCornersExtender2" TargetControlID="PanelMain" BorderColor="Black" Color="SteelBlue" Radius="10">
                    </cc1:RoundedCornersExtender>

                        <asp:Panel ID="Panel1" runat="server" BorderStyle="Solid" BorderColor="Black">
                            <table class="style1" >
                                <tr>
                                    <td style="text-align: left">Select FAB:&nbsp;
                                        <asp:DropDownList ID="DropDownListFABs" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="CustomerID" DataValueField="CustomerID" AppendDataBoundItems="True">
                                            <asp:ListItem>Select...</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RadioButton ID="RadioButtonPast" runat="server" AutoPostBack="True" Text="Past" GroupName="WhenGroup" />
                                        <asp:RadioButton ID="RadioButtonCutternt" runat="server" AutoPostBack="True" Text="Current " GroupName="WhenGroup" Checked="True" />
                                        <asp:RadioButton ID="RadioButtonFuture" runat="server" AutoPostBack="True" Text="Future " GroupName="WhenGroup" Visible="false" />
                                    </td>
                                   
                                    <td style="text-align: right">
                                        <asp:HyperLink ID="HyperLinkReport" runat="server" Visible="false">Open Report</asp:HyperLink>
                                        <asp:Button ID="ButtonRunReport" runat="server" Text="Run Report" />
                                        <asp:Button ID="ButtonAddSO" runat="server" Visible="false" Text="Add So" />
                                    </td>
                                </tr>
                            </table>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT CustomerID FROM MainID WHERE (ExpirationDtd IS NULL) OR (ExpirationDtd &gt; { fn NOW() }) GROUP BY CustomerID"></asp:SqlDataSource>               
                            
                            
                        </asp:Panel>


                    <asp:Panel ID="PanelDetails" runat="server">
                        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="1"><!--**** Start of the Multi View -->
                            <!-- Past -->
                            <asp:View ID="ViewPast" runat="server">
                                
                            </asp:View>

                            <!-- Current -->
                            <asp:View ID="ViewCurrent" runat="server">
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"  CellPadding="3" DataKeyNames="MainID" DataSourceID="SqlDataSourceCurrentSOs" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                                    <Columns>
                                        <asp:ButtonField ButtonType="Button" Text="Edit" commandName="EditSO" />
                                        <asp:TemplateField HeaderText="MainID" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" Text='<%# Eval("MainID") %>' CommandName="ViewSharedIDs" CausesValidation="false" ID="LinkButtonMainID" CommandArgument='<%# Eval("MainID") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="SO" HeaderText="SO" SortExpression="SO" />
                                        <asp:BoundField DataField="PO" HeaderText="PO" SortExpression="PO" />
                                        <asp:BoundField DataField="PO Qty" HeaderText="PO Qty" SortExpression="PO Qty" />
                                        <asp:BoundField DataField="Past SO" HeaderText="Past SO" SortExpression="Past SO" NullDisplayText=" " />
                                        <asp:BoundField DataField="EffectiveDtd" HeaderText="EffectiveDtd" SortExpression="EffectiveDtd" />
                                        <asp:BoundField DataField="ExpirationDtd" HeaderText="ExpirationDtd" SortExpression="ExpirationDtd" NullDisplayText=" "/>
                                        <asp:TemplateField HeaderText="Shipped" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" Text='<%# Eval("Shipped") %>' CommandName="ViewShipments" CausesValidation="false" ID="LinkButtonShipped" CommandArgument='<%# Eval("SO") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="Balance" HeaderText="Balance" ReadOnly="True" SortExpression="Balance" />
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <RowStyle ForeColor="#000066" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                                </asp:GridView>
                                
                                <asp:SqlDataSource ID="SqlDataSourceCurrentSOs" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT TOP (100) PERCENT dbo.MainID.MainID, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number AS PO, dbo.SO_LineItems.Qty AS [PO Qty], dbo.SO_Info.SO_Replaced AS [Past SO], dbo.SO_Info.EffectiveDtd, dbo.SO_Info.ExpirationDtd, SUM(dbo.Pick_Log.Total_Qty) AS [Shipped], dbo.SO_LineItems.Qty - SUM(dbo.Pick_Log.Total_Qty) AS Balance FROM dbo.MainID INNER JOIN dbo.SO_LineItems ON dbo.MainID.MainID = dbo.SO_LineItems.MainID INNER JOIN dbo.SO_Info ON dbo.SO_LineItems.SO = dbo.SO_Info.SO INNER JOIN dbo.Pick_Log ON dbo.SO_Info.SO = dbo.Pick_Log.SO WHERE (dbo.MainID.CustomerID = N'Blank') GROUP BY dbo.MainID.MainID, dbo.SO_LineItems.SO, dbo.SO_Info.PO_Number, dbo.SO_Info.SO_Replaced, dbo.SO_Info.EffectiveDtd, dbo.SO_Info.ExpirationDtd, dbo.SO_LineItems.Qty HAVING (dbo.SO_Info.ExpirationDtd IS NULL OR dbo.SO_Info.ExpirationDtd &gt; { fn NOW() }) ORDER BY dbo.MainID.MainID"></asp:SqlDataSource>
                                   
                            </asp:View>
                            
                            <!-- Future -->
                            <asp:View ID="ViewFuture" runat="server">
                                
                            </asp:View>

                        </asp:MultiView>
                    </asp:Panel>


                    <!--************************************************************************************************************************* -->
                    <!--******************************************** Start Edit Panel *********************************************************** -->
                    <!--************************************************************************************************************************* -->
                    <asp:Button ID="ButtonShow" runat="server" Text="Show" style="display:none" />
                    
                    <asp:Panel ID="PanelSOEdit" runat="server" Width="500" BackColor="#00cc66" BorderColor="Black" BorderStyle="Solid"  HorizontalAlign="Center" >
                       <br />

                        This Will Edit SO# &nbsp; <asp:Label ID="LabelEditSO" runat="server" Text="SO" Font-Bold="true"></asp:Label>.<br />
                         &nbsp; 
                        <table class="style1">
                            <tr>
                                <td style="text-align: right">Main ID#&nbsp;</td>
                                <td style="text-align: Left"><asp:TextBox ID="TextBoxEdit_MainID" runat="server" Width="200"></asp:TextBox>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="text-align: right">PO #&nbsp;</td>
                                <td style="text-align: Left"><asp:TextBox ID="TextBoxEdit_PO" runat="server" Width="200"></asp:TextBox>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="text-align: right">PO Qty&nbsp;</td>
                                <td style="text-align: Left"><asp:TextBox ID="TextBoxEdit_PO_Qty" runat="server" Width="200"></asp:TextBox>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="text-align: right">Past SO#&nbsp;</td>
                                <td style="text-align: Left"><asp:TextBox ID="TextBoxEdit_Past_SO" runat="server" Width="200"></asp:TextBox>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="text-align: right">Effective Date&nbsp;</td>
                                <td style="text-align: Left"><asp:TextBox ID="TextBoxEdit_Eff_Date" runat="server" Width="200"></asp:TextBox>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="text-align: right">Expiration Date&nbsp;</td>
                                <td style="text-align: Left"><asp:TextBox ID="TextBoxEdit_Exp_Date" runat="server" Width="200" Text="date"></asp:TextBox>&nbsp;</td>
                            </tr>
                            
                        </table>
                        <br />
                        <asp:Button ID="ButtonSaveSOEdit" runat="server" Text="Save" />&nbsp;&nbsp; 
                        <asp:Button ID="ButtonMakeNewSO" runat="server" Text="Expire & Make New"  />&nbsp;&nbsp;
                        <asp:Button ID="ButtonCloseSOEdit" runat="server" Text="Close" />
                        <br />
                    </asp:Panel>

                    <cc1:ModalPopupExtender 
                        ID="PanelSOEdit_ModalPopupExtender" 
                        runat="server" 
                        BehaviorID="PanelSOEdit_ModalPopupExtender" 
                        DynamicServicePath="" 
                        TargetControlID="ButtonShow"
                        PopupControlID="PanelSOEdit"
                        OkControlID="ButtonCloseSOEdit">
                    </cc1:ModalPopupExtender>
                
                    <!--************************************************************************************************************************* -->
                    <!--******************************************** End Edit Panel ************************************************************* -->
                    <!--************************************************************************************************************************* -->



                    <!--************************************************************************************************************************* -->
                    <!--******************************************* Start View Shared ID Panel ************************************************** -->
                    <!--************************************************************************************************************************* -->
                    
                    <asp:Button ID="ButtonShowShareID" runat="server" Text="Show" style="display:none" />
                    
                    <asp:Panel ID="PanelViewSharedIDs" runat="server" Width="500" BackColor="#66CCFF" BorderColor="Black" BorderStyle="Solid" HorizontalAlign="Center">
                        <br />
                        <br />
                        
                        Child ID List for <asp:Label ID="LabelMainIDforSharedID" runat="server" Text="Label"></asp:Label><br />
                        <asp:GridView ID="GridViewSharedIDs" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            DataKeyNames="SO_MainID,Child_MainID" DataSourceID="SharedIDsSqlDataSource" ForeColor="#333333"
                            GridLines="None" HorizontalAlign="Center">
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#EFF3FB" />
                            <Columns>
                                <asp:BoundField DataField="SO_MainID" HeaderText="Main ID" ReadOnly="True" SortExpression="SO_MainID" />
                                <asp:BoundField DataField="Child_MainID" HeaderText="Child ID" ReadOnly="True" SortExpression="Child_MainID" />
                                <asp:CommandField ShowDeleteButton="True">
                                    <ItemStyle ForeColor="Red" />
                                </asp:CommandField>
                            </Columns>
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                        <br />
                        
                        Add Child ID<br />
                        <asp:DropDownList ID="ChildIDDropDownList" runat="server" DataSourceID="IDsSqlDataSource"
                            DataTextField="MainID" DataValueField="MainID" Width="112px">
                        </asp:DropDownList>&nbsp;
                        <asp:Button ID="AddChildIDButton" runat="server" OnClick="AddChildIDButton_Click" Text="Add" /><br />
                        <br />                    
                        <br />
                        <asp:Button ID="ButtonCloseShareID" runat="server" Text="Close" />
                        <br />
                        <br />
                        
                        <asp:SqlDataSource ID="IDsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                            SelectCommand="SELECT MainID, CustomerID FROM dbo.MainID WHERE (CustomerID = N'none')">
                        </asp:SqlDataSource>
                        <asp:SqlDataSource ID="SharedIDsSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                            DeleteCommand="DELETE FROM [MainID_SO_LineItems] WHERE [SO_MainID] = @SO_MainID AND [Child_MainID] = @Child_MainID"
                            InsertCommand="INSERT INTO [MainID_SO_LineItems] ([SO_MainID], [Child_MainID]) VALUES (@SO_MainID, @Child_MainID)"
                            SelectCommand="SELECT [SO_MainID], [Child_MainID] FROM [MainID_SO_LineItems]">
                            <DeleteParameters>
                                <asp:Parameter Name="SO_MainID" Type="String" />
                                <asp:Parameter Name="Child_MainID" Type="String" />
                            </DeleteParameters>
                            <InsertParameters>
                                <asp:Parameter Name="SO_MainID" Type="String" />
                                <asp:Parameter Name="Child_MainID" Type="String" />
                            </InsertParameters>
                        </asp:SqlDataSource>

                    </asp:Panel>

                    <cc1:ModalPopupExtender
                        ID="ModalPopupExtender1" 
                        runat="server" 
                        DynamicServicePath="" 
                        BehaviorID="PanelViewSharedIDs_ModalPopupExtender" 
                        TargetControlID="ButtonShowShareID" 
                        PopupControlID="PanelViewSharedIDs"
                        OkControlID="ButtonCloseShareID">

                    </cc1:ModalPopupExtender>
                    <!--************************************************************************************************************************* -->
                    <!--******************************************** End View Shared ID Panel *************************************************** -->
                    <!--************************************************************************************************************************* -->



                    <!--************************************************************************************************************************* -->
                    <!--******************************************* Start View Shipped Panel **************************************************** -->
                    <!--************************************************************************************************************************* -->
                    
                    <asp:Button ID="ButtonShowShipped" runat="server" Text="Show" style="display:none" />

                    <asp:Panel ID="PanelViewShipped" runat="server" Width="500" BackColor="#FFCC66" HorizontalAlign="Center" BorderColor="Black" BorderStyle="Solid" ScrollBars="Vertical" Height="300">
                        <br />
                        <asp:GridView ID="GridViewShipped" runat="server" AutoGenerateColumns="False" DataKeyNames="PickTicket" DataSourceID="SqlDataSourceShipped" HorizontalAlign="Center">
                            <Columns>
                                <asp:BoundField DataField="PickTicket" HeaderText="PickTicket" ReadOnly="True" SortExpression="PickTicket"></asp:BoundField>
                                <asp:BoundField DataField="Qty" HeaderText="Qty" SortExpression="Qty"></asp:BoundField>
                                <asp:BoundField DataField="EventTime" HeaderText="EventTime" SortExpression="EventTime"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <asp:SqlDataSource runat="server" ID="SqlDataSourceShipped" ConnectionString='<%$ ConnectionStrings:ALTSConnectionString %>' SelectCommand="SELECT PickTicket, Total_Qty AS Qty, EventTime FROM Pick_Log WHERE (SO = N'0')"></asp:SqlDataSource>
                        
                        <asp:Button ID="ButtonCloseViewShipped" runat="server" Text="Close" />
                        <br />
                    </asp:Panel>

                    <cc1:ModalPopupExtender 
                        ID="PanelViewShipped_ModalPopupExtender"
                        runat="server" 
                        DynamicServicePath="" 
                        BehaviorID="PanelViewShipped_ModalPopupExtender" 
                        TargetControlID="ButtonShowShipped" 
                        PopupControlID="PanelViewShipped"                        
                        OkControlID="ButtonCloseViewShipped">
                    </cc1:ModalPopupExtender>
                    <!--************************************************************************************************************************* -->
                    <!--******************************************* End View Shipped Panel ****************************************************** -->
                    <!--************************************************************************************************************************* -->



                    <!--************************************************************************************************************************* -->
                    <!--******************************************* Start Enter PO / SO Panel *************************************************** -->
                    <!--************************************************************************************************************************* -->
                    <asp:Button ID="ButtonShowPO" runat="server" Text="Show" style="display:none" />
                    <asp:Panel ID="PanelEnterPO" runat="server" Width="500" BackColor="#669999" HorizontalAlign="Center" BorderColor="Black" BorderStyle="Solid" >
                         <br />

                        Enter New SO# <br />
                        
                        <table class="style1">
                             <tr>
                                <td style="text-align: right">Main ID#&nbsp;</td>
                                <td style="text-align: Left">
                                    <asp:DropDownList ID="DropDownListEnterID"  runat="server" DataSourceID="IDsSqlDataSource"
                                        DataTextField="MainID" DataValueField="MainID" Width="200">
                                    </asp:DropDownList> &nbsp;                                  
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">SO#&nbsp;</td>
                                <td style="text-align: Left"><asp:TextBox ID="TextBoxEnterSO" runat="server" Width="200"></asp:TextBox>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="text-align: right">PO #&nbsp;</td>
                                <td style="text-align: Left"><asp:TextBox ID="TextBoxEnterPO" runat="server" Width="200"></asp:TextBox>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="text-align: right">PO Qty&nbsp;</td>
                                <td style="text-align: Left"><asp:TextBox ID="TextBoxEnterPO_Qty" runat="server" Width="200"></asp:TextBox>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="text-align: right">Past SO#&nbsp;</td>
                                <td style="text-align: Left"><asp:TextBox ID="TextBoxEnterPastSO" runat="server" Width="200"></asp:TextBox>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="text-align: right">Effective Date&nbsp;</td>
                                <td style="text-align: Left"><asp:TextBox ID="TextBoxEnterEffectiveDate" runat="server" Width="200"></asp:TextBox>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="text-align: right">Expiration Date&nbsp;</td>
                                <td style="text-align: Left"><asp:TextBox ID="TextBoxEnterExpirationDate" runat="server" Width="200"></asp:TextBox>&nbsp;</td>
                            </tr>
                            
                        </table>
                        <br />
                        
                        <asp:Button ID="ButtonSavePO" runat="server" Text="Save" />
                        <asp:Button ID="ButtonCloseEnterPO" runat="server" Text="Close" />

                        
                    </asp:Panel>
                        <cc1:ModalPopupExtender 
                            ID="PanelEnterPO_ModalPopupExtender"
                            runat="server" 
                            DynamicServicePath="" 
                            BehaviorID="PanelEnterPO_ModalPopupExtender" 
                            TargetControlID="ButtonShowPO" 
                            PopupControlID="PanelEnterPO" 
                            OkControlID="ButtonCloseEnterPO">
                        </cc1:ModalPopupExtender>
                    <!--************************************************************************************************************************* -->
                    <!--******************************************* End Enter PO / SO  Panel **************************************************** -->
                    <!--************************************************************************************************************************* -->
                    
                    
                </asp:Panel>
               

            </asp:Panel>         

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

