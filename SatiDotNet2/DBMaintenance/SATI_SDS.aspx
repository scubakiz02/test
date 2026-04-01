<%@ Page Title="SDS Management" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="True" CodeFile="SATI_SDS.aspx.vb" Inherits="DBMaintenance_SATI_SDS" EnableSessionState="true"%>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePane" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="CreateButton" />
            <asp:PostBackTrigger ControlID="YesAdd" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="SATI_SDS_Panel" runat="server" Width="1000px">
                <table>
                    <tr>
                        <td style="height: 55px; width: 1000px;">
                            <asp:Panel ID="SDS_TitlePanel" runat="server" Width="1000px" HorizontalAlign="Center">
                                <asp:Label ID="Label1" runat="server" Text="Safety Data Sheets Information Page" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr style="padding-bottom: 15px">
                        <td style="width: 1000px">
                            <asp:Panel ID="SearchPanel" runat="server" Width="1000px" Height="90px" BackColor="LightGray">
                                <table>
                                    <tr>
                                        <td style="padding-left: 10px; width: 990px;">
                                            <asp:Panel ID="SearchInnerPanel" runat="server" Height="75px" Width="980px">
                                                <table>
                                                    <tr>
                                                        <td style="width: 720px">
                                                            <table>
                                                                <tr>
                                                                    <td style="height: 5px"></td>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="SearchLabel" runat="server" Font-Bold="True" Font-Size="Large" Text="Search SDS:" Width="135px"></asp:Label>
                                                                            <asp:TextBox ID="SearchTextBox" runat="server" AutoPostBack="true" Height="25px" placeholder="Search SDS By *, Name, Alias, Or First Letter..." Width="570px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding-left:140px; padding-top:10px">
                                                                            <asp:Label ID="NOTE0" runat="server" Text="NOTE: Type " Font-Bold="true"></asp:Label>
                                                                            <asp:Label ID="NOTE1" runat="server" Text="* (Asterisk) " Font-Bold="true" ForeColor="DarkRed"></asp:Label>
                                                                            <asp:Label ID="NOTE2" runat="server" Text="to view all SDS's" Font-Bold="true"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </tr>
                                                            </table>

                                                        </td>
                                                        <td style="width: 195px">
                                                            <asp:Panel ID="RadioButtonPanel" runat="server" Width="250px">
                                                                <table>
                                                                    <tr>
                                                                        <td style="width: 244px">&nbsp;
                                                                            <asp:Label ID="CurrentLabel" runat="server" Text="View Current to Date SDS?" Width="210px"></asp:Label>
                                                                            <asp:RadioButton ID="CurrentRadio" runat="server" AutoPostBack="true" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 244px; padding-top:15px">&nbsp;
                                                                            <asp:Label ID="RetiredLabel" runat="server" Text="View Retired to Date SDS?" Width="210px"></asp:Label>
                                                                            <asp:RadioButton ID="RetiredRadio" runat="server" AutoPostBack="true" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 1000px">
                            <asp:Panel ID="TempPanel" runat="server" Height="10px"></asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 1000px">
                            <asp:Panel ID="SDS_CreatePanel" runat="server" Width="1000px" BackColor="#507CD1" HorizontalAlign="Center" Height="150px">
                                <table border="0">
                                    <tr style="text-align: left">
                                        <td style="width: 485px; height: 35px;">&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="NameLabel" runat="server" Text="SDS Name: " Width="135px" Font-Bold="True"></asp:Label>
                                            <asp:TextBox ID="NameTextBox" runat="server" Width="303px" placeholder="SDS Full Name..."></asp:TextBox>
                                        </td>
                                        <td style="width: 250px; text-align: left; height: 35px;">
                                            <asp:Label ID="AKALabel" runat="server" Text="SDS Alias Name: " Width="121px" Font-Bold="True"></asp:Label>
                                            <asp:TextBox ID="AKATextBox" runat="server" Width="110px" placeholder=" AKA (OPTIONAL)"></asp:TextBox>
                                        </td>
                                        <td style="width: 250px; text-align: left; height: 35px;">

                                            <asp:Label ID="Optional" runat="server" Text="OPTIONAL " ForeColor="DarkRed" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="ExpDateLabel" runat="server" Text="EXP-Date:" Font-Bold="True"></asp:Label>
                                            <asp:TextBox ID="ExpDatetextBox" runat="server" Width="75px" TextMode="Date" placeholder="Optional"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <table border="0">
                                    <tr>
                                        <td style="text-align: left; width: 486px; height: 46px;">
                                            <table style="width: 469px; height: 45px;">
                                                <tr>
                                                    <td style="width: 148px">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                    <asp:Label ID="FileUpLoadLabel0" runat="server" Text="Upload " Width="52px" Font-Bold="True"></asp:Label>
                                                                    <asp:Label ID="FileUploadLabel1" runat="server" Text="PDF " Width="33px" Font-Bold="True" ForeColor="DarkRed"></asp:Label>
                                                                    <asp:Label ID="FileUploadLabel2" runat="server" Text="File: " Width="30px" Font-Bold="true"></asp:Label>
                                                                 </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                    <asp:Label ID="MaxLabel" runat="server" Text="(MAX LENGTH = 35 CHAR)" Width="121px" font-bold="true" Font-Size="XX-Small"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td>
                                                        <asp:FileUpload ID="Uploader" runat="server" autopostback="true" Width="306px" Height="25px"/>
                                                    </tb>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="width: 500px; text-align: center; height: 46px;">
                                            <asp:Label ID="ErrorMessage" runat="server" Text="------------------------------------------------------------------------------------" Width="465px" ForeColor="Black" Font-Bold="True" Style="margin-left: 0px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table border="0">
                                    <tr>
                                        <td style="width: 129px; text-align: left; height: 28px;">&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="NotesLabel" runat="server" Text="SDS Notes: " Width="86px" Font-Bold="True" Height="21px"></asp:Label>
                                        </td>
                                        <td style="width: 349px; height: 28px;">
                                            <asp:TextBox ID="NotesTextBox" runat="server" Width="301px" TextMode="MultiLine" placeholder="SDS Description (OPTIONAL)"></asp:TextBox>
                                        </td>
                                        <td style="width: 250px; height: 28px; text-align: left; align-content: normal">
                                            <%--&nbsp;--%>
                                            <asp:Button ID="CreateButton" runat="server" Text="Create" Font-Bold="True" Width="245px" Height="30px" OnClick="UploadFile" />
                                        </td>
                                        <td style="width: 250px; height: 28px; text-align: left">&nbsp;
                                            <asp:Button ID="CancelButton" runat="server" Text="Cancel" Font-Bold="True" Width="242px" OnClick="CancelUpload" Height="30px" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 1000px">
                            <asp:Panel ID="Temp1" runat="server" Height="10px"></asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 1000px">
                            <asp:Panel ID="CurrentPanel" runat="server">
                                <asp:GridView ID="SATI_SDS_Table" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="Key" DataSourceID="SqlDataSourceSATI_SDS" ForeColor="#333333" GridLines="None" Width="1000px" ShowFooter="True">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True" SortExpression="Key">
                                            <HeaderStyle Width="30px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name"></asp:BoundField>
                                        <asp:BoundField DataField="FileName" HeaderText="FileName" SortExpression="FileName" ReadOnly="True"></asp:BoundField>
                                        <asp:BoundField DataField="AKA" HeaderText="Alias " SortExpression="AKA"></asp:BoundField>
                                        <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes">
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Op" HeaderText="Op" SortExpression="Op" ReadOnly="True"></asp:BoundField>
                                        <asp:TemplateField HeaderText="ExpDate" SortExpression="ExpDate">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("ExpDate", "{0:d}") %>' Width="75px"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" TextMode="Date" Text='<%# Bind("ExpDate", "{0:d}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ExpOp" HeaderText="ExpOp" SortExpression="ExpOp" ReadOnly="True"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" Text="View" OnClick="DownloadFile"
                                                    CommandArgument='<%# Eval("FileName") %>' ForeColor="Blue"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkRetire" runat="server" Text="Retire" OnClick="RetireRow"
                                                    CommandArgument='<%# Container.DataItemIndex %>' ForeColor="Blue"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:CommandField DeleteText="Del" ShowEditButton="True">
                                            <ItemStyle ForeColor="Blue" />
                                        </asp:CommandField>
                                    </Columns>
                                    <EditRowStyle BackColor="#2461BF" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <RowStyle BackColor="#EFF3FB" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                </asp:GridView>
                                <asp:Panel ID="FooterErrorPanel" runat="server" Width="1000px" BackColor="#507CD1" Height="26" HorizontalAlign="Center">
                                    <asp:Label ID="FooterErrorMessage" runat="server" Text=""></asp:Label>
                                </asp:Panel>
                                <asp:SqlDataSource ID="SqlDataSourceSATI_SDS" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                                    DeleteCommand="DELETE FROM [T_SATI_SDS] WHERE [Key] = @Key"
                                    InsertCommand="INSERT INTO [T_SATI_SDS] ([Name], [FileName], [AKA], [Notes], [Op], [ExpDate], [ExpOp]) VALUES (@Name, @FileName, @AKA, @Notes, @Op, @ExpDate, @ExpOp)"
                                    SelectCommand="SELECT [Key], Name, FileName, AKA, Notes, Op, ExpDate, ExpOp FROM T_SATI_SDS WHERE (Name = @Name) AND (ExpDate > GETDATE() OR ExpDate IS NULL) AND (FileName IS NOT NULL) OR (ExpDate > GETDATE() OR ExpDate IS NULL) AND (AKA = @AKA) AND (FileName IS NOT NULL) OR (ExpDate > GETDATE() OR ExpDate IS NULL) AND (SUBSTRING(Name, 1, 1) = @Name) AND (FileName IS NOT NULL) OR (ExpDate > GETDATE() OR ExpDate IS NULL) AND (SUBSTRING(AKA, 1, 1) = @AKA) AND (FileName IS NOT NULL) ORDER BY Name, FileName, ExpDate"
                                    UpdateCommand="UPDATE [T_SATI_SDS] SET [Name] = @Name, [AKA] = @AKA, [Notes] = @Notes, [ExpDate] = @ExpDate WHERE [Key] = @Key">
                                    <DeleteParameters>
                                        <asp:Parameter Name="Key" Type="Int32" />
                                    </DeleteParameters>
                                    <InsertParameters>
                                        <asp:Parameter Name="Name" Type="String" />
                                        <asp:Parameter Name="FileName" Type="String" />
                                        <asp:Parameter Name="AKA" Type="String" />
                                        <asp:Parameter Name="Notes" Type="String" />
                                        <asp:Parameter Name="Op" Type="String" />
                                        <asp:Parameter DbType="Date" Name="ExpDate" />
                                        <asp:Parameter Name="ExpOp" Type="String" />
                                    </InsertParameters>
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="SearchTextBox" Name="Name" PropertyName="Text" Type="String" />
                                        <asp:ControlParameter ControlID="SearchTextBox" Name="AKA" PropertyName="Text" Type="String" />
                                    </SelectParameters>
                                    <UpdateParameters>
                                        <asp:Parameter Name="Name" Type="String" />
                                        <asp:Parameter Name="AKA" Type="String" />
                                        <asp:Parameter Name="Notes" Type="String" />
                                        <asp:Parameter DbType="Date" Name="ExpDate" />
                                        <asp:Parameter Name="Key" Type="Int32" />
                                    </UpdateParameters>
                                </asp:SqlDataSource>
                            </asp:Panel>
                            <asp:Panel ID="RetiredPanel" runat="server">
                                <asp:GridView ID="SATI_SDS_Retire" runat="server" DataSourceID="SqlDataSourceRetired" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Width="1000px" ShowFooter="True" DataKeyNames="Key">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True" SortExpression="Key"></asp:BoundField>
                                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name"></asp:BoundField>
                                        <asp:BoundField DataField="FileName" HeaderText="FileName" SortExpression="FileName"></asp:BoundField>
                                        <asp:BoundField DataField="AKA" HeaderText="Alias " SortExpression="AKA"></asp:BoundField>
                                        <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes"></asp:BoundField>
                                        <asp:BoundField DataField="Op" HeaderText="Op" SortExpression="Op"></asp:BoundField>
                                        <asp:BoundField DataField="ExpDate" HeaderText="ExpDate" SortExpression="ExpDate" DataFormatString="{0:d}"></asp:BoundField>
                                        <asp:BoundField DataField="ExpOp" HeaderText="ExpOp" SortExpression="ExpOp"></asp:BoundField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" Text="View" OnClick="DownloadFile"
                                                    CommandArgument='<%# Eval("FileName") %>' ForeColor="Blue"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkReinstate" runat="server" Text="Restore" OnClick="ReinstateFile"
                                                    CommandArgument='<%# Container.DataItemIndex %>' ForeColor="Blue"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle BackColor="#2461BF" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <RowStyle BackColor="#EFF3FB" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                </asp:GridView>
                                <asp:Panel ID="FooterRestorePanel" runat="server" Width="1000px" BackColor="#507CD1" Height="27px" HorizontalAlign="Center">
                                    <table>
                                        <tr>
                                            <td style="width:998px; align-content:center">
                                                <asp:Label ID="OptionalLabel2" runat="server" Text="OPTIONAL: " ForeColor="DarkRed" Font-Bold="true"></asp:Label>
                                                <asp:Label ID="RestoreFileLabel" runat="server" Text = "Upload FILE" Font-Bold="true"></asp:Label> &nbsp;
                                                <asp:FileUpload ID="RestoreUploaded" runat="server" Width="230px"/>
                                                &emsp;&emsp;&emsp;&nbsp;
                                                <asp:Label ID="OptionalLabel3" runat="server" Text="OPTIONAL: " ForeColor="DarkRed" Font-Bold="true"></asp:Label>
                                                <asp:Label ID="ExpDateQuestionLabel" runat="server" Text="New EXP Date" Font-Bold="True"></asp:Label>&nbsp;
                                                <asp:TextBox ID="RestoreExpDate" runat="server" Width="75px" TextMode="Date" Height="18px"></asp:TextBox> 
                                                &emsp;&emsp;&emsp;&emsp;&emsp;
                                                <asp:Button ID="YesAdd" runat="server" Text="Restore" Width="65px" Height="20px"/>&nbsp;
                                                <asp:Button ID="NoAdd" runat="server" Text="Cancel" Width="60px" Height="20px"/>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="FooterErrorPanel2" runat="server" Width="1000px" BackColor="#507CD1" Height="27" HorizontalAlign="Center">
                                    <asp:Label ID="FooterErrorMessage2" runat="server" Text=""></asp:Label>
                                </asp:Panel>
                                <asp:SqlDataSource ID="SqlDataSourceRetired" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                                    DeleteCommand="DELETE FROM [T_SATI_SDS] WHERE [Key] = @Key"
                                    InsertCommand="INSERT INTO [T_SATI_SDS] ([Name], [FileName], [AKA], [Notes], [Op], [ExpDate], [ExpOp]) VALUES (@Name, @FileName, @AKA, @Notes, @Op, @ExpDate, @ExpOp)"
                                    SelectCommand="SELECT [Key], Name, FileName, AKA, Notes, Op, ExpDate, ExpOp FROM T_SATI_SDS WHERE (Name = @Name) AND (ExpDate &lt;= GETDATE()) OR (ExpDate &lt;= GETDATE()) AND (AKA = @AKA) OR (ExpDate &lt;= GETDATE()) AND (SUBSTRING(Name, 1, 1) = @Name) OR (ExpDate &lt;= GETDATE()) AND (SUBSTRING(AKA, 1, 1) = @AKA) ORDER BY Name, FileName, ExpDate"
                                    UpdateCommand="UPDATE [T_SATI_SDS] SET [Name] = @Name, [AKA] = @AKA, [Notes] = @Notes, [ExpDate] = @ExpDate WHERE [Key] = @Key">
                                    <DeleteParameters>
                                        <asp:Parameter Name="Key" Type="Int32" />
                                    </DeleteParameters>
                                    <InsertParameters>
                                        <asp:Parameter Name="Name" Type="String" />
                                        <asp:Parameter Name="FileName" Type="String" />
                                        <asp:Parameter Name="AKA" Type="String" />
                                        <asp:Parameter Name="Notes" Type="String" />
                                        <asp:Parameter Name="Op" Type="String" />
                                        <asp:Parameter DbType="Date" Name="ExpDate" />
                                        <asp:Parameter Name="ExpOp" Type="String" />
                                    </InsertParameters>
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="SearchTextBox" Name="Name" PropertyName="Text" Type="String" />
                                        <asp:ControlParameter ControlID="SearchTextBox" Name="AKA" PropertyName="Text" Type="String" />
                                    </SelectParameters>
                                    <UpdateParameters>
                                        <asp:Parameter Name="Name" Type="String" />
                                        <asp:Parameter Name="AKA" Type="String" />
                                        <asp:Parameter Name="Notes" Type="String" />
                                        <asp:Parameter DbType="Date" Name="ExpDate" />
                                        <asp:Parameter Name="Key" Type="Int32" />
                                    </UpdateParameters>
                                </asp:SqlDataSource>
                            </asp:Panel>
                            <asp:Panel ID="BothPanel" runat="server">
                                <asp:GridView ID="SATI_SDS_Both" runat="server" DataSourceID="SqlDataSourceBoth" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Width="1000px" ShowFooter="True" DataKeyNames="Key">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" ReadOnly="True" SortExpression="Key">
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="50px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name"></asp:BoundField>
                                        <asp:BoundField DataField="FileName" HeaderText="FileName" SortExpression="FileName"></asp:BoundField>
                                        <asp:BoundField DataField="AKA" HeaderText="AKA" SortExpression="AKA"></asp:BoundField>
                                        <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes">
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Op" HeaderText="Op" SortExpression="Op"></asp:BoundField>
                                        <asp:BoundField DataField="ExpDate" HeaderText="ExpDate" SortExpression="ExpDate" DataFormatString="{0:d}">
                                        <HeaderStyle Width="75px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ExpOp" HeaderText="ExpOp" SortExpression="ExpOp" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDownload" runat="server" Text="View" OnClick="DownloadFile"
                                                    CommandArgument='<%# Eval("FileName") %>' ForeColor="Blue"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EditRowStyle BackColor="#2461BF" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <RowStyle BackColor="#EFF3FB" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSourceBoth" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                                    DeleteCommand="DELETE FROM [T_SATI_SDS] WHERE [Key] = @Key"
                                    InsertCommand="INSERT INTO [T_SATI_SDS] ([Name], [FileName], [AKA], [Notes], [Op], [ExpDate], [ExpOp]) VALUES (@Name, @FileName, @AKA, @Notes, @Op, @ExpDate, @ExpOp)"
                                    SelectCommand="SELECT [Key], Name, FileName, AKA, Notes, Op, ExpDate, ExpOp FROM T_SATI_SDS WHERE (FileName IS NOT NULL) OR (FileName IS NOT NULL) OR (FileName IS NOT NULL) OR (FileName IS NOT NULL) ORDER BY Name, FileName, ExpDate"
                                    UpdateCommand="UPDATE [T_SATI_SDS] SET [Name] = @Name, [AKA] = @AKA, [Notes] = @Notes, [ExpDate] = @ExpDate WHERE [Key] = @Key">
                                    <DeleteParameters>
                                        <asp:Parameter Name="Key" Type="Int32" />
                                    </DeleteParameters>
                                    <InsertParameters>
                                        <asp:Parameter Name="Name" Type="String" />
                                        <asp:Parameter Name="FileName" Type="String" />
                                        <asp:Parameter Name="AKA" Type="String" />
                                        <asp:Parameter Name="Notes" Type="String" />
                                        <asp:Parameter Name="Op" Type="String" />
                                        <asp:Parameter DbType="Date" Name="ExpDate" />
                                        <asp:Parameter Name="ExpOp" Type="String" />
                                    </InsertParameters>
                                    <UpdateParameters>
                                        <asp:Parameter Name="Name" Type="String" />
                                        <asp:Parameter Name="AKA" Type="String" />
                                        <asp:Parameter Name="Notes" Type="String" />
                                        <asp:Parameter DbType="Date" Name="ExpDate" />
                                        <asp:Parameter Name="Key" Type="Int32" />
                                    </UpdateParameters>
                                </asp:SqlDataSource>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 1000px">
                            <asp:Panel ID="Temp2" runat="server" Height="10px"></asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
