<%@ Page Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ChangePassword.aspx.vb" Inherits="ChangePassword" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <asp:ChangePassword ID="ChangePassword1" runat="server" BackColor="#EFF3FB" BorderColor="#B5C7DE" BorderPadding="4" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" Height="208px">
        <CancelButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px"
            Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" />
        <PasswordHintStyle Font-Italic="True" ForeColor="#507CD1" />
        <ContinueButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid"
            BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" />
        <ChangePasswordButtonStyle BackColor="White" BorderColor="#507CD1" BorderStyle="Solid"
            BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" />
        <TitleTextStyle BackColor="#507CD1" Font-Bold="True" Font-Size="0.9em" ForeColor="White" />
        <ChangePasswordTemplate>
            <table border="0" cellpadding="4" cellspacing="0" style="border-collapse: collapse">
                <tr>
                    <td>
                        <table border="0" cellpadding="0" style="height: 208px">
                            <tr>
                                <td align="center" colspan="2" style="font-weight: bold; font-size: 0.9em; color: white;
                                    background-color: #507cd1">
                                    Change Your Password</td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="CurrentPasswordLabel" runat="server" AssociatedControlID="CurrentPassword">Password:</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="CurrentPassword" runat="server" Font-Size="0.8em" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server" ControlToValidate="CurrentPassword"
                                        ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="NewPasswordLabel" runat="server" AssociatedControlID="NewPassword">New Password:</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="NewPassword" runat="server" Font-Size="0.8em" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server" ControlToValidate="NewPassword"
                                        ErrorMessage="New Password is required." ToolTip="New Password is required."
                                        ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="ConfirmNewPasswordLabel" runat="server" AssociatedControlID="ConfirmNewPassword">Confirm New Password:</asp:Label></td>
                                <td>
                                    <asp:TextBox ID="ConfirmNewPassword" runat="server" Font-Size="0.8em" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server" ControlToValidate="ConfirmNewPassword"
                                        ErrorMessage="Confirm New Password is required." ToolTip="Confirm New Password is required."
                                        ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:CompareValidator ID="NewPasswordCompare" runat="server" ControlToCompare="NewPassword"
                                        ControlToValidate="ConfirmNewPassword" Display="Dynamic" ErrorMessage="The Confirm New Password must match the New Password entry."
                                        ValidationGroup="ChangePassword1"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color: red">
                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="ChangePasswordPushButton" runat="server" BackColor="White" BorderColor="#507CD1"
                                        BorderStyle="Solid" BorderWidth="1px" CommandName="ChangePassword" Font-Names="Verdana"
                                        Font-Size="0.8em" ForeColor="#284E98" Text="Change Password" ValidationGroup="ChangePassword1" />
                                </td>
                                <td>
                                    <asp:Button ID="CancelPushButton" runat="server" BackColor="White" BorderColor="#507CD1"
                                        BorderStyle="Solid" BorderWidth="1px" CausesValidation="False" CommandName="Cancel"
                                        Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98" Text="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ChangePasswordTemplate>
        <TextBoxStyle Font-Size="0.8em" />
        <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
    </asp:ChangePassword>
    <br />
</asp:Content>

