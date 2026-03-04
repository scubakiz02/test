<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false"  CodeFile="MakeSurfScanWaferBoxLabel.aspx.vb" Inherits="Production_MakeSurfScanWaferBoxLabel" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server" >
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        
        <ContentTemplate> 
        
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Make 300mm Labels"></asp:Label>
        <br />
            <asp:Panel ID="Panel4" runat="server" BackColor="#33CCCC" BorderColor="#33CCCC" BorderWidth="10px">
                 <table>
                     <tr>
                         <td style="vertical-align: top; text-align: left;" >
                            <table >
                                <tr >
                                    <td style="width: 125px; vertical-align: top; text-align: left;" >
                                         Select Surf Scan<br />
                                        <asp:ListBox ID="ListBoxTool" runat="server" Width="125px" AutoPostBack="True"  Rows="3" Height="85px">
                                            <asp:ListItem Value="SP1">SP1-1 (T2)</asp:ListItem>
                                            <asp:ListItem Value="SP1-3">SP1-3 (T9)</asp:ListItem>
                                            <asp:ListItem Value="SP2-S0132">SP2 (T6)</asp:ListItem>
                                            <asp:ListItem Value="SP3-2110224">SP3-1</asp:ListItem>
                                            <asp:ListItem Value="SP3-2110164">SP3-2</asp:ListItem>
                                            <asp:ListItem Value="SP5-2130406">SP5-1</asp:ListItem>
                                        </asp:ListBox><br />
                                        <br />
                                        
                                    </td>
                                    <td style="width: 125px; vertical-align: top; text-align: left;" >
                                        Select Station<br />
                                        <asp:ListBox ID="ListBoxStation" runat="server" Width="125px" AutoPostBack="True"   Rows="3" Height="85px">
                                            <asp:ListItem>2</asp:ListItem>
                                            <asp:ListItem>3</asp:ListItem>
                                        </asp:ListBox><br />
                                        <br />
                                        
                                    </td>
                                                                   
                                </tr>
                                <tr>
                                    <td>
                                        Select Printer<br />
                                        <asp:DropDownList id="DropDownListPrinter" Width="125px" runat="server"  AutoPostBack="True">
                                            <asp:ListItem>Select Printer...</asp:ListItem>
                                            <asp:ListItem Selected="True">Zebra1</asp:ListItem>
                                            <asp:ListItem>Zebra2</asp:ListItem>
                                            <asp:ListItem>Zebra_2B</asp:ListItem>
                                            <asp:ListItem>Zebra9</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="Button_Pull_Data" runat="server" Width="125px" Height="40px" Text="Pull Scribe Data" Visible="false"/>
                                    </td>
                                </tr>
                                <tr >
                                    <td colspan="2" style="width: 250px; vertical-align: top; text-align: left;" >
                                        <asp:Panel ID="Panel_000" runat="server">
                                            <asp:UpdateProgress id="UpdateProgress2" runat="server">
                                                <ProgressTemplate>
                                                    &nbsp;<IMG src="../Color/Animated_LoadingBigger.gif" />Loading...
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </asp:Panel>
                                            <asp:Panel ID="PanelInfo" runat="server">
                                                <asp:Label ID="Label2" runat="server" Text="Info"></asp:Label><br />
                                                <asp:TextBox ID="TextBoxInfo" runat="server" BackColor="#D2C4C1" TextMode="MultiLine" Width="250px" BorderStyle="None" Height="100" ></asp:TextBox><br />
                                                <br />
                                                <br />
                                                <asp:Button ID="ButtonPrint" runat="server" Text="Print" Width="250px" Height="100px" Font-Size="XX-Large" Font-Bold="True" Visible="false"/>
                                                
                                            </asp:Panel>                       
                                    </td>                                           
                                </tr>

                    
                            </table>
                         </td>
                         <td style="vertical-align: top; text-align: left;" >
                            <asp:Panel ID="PanelScribeCheck" runat="server"  BackColor="LightGreen" BorderColor="LightGreen" BorderWidth="10px" Visible="false">
                                <div align="center">
                                    <asp:Label ID="LabelWhat" runat="server" Text="Label" Font-Bold="True" Font-Size="Larger"></asp:Label>
                                </div>
                                
                               
                                <asp:Panel ID="PanelScribes" runat="server">
                                   
                                    <table>
                                        <tr>
                                            <td>&nbsp; Instance &nbsp; </td>
                                            <td style="text-align: right;">&nbsp; Slot</td>
                                            <td>&nbsp; Scribe &nbsp; </td>
                                            <td><asp:Label ID="Labelhidden00" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr> 
                                        <tr>
                                            <td><asp:Label ID="LabelInstance25" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot25" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe25" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe25_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe25" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance24" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot24" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe24" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe24_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe24" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance23" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot23" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe23" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe23_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe23" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance22" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot22" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe22" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe22_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe22" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance21" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot21" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe21" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe21_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe21" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance20" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot20" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe20" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe20_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe20" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance19" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot19" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe19" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe19_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe19" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance18" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot18" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe18" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe18_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe18" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance17" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot17" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe17" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe17_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe17" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance16" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot16" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe16" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe16_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe16" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance15" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot15" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe15" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe15_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe15" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance14" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot14" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe14" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe14_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe14" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance13" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot13" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe13" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe13_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe13" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance12" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot12" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe12" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe12_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe12" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance11" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot11" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe11" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe11_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe11" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance10" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot10" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe10" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe10_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe10" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance9" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot9" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe9" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe9_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe9" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance8" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot8" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe8" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe8_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe8" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance7" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot7" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe7" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe7_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe7" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance6" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot6" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe6" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe6_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe6" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance5" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot5" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe5" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe5_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe5" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance4" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot4" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe4" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe4_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe4" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance3" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot3" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe3" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe3_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe3" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance2" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot2" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe2" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe2_TextChanged"></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe2" runat="server" style="display:none" Text=""></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td><asp:Label ID="LabelInstance1" runat="server"  Text="0"></asp:Label></td>
                                            <td style="text-align: right;">&nbsp; <asp:Label ID="LabelSlot1" runat="server" Text="0"></asp:Label></td>
                                            <td>&nbsp; <asp:TextBox ID="TextBoxScribe1" runat="server" AutoPostBack="true" OnTextChanged="TextBoxScribe1_TextChanged" ></asp:TextBox></td>
                                            <td><asp:Label ID="LabelScribe1" runat="server" style="display:none" Text=""></asp:Label></td>

                                        </tr>
                                        
                                       
                                    </table>
                                    <div align="center">
                                        <asp:Button ID="ButtonCheck" runat="server" Text="Check Scribes" />
                                        <cc1:AnimationExtender ID="ButtonCheck_AnimationExtender" runat="server" BehaviorID="ButtonCheck_AnimationExtender" TargetControlID="ButtonCheck">
                                        </cc1:AnimationExtender>
                                    </div>
                                    
                                </asp:Panel>
                                
                            </asp:Panel>
                         </td>
                         <td>
                            
                         </td>
                     </tr>
                 </table>               
              
            </asp:Panel>
    

        <asp:Panel ID="PanelTest" runat="server">
            
            <asp:GridView ID="GridViewLook" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical">
                <AlternatingRowStyle BackColor="#CCCCCC" />
                <FooterStyle BackColor="#CCCCCC" />
                <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#808080" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#383838" />
            </asp:GridView>
            <asp:GridView ID="GridView1" runat="server"></asp:GridView>
        </asp:Panel>
        
       </ContentTemplate>
            
    </asp:UpdatePanel>     
</asp:Content>



