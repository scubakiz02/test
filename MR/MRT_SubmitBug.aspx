<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="true" CodeFile="MRT_SubmitBug.aspx.vb" Inherits="MR_MRT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript"> 
        function countChars(obj) {
            var maxLength = 1000;
            var strLength = obj.value.length;
            var charRemain = (maxLength - strLength);

            if (charRemain < 1) {
                document.getElementById("charNum").innerHTML = '<span style="color: red;">You have achieved the limit of ' + maxLength + '.</span>';
                if (charRemain < 0) {
                    obj.value = obj.value.substring(0, maxLength);
                }
            } else {
                document.getElementById("charNum").innerHTML = 'Characters Remaining: ' + charRemain;

                // Call the server-side method asynchronously
                PageMethods.EvaluateProblemTextBox(obj.value, function (infoLabelText) {
                    document.getElementById('<%= infoLabel.ClientID %>').innerText = infoLabelText
                    document.getElementById('<%= Button1.ClientID %>').disabled = infoLabelText === "" ? false : true; //because delegated function in code-behind cannot access asp elements
                }, function (error) {
                    console.error("Error writing to DB: " + error.get_message());
                });

            }
        }

        let lastClickTime = 0;
        const clickThreshold = 5000;

        function detectRapidClick(event) {
            let currentTime = new Date().getTime();

            if (currentTime - lastClickTime < clickThreshold) { // Prevent postback
                event.preventDefault(); 
                return false;
            }

            lastClickTime = currentTime;
            return true; //ensures postback
        }

    </script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table style="margin-bottom: 0px">
                <tr>
                    <td colspan="3" style="height: 23px;">

                        <table class="MasterPagePanelSub">
                            <tr>
                                <td>&nbsp;<asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Maintenance Request for Process Tools"></asp:Label></td>
                                <td style="text-align: right">&nbsp;<asp:Button ID="Button2" runat="server" Text="View Open Tickets" BackColor="#FFFF99" PostBackUrl="~/MR/OpenMRQuickView.aspx" /></td>
                            </tr>
                        </table>


                    </td>
                </tr>

                <tr>
                    <td>Select Department&nbsp;
                        <asp:DropDownList ID="DepartmentDropDownList" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                            DataSourceID="DepartmentSqlDataSource" DataTextField="Department" DataValueField="Department"
                            OnSelectedIndexChanged="departmentDropDownList_SelectedIndexChanged" Width="168px">
                            <asp:ListItem>Select One...</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>Select Tool&nbsp;
                        <asp:DropDownList ID="ToolDropDownList" runat="server" DataSourceID="ToolSqlDataSource"
                            DataTextField="Tool" DataValueField="Key" Width="168px" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td>Is the tool down &nbsp;
                        <asp:DropDownList ID="DropDownListTicketType" OnSelectedIndexChanged="DropDownListTicketType_OnSelectedIndexChanged" AutoPostBack="True" runat="server">
                            <asp:ListItem>Select...</asp:ListItem>
                            <asp:ListItem Value="Standard">No</asp:ListItem>
                            <asp:ListItem Value="Down">Yes</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td style="border-style: solid; border-width: thin" valign="top">
                        <table class="style1" bgcolor="#CCFFCC">
                            <tr>
                                <td colspan="2" style="width: 118px">Wafer Infomation:
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 118px">Lot Number?</td>
                                <td>
                                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 118px">Instance Number?</td>
                                <td>
                                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>
                            </tr>
                        </table>

                    </td>
                    <td colspan="2" valign="top">
                        <asp:Panel ID="PanelSGT" runat="server" BackColor="#66CCFF" Visible="False">
                            Select problem areas.<br />
                            <asp:CheckBoxList
                                ID="CheckBoxList_SGL"
                                runat="server"
                                DataSourceID="SqlDataSource_SGN"
                                DataTextField="SG_Name"
                                DataValueField="SB_Tag" RepeatLayout="Flow">
                            </asp:CheckBoxList>
                        </asp:Panel>
                    </td>
                </tr>

                <tr>
                    <td colspan="3" style="padding-top: 20px">&nbsp;Describe the Problem Below 
                        <asp:Panel ID="Panel1" runat="server" Height="17px" Style="padding-left: 4px">
                            <p id="charNum" style="color: blue">Characters Remaining: 1000 </p>
                        </asp:Panel>
                        <asp:TextBox ID="ProblemTextBox" runat="server" Height="75px" TextMode="MultiLine" Rows="3" Width="900px" onkeyup="countChars(this);"></asp:TextBox>
                    </td>
                </tr>

                <tr>
<%--                    <td colspan="3" style="height: 53px">&nbsp;<asp:Button ID="Button1" Enabled="False" runat="server" Text="Submit" /><br />--%>
                    <td colspan="3" style="height: 53px">&nbsp;<asp:Button ID="Button1" runat="server" Text="Submit" OnClientClick="return detectRapidClick(event);"/><br />
                        &nbsp;<asp:Label ID="infoLabel" runat="server" Width="640px" Style="color: red;"></asp:Label>
                    </td>
                </tr>

            </table>
            <asp:SqlDataSource ID="DepartmentSqlDataSource" runat="server"
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" SelectCommand="SELECT [Department] FROM [T_Departments] ORDER BY [Department]"></asp:SqlDataSource>

            <asp:SqlDataSource ID="ToolSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                SelectCommand="SELECT Tool, [Key] FROM dbo.T_Tools WHERE (Department = '0')"></asp:SqlDataSource>

            <asp:SqlDataSource ID="SqlDataSource_SGN" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                SelectCommand="SELECT T_Tool_SubGroup_Tag_Names.SG_Name, T_Tool_SubGroup_Tag_Names.SB_Tag FROM T_Tools INNER JOIN T_Tool_SubGroup_Tag_Names ON T_Tools.[Key] = T_Tool_SubGroup_Tag_Names.Tool_Key WHERE (T_Tools.Tool = 'CMP 1')"></asp:SqlDataSource>

            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

