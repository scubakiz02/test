<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="DataPackMaker.aspx.vb" Inherits="DBMaintenance_DataPackMaker" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server">
                Make Data Packs:<br />
                <br />
                <asp:Panel ID="Panel1" runat="server">
                    <asp:RadioButton ID="RadioButtonIBM" runat="server" Text="IBM" AutoPostBack="true" GroupName="Cust" />
                    &nbsp;<asp:RadioButton ID="RadioButtonGF" runat="server" Text="GF (USA)" AutoPostBack="true" GroupName="Cust" />
                    &nbsp;<asp:RadioButton ID="RadioButtonOnSemi" runat="server" Text="On Semi" AutoPostBack="true" GroupName="Cust" />
                    &nbsp;<asp:RadioButton ID="RadioButtonWafetTech" runat="server" Text="Wafer Tech" AutoPostBack="True" GroupName="Cust" />
                    &nbsp;<asp:RadioButton ID="RadioButtonMicron" runat="server" AutoPostBack="True" GroupName="Cust" Text="Micron" />
                    &nbsp;<asp:RadioButton ID="RadioButtonSamsung" runat="server" AutoPostBack="True" GroupName="Cust" Text="Samsung" />
                    &nbsp;<asp:RadioButton ID="RadioButtonFrescale" runat="server" AutoPostBack="True" GroupName="Cust" Text="NXP/FreeScale" />
                    &nbsp;<asp:RadioButton ID="RadioButtonAvago" runat="server" AutoPostBack="True" GroupName="Cust" Text="Avago" />
                    &nbsp;<asp:RadioButton ID="RadioButtonIMEC" runat="server" AutoPostBack="True" GroupName="Cust" Text="IMEC" /><br />

                    <asp:RadioButton ID="RadioButtonIntel" runat="server" AutoPostBack="True" GroupName="Cust" Text="Intel 300mm (Test)" />
                    &nbsp;<asp:RadioButton ID="RadioButtonIntelChinaExtra" runat="server" AutoPostBack="True" GroupName="Cust" Text="Intel China Extra (Test)" />
                    &nbsp;<asp:RadioButton ID="RadioButtonMicronGrindTest" runat="server" AutoPostBack="True" GroupName="Cust" Text="Micron grind (Test)" />
                    &nbsp;<asp:RadioButton ID="RadioButtonWD" runat="server" AutoPostBack="True" GroupName="Cust" Text="WD Test" /><br />


                </asp:Panel>
                <br />
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>
                        &nbsp;<img src="../Color/Animated_LoadingBigger.gif" />Loading...
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:Panel ID="Panel2" runat="server" Visible="false">

                    <div
                        style="border-right: 2px groove; border-top: 2px groove; font-size: xx-small; z-index: 101; left: 16px; border-left: 2px groove; width: 394px; border-bottom: 2px groove; font-family: Verdana; width: 90%">
                        ID#
                        <asp:TextBox ID="TextBoxID" runat="server"></asp:TextBox><asp:Button ID="cmdUp" runat="server" Width="81px" Text="Find" Height="23px" OnClick="cmdUp_Click" />
                        &nbsp; &nbsp;<asp:Label ID="lblCurrentDir" runat="server" Font-Italic="True">Currently showing </asp:Label><br />
                        <br />
                        <table style="width: 100%">
                            <tr>
                                <td valign="top">
                                    <asp:GridView ID="gridDirList" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="gridDirList_SelectedIndexChanged"
                                        Width="418px" GridLines="None" CellPadding="0" CellSpacing="1" DataKeyNames="FullName">
                                        <HeaderStyle Font-Bold="True" BackColor="#CCFFFF" HorizontalAlign="Left"></HeaderStyle>
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <img src="../color/folder.jpg" alt="" />

                                                </ItemTemplate>
                                                <HeaderStyle Width="20px" />
                                            </asp:TemplateField>
                                            <asp:ButtonField DataTextField="Name" CommandName="Select" HeaderText="Name">
                                                <HeaderStyle Width="200px" />
                                            </asp:ButtonField>
                                            <asp:BoundField HeaderText="Size">
                                                <HeaderStyle Width="50px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="LastWriteTime" HeaderText="Last Modified" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:GridView ID="gridFileList" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="gridFileList_SelectedIndexChanged" Width="417px" GridLines="None" CellPadding="0" CellSpacing="1" DataKeyNames="FullName">
                                        <SelectedRowStyle BackColor="#C0FFFF"></SelectedRowStyle>
                                        <HeaderStyle Font-Size="1px"></HeaderStyle>
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderStyle Width="20px"></HeaderStyle>
                                                <ItemTemplate>
                                                    <img src="../color/file.jpg" alt="" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:ButtonField DataTextField="Name" CommandName="Select">
                                                <HeaderStyle Width="200px"></HeaderStyle>
                                            </asp:ButtonField>
                                            <asp:BoundField DataField="Length">
                                                <HeaderStyle Width="50px"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="LastWriteTime"></asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </td>

                                <td valign="top">
                                    <asp:FormView ID="formFileDetails" runat="server" Font-Size="XX-Small">

                                        <ItemTemplate>
                                            <b>File:
							<%#DataBinder.Eval(Container.DataItem, "FullName")%>
                                            </b>
                                            <br>
                                            Created at
						<%#DataBinder.Eval(Container.DataItem, "CreationTime")%>
                                            <br>
                                            Last updated at
						<%#DataBinder.Eval(Container.DataItem, "LastWriteTime")%>
                                            <br>
                                            Last accessed at
						<%#DataBinder.Eval(Container.DataItem, "LastAccessTime")%>
                                            <br>
                                            <i>
                                                <%#DataBinder.Eval(Container.DataItem, "Attributes")%>
                                            </i>
                                            <br>
                                            <%#DataBinder.Eval(Container.DataItem, "Length")%>
						bytes.
						<hr>
                                            <%#GetVersionInfoString(DataBinder.Eval(Container.DataItem, "FullName"))%>
                                        </ItemTemplate>
                                    </asp:FormView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <br />
                <br />
                <asp:Panel ID="Panel3" runat="server" Visible="false">
                    File To Make Data Pack From:<br />
                    <asp:TextBox ID="FileNameTextBox0" runat="server" Width="881px" BackColor="#CCFFFF"></asp:TextBox>
                    <br />
                    <asp:Label ID="LabelSeq" runat="server" Text="Seq for SQUIT 10xx"></asp:Label>
                    <asp:TextBox ID="TextBoxSeqNumber" runat="server" Width="71px">1001</asp:TextBox>
                    &nbsp;
                <asp:CheckBox ID="CheckBoxNewOrOldIBM" runat="server" Text="RFID" Checked="True" />
                    &nbsp;
                <asp:Button ID="Button7" runat="server" Text="Make Data Pack" Width="113px" />
                    <br />
                </asp:Panel>

                <br />
                <br />
                <asp:Button ID="Button_Get_ISQUIT_Package" runat="server" Text="iSQUIT Package Name" />
                &nbsp;<asp:TextBox ID="TextBoxGackageName" runat="server" Width="273px"></asp:TextBox>
                &nbsp;<asp:Label ID="Label1" runat="server" Text="ISQUIT_PUREWAFER_YYYYMMDD_HHMMSS" Font-Italic="True" Font-Size="X-Small" ForeColor="#3366FF"></asp:Label>
                <br />

            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>

