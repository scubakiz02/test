<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="OpenTicketStatusBoard.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:Panel ID="Panel2" runat="server"></asp:Panel>
    <asp:UpdatePanel ID="UpdatePane" runat="server">
        <ContentTemplate>
            <script src="../scripts/WebComponents/sati-full-screen.js"></script>
            <script src="../scripts/common.js"></script>
            <script type="text/javascript">
                window.addEventListener("load", async function () {
                    const Panel1 = document.getElementById("<%= Panel1.ClientID %>");
                    Panel1.innerHTML = await getBuildHtml();

                    setInterval(async function() {
                        Panel1.innerHTML = await getBuildHtml();
                    }, 60000); //60 second poll interval
                })

                async function getBuildHtml() {
                    const res = await httpGet("/api/mr-status-board-html.ashx");
                    return res.html;
                }
            </script>
            <sati-full-screen></sati-full-screen>
            <asp:Panel ID="Panel1" runat="server">
            </asp:Panel>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

