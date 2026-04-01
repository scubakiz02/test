<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ReportEdit.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <script type="text/javascript">
                window.addEventListener("load", function () {
                    document.getElementById("ctl00_MasterPagePanelTop").style.display = "none"; //hide header
                    document.getElementById("ctl00_MasterPagePanelBottom").style.display = "none"; //hide footer
                    document.getElementById("ctl00_MasterPagePanel").style.minWidth = "unset"; //prevent min-width on div with id of 'ctl00_MasterPagePanel'

                    //modify styles placed on html body
                    document.body.style.background = "none";
                    document.body.style.margin = "0";
                })

                function redirect(url) {
                    window.location.href = url + this.id
                }

                function disableIframe() {
                    window.parent.iframeEnabled(false);
                }
            </script>

            <style>
                :root {
                    --UWhitespace: 0.5em;
                    --UFontSize: (calc(var(--UWhitespace) * 2))
                }
            </style>

            <asp:Panel ID="DataDisplayPanel" runat="server" Style="display: flex; flex-direction: column; gap: var(--UWhitespace);">
                <div style="display: flex; gap: var(--UWhitespace);">
                    <asp:Label ID="DbLabelFieldLabel" runat="server" />
                    <span>,</span>
                    <asp:Label ID="DataLabel" ForeColor="Blue" runat="server" />
                </div>

                <asp:Panel runat="server">
                    <asp:Label Text="Date: " ID="DbDateLabel" runat="server" />
                    <asp:TextBox ID="DbDateTextBox" runat="server" ReadOnly="True"/>
                </asp:Panel>

                <asp:Panel runat="server">
                    <asp:Label Text="Operator: " ID="DbOperatorLabel" runat="server" />
                    <asp:TextBox ID="DbOperatorTextBox" runat="server" />
                </asp:Panel>

                <asp:Panel runat="server">
                    <asp:Label Text="Value: " ID="DbValueLabel" runat="server" />
                    <asp:TextBox ID="DbValueTextBox" runat="server" />
                </asp:Panel>

            </asp:Panel>

            <asp:Button OnClick="ExitIframeButton_onClick" Text="Cancel" runat="server" />
            <asp:Button OnClick="ExitIframeButton_onClick" Text="Update" runat="server" />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
