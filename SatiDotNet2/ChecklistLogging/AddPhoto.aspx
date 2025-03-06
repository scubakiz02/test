<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="AddPhoto.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="CreateButton" />
        </Triggers>

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
                    document.getElementById("loadingSpinner").style.display = "block";
                }
            </script>

            <style>
                :root {
                    --UWhitespace: 0.5em;
                    --UFontSize: (calc(var(--UWhitespace) * 2))
                }
            </style>

            <asp:Panel runat="server" Style="display: flex; flex-direction: column; gap: var(--UWhitespace);">
                <asp:Button OnClick="ExitIframeButton_onClick" Text="&#x58;" runat="server" Style="position: absolute; right: 0; top: 0; margin: var(--UWhitespace); font-weight: bold; background: white; border: none; font-size: calc(var(--UFontSize)* 2);" />
                <asp:Panel runat="server" ID="UploadPanel">
                    <asp:FileUpload ID="Uploader" runat="server" Height="25px" Width="306px" />
                    <asp:Button ID="CreateButton" runat="server" Font-Bold="True" OnClick="UploadFile" OnClientClick="showSpinner(); return true;" Text="Upload" />
                    <svg id="loadingSpinner" style="display: none;" width="24" height="24" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                        <style>
                            .spinner_Wezc {
                                transform-origin: center;
                                animation: spinner_Oiah .75s step-end infinite
                            }

                            @keyframes spinner_Oiah {
                                8.3% {
                                    transform: rotate(30deg)
                                }

                                16.6% {
                                    transform: rotate(60deg)
                                }

                                25% {
                                    transform: rotate(90deg)
                                }

                                33.3% {
                                    transform: rotate(120deg)
                                }

                                41.6% {
                                    transform: rotate(150deg)
                                }

                                50% {
                                    transform: rotate(180deg)
                                }

                                58.3% {
                                    transform: rotate(210deg)
                                }

                                66.6% {
                                    transform: rotate(240deg)
                                }

                                75% {
                                    transform: rotate(270deg)
                                }

                                83.3% {
                                    transform: rotate(300deg)
                                }

                                91.6% {
                                    transform: rotate(330deg)
                                }

                                100% {
                                    transform: rotate(360deg)
                                }
                            }
                        </style><g class="spinner_Wezc"><circle cx="12" cy="2.5" r="1.5" opacity=".14" /><circle cx="16.75" cy="3.77" r="1.5" opacity=".29" /><circle cx="20.23" cy="7.25" r="1.5" opacity=".43" /><circle cx="21.50" cy="12.00" r="1.5" opacity=".57" /><circle cx="20.23" cy="16.75" r="1.5" opacity=".71" /><circle cx="16.75" cy="20.23" r="1.5" opacity=".86" /><circle cx="12" cy="21.5" r="1.5" /></g>
                    </svg>
                    <%--                    <asp:Label ID="ErrorMessage" runat="server" Font-Bold="True" ForeColor="Red" Style="margin-left: 0px" Width="465px"></asp:Label>--%>
                </asp:Panel>

                <asp:Panel Visible="False" Style="display: flex; flex-direction: column; gap: var(--UWhitespace);" runat="server" ID="CancelSetPanel">
                    <div style="position: relative; width: fit-content;">
                        <asp:ImageButton ID="SnapshotImageButton" runat="server" Style="max-width: 50vw; max-height: 50vh; object-fit: contain;" />
                        <asp:Button OnClick="CancelImage_OnClick" Style="position: absolute; top: 0; right: 0; background: black; color: white; opacity: .5; border-radius: 50%;" Text="&#x58;" runat="server" />
                    </div>

                    <asp:Panel Style="display: flex; gap: var(--UWhitespace);" runat="server">
                        <asp:Label Text="Title: " Style="font-size: calc(var(--UFontSize) * 2);" runat="server" />
                        <asp:TextBox runat="server" ID="ImgNameTextBox" Style="border: 2px solid black; width: 300px;" />
                        <asp:Button OnClick="ExitIframeButton_onClick" Text="Set" runat="server" />
                    </asp:Panel>

                    <asp:Label Text="" Style="color: red;" runat="server" ID="UserErrorLabel" />
                </asp:Panel>

            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
