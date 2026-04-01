<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="TestMain.aspx.vb" Inherits="TestArea_TestMain" title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script language="javascript" type="text/javascript">


function test(t){
alert(t);
}

</script>

    
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
    

        <ContentTemplate>
            &nbsp; &nbsp;<asp:Button ID="Button13" runat="server" Text="Test Maintenance Request Close Email" />
            <br /><br />
            <asp:Button ID="Button12" runat="server" Text="Test Flex for xlsX" /><br />
            <br />
            <asp:Button ID="Button11" runat="server" Text="TestEmail" />
            <br />
            <br />
            <br />
            <br />
            Test RFID Last 20 Days <br />
            RFID&nbsp;<asp:TextBox ID="TextBoxRFID" runat="server" Width="300"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;Date and Time &nbsp;
            <asp:TextBox ID="TextBoxRFIDDateTime" runat="server"></asp:TextBox>&nbsp;&nbsp;<asp:Button
                ID="Button1CheckRFID" runat="server" Text="Look" />&nbsp;&nbsp;<asp:TextBox ID="TextBoxRFIDFind" runat="server"></asp:TextBox>
            <br />
            <br />
            file look test<br />
            <asp:Button ID="Button10" runat="server" Text="Button" />
            <asp:TextBox ID="TextBox13" runat="server"></asp:TextBox>
            <br />
            <br />
            <br />
            Select intel CofA file<br />
            <asp:TextBox ID="TextBox12" runat="server"></asp:TextBox>
            <br />
            test intel data pack<br />
            <asp:Button ID="Button9" runat="server" Text="Button" />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            &nbsp;&nbsp;<br />
            Lot<asp:TextBox ID="TextBox11" runat="server"></asp:TextBox>
            ,
            <asp:Button ID="Button8" runat="server" Text="Test Metals Code" />
            <br />
            <br />
            File Name in the &quot;LabelArchive\CofA Files&quot; Dir<br />
            <asp:TextBox ID="FileNameTextBox0" runat="server" Width="320px"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="Button7" runat="server" Text="XML Test" Width="113px" />
            <br />
            <br />
            <br />
            <asp:Button ID="Button6" runat="server" Text="test geo fix" />
            <asp:TextBox ID="TextBoxcb" runat="server"></asp:TextBox>
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="Key" DataSourceID="SqlDataSource2">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" 
                        ReadOnly="True" SortExpression="Key" />
                    <asp:BoundField DataField="Department" HeaderText="Department" 
                        SortExpression="Department" />
                    <asp:BoundField DataField="RecordDate" HeaderText="RecordDate" 
                        SortExpression="RecordDate" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>" 
                SelectCommand="SELECT [Key], [Department], [RecordDate] FROM [T_Departments]">
            </asp:SqlDataSource>
            <br />
            <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" 
                DataKeyNames="Key" DataSourceID="SqlDataSource2" Height="50px" Width="125px">
                <Fields>
                    <asp:BoundField DataField="Key" HeaderText="Key" InsertVisible="False" 
                        ReadOnly="True" SortExpression="Key" />
                    <asp:BoundField DataField="Department" HeaderText="Department" 
                        SortExpression="Department" />
                    <asp:BoundField DataField="RecordDate" HeaderText="RecordDate" 
                        SortExpression="RecordDate" />
                </Fields>
            </asp:DetailsView>
            <br />
            <br />
            <br />
            
        
            
            <asp:Button ID="Button5" runat="server" Text="Button"/>
            <br />
           
        
        <br />
            <br />
            <br />
&nbsp;<asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Button" />
                <asp:Panel ID="Panel9" runat="server"  BackColor ="#FF0066" Width="143px" >
                    
                    <cc1:AnimationExtender ID = "FormChange" runat ="server" TargetControlId="Button5" >
                    <Animations>
                        <OnClick>
                            <sequence>
                              <enableAction Enabled="false" />
                              <color AnimationTarget="Panel9"
                                Duration="1"
                                StartValue="#FF0000"
                                EndValue="#666666"
                                Property="style"
                                PropertyKey="backgroundColor" />
                              <color AnimationTarget="Panel9"
                                Duration="1"
                                StartValue="#FF0000"
                                EndValue="#666666"
                                Property="style"
                                PropertyKey="backgroundColor" />
                              <enableAction Enabled="true" />
                            </sequence>
                        </OnClick>
                     </Animations>
                    </cc1:AnimationExtender>
                    
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                </asp:Panel>
           
        
            <br />
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1">
                <Columns>
                    <asp:BoundField DataField="Shift" HeaderText="Shift" SortExpression="Shift" />
                </Columns>
            </asp:GridView>
            <asp:HyperLink ID="HyperLink3" runat="server">HyperLink</asp:HyperLink><br />
            <br />
            <asp:HyperLink ID="HyperLink9" runat="server" 
                NavigateUrl="~/TestArea/Videos.aspx?Parm=Tim">Test Parm</asp:HyperLink>
            <br />
            <br />
            <br />
            <br />
    
            <asp:Panel ID="Panel6" runat="server" Height="344px" Width="616px">
                &nbsp;<br />
                <asp:Button id="btnShowPopup" runat="server" style="display:none" />
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" Width="584px">
                    <Columns>
                        <asp:TemplateField HeaderText="Shift" SortExpression="Shift">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Shift") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Shift") %>'></asp:Label>&nbsp;
            <cc1:HoverMenuExtender ID="HoverMenuExtender1" runat="server" PopupControlID="Panel7" TargetControlID="Label1" PopDelay="500" PopupPosition="Right">
            </cc1:HoverMenuExtender>
                                &nbsp; &nbsp;
                                &nbsp; &nbsp;&nbsp;
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="terst">
                            <ItemTemplate>
                                <asp:Button ID="PopUpButton" runat="server" OnClick="PopUpButton_Click" Text="Button" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
    <asp:Panel ID="Panel5" runat="server" BackColor="#FF8080" Height="32px" Width="96px">
        <asp:Button ID="OkButton" runat="server" Text="OK" />
        <asp:Button ID="CancelButton" runat="server" Text="Cancel" /></asp:Panel>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                    SelectCommand="SELECT Shift FROM dbo.Shift"></asp:SqlDataSource>
                <br />
                <br />
                <br />
                <br />
                <br />
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="modalBackground"
                DropShadow="True" PopupControlID="Panel8" TargetControlID ="btnShowPopup" >
            </cc1:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp;<asp:Button ID="Button4" runat="server" Text="Button" />
    &nbsp; &nbsp;
    <asp:Panel ID="Panel8" runat="server" BackColor="#FFFF80" Height="40px" Width="80px">
        <asp:Button ID="ok2Button" runat="server" Text="ok" Width="48px" /></asp:Panel>
    <asp:Panel ID="Panel7" runat="server" BackColor="#C0FFFF" Height="96px" Width="208px">
        hover</asp:Panel>
    <br />
    <br />
    &nbsp;<br />
    <br />
    <br />
    <br />
    <br />
    &nbsp;
    <br />
    <br />
    <br />
    <asp:Panel ID="Panel4" runat="server" BackColor="#C0FFFF" BorderColor="#C0FFFF" Width="672px">
        <br />
        <asp:TextBox ID="AddressTextBox" runat="server" Width="632px"></asp:TextBox><br />
        <asp:RadioButton ID="RadioButtonNormal" runat="server" GroupName="Type" Text="Normal" />&nbsp;<asp:RadioButton
            ID="RadioButtonKey" runat="server" Checked="True" GroupName="Type" Text="Key" /><br />
        <asp:RadioButton ID="RadioButtonShip" runat="server" Checked="True" GroupName="Address"
            Text="Ship" />&nbsp;<asp:RadioButton ID="RadioButtonBill" runat="server" GroupName="Address"
                Text="Bill" />
        ID<asp:TextBox ID="IDTextBox" runat="server" Width="96px"></asp:TextBox>&nbsp;<br />
        Key&nbsp;
        <asp:TextBox ID="KeyTextBox" runat="server" Width="32px"></asp:TextBox><br />
        <asp:Button ID="GetAddessButton" runat="server" Text="Get Address" /></asp:Panel>
    <br />
    <br />
    <br />
    <asp:Panel ID="Panel3" runat="server" Width="680px">
        Test a CB Box for Missing Data Keys<br />
        <asp:TextBox ID="CBBoxTextBox" runat="server"></asp:TextBox>
        &nbsp;<br />
        <asp:RadioButton ID="PreRadioButton" runat="server" GroupName="DataKeys" Text="PreGeo" />&nbsp;<asp:RadioButton
            ID="PostRadioButton" runat="server" GroupName="DataKeys" Text="PostGeo" />&nbsp;<asp:RadioButton
                ID="ParticalRadioButton" runat="server" GroupName="DataKeys" Text="Partical" />&nbsp;<asp:RadioButton
                    ID="AllRadioButton" runat="server" Checked="True" GroupName="DataKeys" Text="All"
                    Width="48px" /><br />
        <asp:Button ID="CB_BoxButton" runat="server" Text="Button" />&nbsp;<br />
        <br />
        Was there a Problem?
        <asp:Label ID="ProblemLabel" runat="server" Text="Result" Width="472px"></asp:Label></asp:Panel>
    <br />
    <br />
    <br />
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/TestArea/Videos.aspx">Videos</asp:HyperLink><br />
    <br />
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/PC/Kill_A_Lot.aspx">Kill A Lot Page</asp:HyperLink><br />
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <img src="../Color/Animated_LoadingBigger.gif" />Working...
        </ProgressTemplate>
    </asp:UpdateProgress>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    
    <asp:Button ID="Button1" runat="server" Text="Button" /><br />
            <br />
            <asp:Panel ID="Panel1" runat="server" BackColor="#C0C0FF" Width="360px">
                Test Micron Data Pack<br />
                <br />
                File Name in the "LabelArchive\CofA Files" Dir<br />
                <asp:TextBox ID="FileNameTextBox" runat="server" Width="320px"></asp:TextBox><br />
                <br />
                <asp:Button ID="MicronTestButton" runat="server" Text="Test" /><br />
                <br />
            </asp:Panel>
            <br />
            <br />
            <asp:Panel ID="Panel2" runat="server" BackColor="#C0FFC0" Width="512px">
                T7 Check Sum Test<br />
                <br />
                M12
                <asp:TextBox ID="M12TextBox" runat="server" Width="80px">46DQJ024SJ</asp:TextBox>
                CS
                <asp:TextBox ID="CSTextBox" runat="server" Width="32px"></asp:TextBox><br />
                Chr-32 &nbsp;&nbsp; / &nbsp;
                <br />
                1&nbsp; -
                <asp:TextBox ID="C1Neg32TextBox" runat="server" Width="24px">00</asp:TextBox>
                /
                <asp:TextBox ID="C1MODTextBox" runat="server" Width="24px">00</asp:TextBox><br />
                2&nbsp; -
                <asp:TextBox ID="C2Neg32TextBox" runat="server" Width="24px">00</asp:TextBox>
                /
                <asp:TextBox ID="C2MODTextBox" runat="server" Width="24px">00</asp:TextBox><br />
                3&nbsp; -
                <asp:TextBox ID="C3Neg32TextBox" runat="server" Width="24px">00</asp:TextBox>
                /
                <asp:TextBox ID="C3MODTextBox" runat="server" Width="24px">00</asp:TextBox><br />
                4&nbsp; -
                <asp:TextBox ID="C4Neg32TextBox" runat="server" Width="24px">00</asp:TextBox>
                /
                <asp:TextBox ID="C4MODTextBox" runat="server" Width="24px">00</asp:TextBox><br />
                5&nbsp; -
                <asp:TextBox ID="C5Neg32TextBox" runat="server" Width="24px">00</asp:TextBox>
                /
                <asp:TextBox ID="C5MODTextBox" runat="server" Width="24px">00</asp:TextBox><br />
                6&nbsp; -
                <asp:TextBox ID="C6Neg32TextBox" runat="server" Width="24px">00</asp:TextBox>
                /
                <asp:TextBox ID="C6MODTextBox" runat="server" Width="24px">00</asp:TextBox><br />
                7&nbsp; -
                <asp:TextBox ID="C7Neg32TextBox" runat="server" Width="24px">00</asp:TextBox>
                /
                <asp:TextBox ID="C7MODTextBox" runat="server" Width="24px">00</asp:TextBox><br />
                8&nbsp; -
                <asp:TextBox ID="C8Neg32TextBox" runat="server" Width="24px">00</asp:TextBox>
                /
                <asp:TextBox ID="C8MODTextBox" runat="server" Width="24px">00</asp:TextBox><br />
                9&nbsp; -
                <asp:TextBox ID="C9Neg32TextBox" runat="server" Width="24px">00</asp:TextBox>
                /
                <asp:TextBox ID="C9MODTextBox" runat="server" Width="24px">00</asp:TextBox><br />
                10-
                <asp:TextBox ID="C10Neg32TextBox" runat="server" Width="24px">00</asp:TextBox>
                /
                <asp:TextBox ID="C10MODTextBox" runat="server" Width="24px">00</asp:TextBox><br />
                Mod 33
                <asp:TextBox ID="MOD33TextBox" runat="server" Width="24px">00</asp:TextBox>
                Mod 16
                <asp:TextBox ID="MOD16TextBox" runat="server" Width="24px">00</asp:TextBox>
                Mod 59
                <asp:TextBox ID="MOD59TextBox" runat="server" Width="24px">00</asp:TextBox><br />
                Mod59 Binary
                <asp:TextBox ID="Mod59BinaryTextBox" runat="server" Width="56px"></asp:TextBox>
                Binary234
                <asp:TextBox ID="Binary234TextBox" runat="server" Width="56px"></asp:TextBox>&nbsp;
                Binary E3 &nbsp;<asp:TextBox ID="BinaryE3TextBox" runat="server" Width="56px"></asp:TextBox><br />
                CS1
                <asp:TextBox ID="CS1TextBox" runat="server" Width="24px">00</asp:TextBox>
                CS2
                <asp:TextBox ID="CS2TextBox" runat="server" Width="24px">00</asp:TextBox><br />
                <br />
                <asp:Button ID="TryButton" runat="server" Text="Try" /><br />
            </asp:Panel>
            <br />
            <br />
            <br />
            Test Random number<br />
            Lower Limit
            <asp:TextBox ID="RandomLowerTextBox" runat="server" Width="48px">1</asp:TextBox>
            as Integer<br />
            Upper Limit
            <asp:TextBox ID="RandomUpperTextBox" runat="server" Width="48px" OnTextChanged="RandomUpperTextBox_TextChanged">48</asp:TextBox>
            as Integer<br />
            Softmulti
            <asp:TextBox ID="SoftmultiTextBox" runat="server" Width="48px">3</asp:TextBox>
            as integer<br />
            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Try" /><br />
            Results as Doubles<br />
            <asp:TextBox ID="TextBox1" runat="server" Width="32px"></asp:TextBox>,<asp:TextBox
                ID="TextBox2" runat="server" Width="32px"></asp:TextBox>,<asp:TextBox ID="TextBox3"
                    runat="server" Width="32px"></asp:TextBox>,<asp:TextBox ID="TextBox4" runat="server"
                        Width="32px"></asp:TextBox>,<asp:TextBox ID="TextBox5" runat="server" Width="32px"></asp:TextBox>,<asp:TextBox
                            ID="TextBox6" runat="server" Width="32px"></asp:TextBox>,<asp:TextBox ID="TextBox7"
                                runat="server" Width="32px"></asp:TextBox>,<asp:TextBox ID="TextBox8" runat="server"
                                    Width="32px"></asp:TextBox>,<asp:TextBox ID="TextBox9" runat="server" Width="32px"></asp:TextBox>,<asp:TextBox
                                        ID="TextBox10" runat="server" Width="32px"></asp:TextBox><br />
            <br />
            <asp:TextBox ID="RandomResultsTextBox" runat="server" Width="464px"></asp:TextBox><br />
        </ContentTemplate>
    </asp:UpdatePanel><asp:UpdateProgress ID="UpdateProgress2" runat="server">
        <ProgressTemplate>
            <img src="../Color/Animated_LoadingBigger.gif" />Working...
        </ProgressTemplate>
    </asp:UpdateProgress>
    <br />
    &nbsp;&nbsp;<br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    
    <label id="lblMessage" enableviewstate="False" runat="server" />

    </label>

</asp:Content>

