<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="AddPhoto.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>


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

                    var titleTbx = document.getElementById('<%= ImgNameTextBox.ClientID %>');

                    titleTbx.addEventListener("blur", function () { //in case user click 'Go' on soft keyboard
                        debugger;
                        document.getElementById('<%= SetTitleButton.ClientID %>').click();
                    });
                    
                })

                function disableElement() {
                    this.style.opacity = .5;
                    this.style.pointerEvents = "none";
                    this.style.userSelect = "none";
                }

                function redirect(url) {
                    window.location.href = url + this.id
                }

                function disableIframe() {
                    window.parent.iframeEnabled(false);
                }

                function showSpinner() {
                    document.getElementById("fileUploadLoadingSpinner").style.display = "block";
                }

            </script>

            <style>
                :root {
                    --UWhitespace: 0.5em;
                    --UFontSize: (calc(var(--UWhitespace) * 2))
                }
            </style>

            <asp:Panel runat="server" Style="display: flex; flex-direction: column; gap: var(--UWhitespace);">
                <asp:Panel Style="display: flex; flex-direction: column; gap: var(--UWhitespace);" runat="server" ID="CancelSetPanel">
                    <div style="position: relative; width: fit-content;">
                        <asp:ImageButton ID="SnapshotImageButton" runat="server" Style="max-width: 50vw; max-height: 50vh; object-fit: contain;" />
                        <asp:Button OnClick="CancelImage_OnClick" Style="position: absolute; top: 0; right: 0; background: black; color: white; opacity: .5; border-radius: 50%; width: 30px; height: 30px;" Text="&#x58;" runat="server" />
                    </div>

                    <asp:Panel Style="display: flex; gap: var(--UWhitespace);" runat="server">
                        <asp:Label Text="Title: " Style="font-size: calc(var(--UFontSize) * 2);" runat="server" />
                        <asp:TextBox runat="server" ID="ImgNameTextBox" Style="border: 2px solid black; width: 300px;" />
                        <asp:Button ID="SetTitleButton" OnClick="ExitIframeButton_onClick" Text="Set" runat="server" />
                    </asp:Panel>

                    <asp:Label Text="" Style="color: red;" runat="server" ID="UserErrorLabel" />
                </asp:Panel>

            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
