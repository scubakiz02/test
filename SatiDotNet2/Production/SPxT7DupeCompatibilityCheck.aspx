<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SPxT7DupeCompatibilityCheck.aspx.vb" Inherits="Production_SPxT7DupeCompatibilityCheck" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    &nbsp;&nbsp;<asp:UpdateProgress ID="UpdateProgress2" runat="server">
        <ProgressTemplate>
            &nbsp;<img src="../Color/Animated_LoadingBigger.gif" />Loading...
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" Width="904px">
                <table style="width: 896px">
                    <tr>
                        <td>
                            <asp:Panel ID="Panel2" runat="server" Width="125px">
                                Select SPx<br />
                                <asp:ListBox ID="ToolListBox" runat="server" AutoPostBack="True" Height="76px" OnSelectedIndexChanged="ToolListBox_SelectedIndexChanged"
                                    Width="120px">
                                    <asp:ListItem Value="SP1">SP1 (T2)</asp:ListItem>
                                    <asp:ListItem Value="SP1-3">SP1-3 (T9)</asp:ListItem>
                                    <asp:ListItem Value="SP2-S0132">SP2 (T6)</asp:ListItem>
                                    <asp:ListItem Value="SP3-2110224">SP3 (T8)</asp:ListItem>

                                </asp:ListBox></asp:Panel>
                        </td>
                        <td colspan="2">
                        <br />
                            Scan Inbound Instance
                            <asp:TextBox ID="InstanceTextBox" runat="server" OnTextChanged="IntanceTextBox_TextChanged" AutoPostBack="True"></asp:TextBox>
                            &nbsp;
                            <br /><br />
                            Wafers Marked 
                            <asp:TextBox ID="TextBox1" runat="server" Text="Pull" BackColor="Red" Width="40px"></asp:TextBox>
                            are not scribed with a proper sequence number.<br />
                            Reject wafer. Do not sent to rework..<br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Inbound Instance</td>
                        <td>
                            Station 2</td>
                        <td>
                            Station 3</td>
                    </tr>
                    <tr>
                        <td>
                            01.<asp:TextBox ID="InboundSlot1FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot1SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            02.<asp:TextBox ID="InboundSlot2FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot2SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            03.<asp:TextBox ID="InboundSlot3FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot3SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            04.<asp:TextBox ID="InboundSlot4FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot4SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            05.<asp:TextBox ID="InboundSlot5FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot5SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            06.<asp:TextBox ID="InboundSlot6FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot6SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            07.<asp:TextBox ID="InboundSlot7FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot7SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            08.<asp:TextBox ID="InboundSlot8FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot8SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            09.<asp:TextBox ID="InboundSlot9FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot9SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            10.<asp:TextBox ID="InboundSlot10FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot10SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            11.<asp:TextBox ID="InboundSlot11FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot11SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            12.<asp:TextBox ID="InboundSlot12FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot12SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            13.<asp:TextBox ID="InboundSlot13FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot13SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            14.<asp:TextBox ID="InboundSlot14FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot14SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            15.<asp:TextBox ID="InboundSlot15FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot15SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            16.<asp:TextBox ID="InboundSlot16FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot16SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            17.<asp:TextBox ID="InboundSlot17FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot17SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            18.<asp:TextBox ID="InboundSlot18FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot18SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            19.<asp:TextBox ID="InboundSlot19FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot19SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            20.<asp:TextBox ID="InboundSlot20FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot20SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            21.<asp:TextBox ID="InboundSlot21FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot21SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            22.<asp:TextBox ID="InboundSlot22FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot22SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            23.<asp:TextBox ID="InboundSlot23FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot23SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            24.<asp:TextBox ID="InboundSlot24FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot24SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            25.<asp:TextBox ID="InboundSlot25FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="InboundSlot25SeqTextBox" runat="server" Width="40px"></asp:TextBox></td>
                        <td>
                            01.<asp:TextBox ID="Station2Slot1FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot1SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            02.<asp:TextBox ID="Station2Slot2FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot2SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            03.<asp:TextBox ID="Station2Slot3FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot3SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            04.<asp:TextBox ID="Station2Slot4FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot4SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            05.<asp:TextBox ID="Station2Slot5FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot5SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            06.<asp:TextBox ID="Station2Slot6FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot6SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            07.<asp:TextBox ID="Station2Slot7FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot7SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            08.<asp:TextBox ID="Station2Slot8FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot8SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            09.<asp:TextBox ID="Station2Slot9FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot9SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            10.<asp:TextBox ID="Station2Slot10FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot10SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            11.<asp:TextBox ID="Station2Slot11FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot11SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            12.<asp:TextBox ID="Station2Slot12FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot12SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            13.<asp:TextBox ID="Station2Slot13FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot13SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            14.<asp:TextBox ID="Station2Slot14FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot14SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            15.<asp:TextBox ID="Station2Slot15FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot15SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            16.<asp:TextBox ID="Station2Slot16FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot16SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            17.<asp:TextBox ID="Station2Slot17FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot17SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            18.<asp:TextBox ID="Station2Slot18FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot18SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            19.<asp:TextBox ID="Station2Slot19FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot19SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            20.<asp:TextBox ID="Station2Slot20FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot20SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            21.<asp:TextBox ID="Station2Slot21FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot21SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            22.<asp:TextBox ID="Station2Slot22FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot22SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            23.<asp:TextBox ID="Station2Slot23FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot23SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            24.<asp:TextBox ID="Station2Slot24FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot24SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            25.<asp:TextBox ID="Station2Slot25FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station2Slot25SeqTextBox" runat="server" Width="40px"></asp:TextBox></td>
                        <td>
                            01.<asp:TextBox ID="Station3Slot1FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot1SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            02.<asp:TextBox ID="Station3Slot2FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot2SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            03.<asp:TextBox ID="Station3Slot3FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot3SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            04.<asp:TextBox ID="Station3Slot4FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot4SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            05.<asp:TextBox ID="Station3Slot5FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot5SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            06.<asp:TextBox ID="Station3Slot6FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot6SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            07.<asp:TextBox ID="Station3Slot7FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot7SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            08.<asp:TextBox ID="Station3Slot8FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot8SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            09.<asp:TextBox ID="Station3Slot9FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot9SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            10.<asp:TextBox ID="Station3Slot10FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot10SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            11.<asp:TextBox ID="Station3Slot11FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot11SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            12.<asp:TextBox ID="Station3Slot12FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot12SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            13.<asp:TextBox ID="Station3Slot13FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot13SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            14.<asp:TextBox ID="Station3Slot14FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot14SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            15.<asp:TextBox ID="Station3Slot15FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot15SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            16.<asp:TextBox ID="Station3Slot16FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot16SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            17.<asp:TextBox ID="Station3Slot17FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot17SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            18.<asp:TextBox ID="Station3Slot18FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot18SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            19.<asp:TextBox ID="Station3Slot19FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot19SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            20.<asp:TextBox ID="Station3Slot20FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot20SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            21.<asp:TextBox ID="Station3Slot21FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot21SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            22.<asp:TextBox ID="Station3Slot22FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot22SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            23.<asp:TextBox ID="Station3Slot23FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot23SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            24.<asp:TextBox ID="Station3Slot24FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot24SeqTextBox" runat="server" Width="40px"></asp:TextBox><br />
                            25.<asp:TextBox ID="Station3Slot25FullTextBox" runat="server" Width="90px"></asp:TextBox>,
                            <asp:TextBox ID="Station3Slot25SeqTextBox" runat="server" Width="40px"></asp:TextBox></td>
                    </tr>
                </table>
                <asp:TextBox ID="InfoTextBox" runat="server" Width="888px"></asp:TextBox></asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

