<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="PageMethods.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <script type="text/javascript">
                function getAspControl(id) {
                    return document.querySelector('[id$="' + id + '"]');
                }

                function SetDBConnection(id) {
                    getAspControl(id).addEventListener("blur", function () {
                        const AspId = this.id.split("ctl00_ContentPlaceHolder1_")[1];
                        const Key = AspId.split("_")[1];

                        // Call the server-side method asynchronously
                        PageMethods.DbWrite(Key, this.value, function (success) {
                            if (success) console.log("successful write to DB");
                            else console.log("unsuccessful write to DB");
                        }, function (error) {
                            console.error("Error writing to DB: " + error.get_message());
                        });
                    });
                }
            </script>

            <asp:Panel ID="ItemsPanel" runat="server">
                <asp:TextBox ID="Label_1" runat="server"/>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
