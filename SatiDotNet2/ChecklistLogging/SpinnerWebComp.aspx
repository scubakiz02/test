<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="SpinnerWebComp.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script src="Spinner.js"></script>
            <script type="text/javascript">
                function displaySpin() {
                    document.getElementById("Overlay").style.display = "flex";
                }

                function hideSpin() {
                    document.getElementById("Overlay").style.display = "none";
                }
            </script>

            <style>
                .overlay {
                    position: absolute;
                    width: 100%;
                    height: 100%;
                    background-color: black;
                    opacity: .5;
                }

                .spinner {
                    width: 50px;
                    height: 50px;
                    border: 6px solid #fff;
                    border-top: 6px solid transparent;
                    border-radius: 50%;
                    animation: spin 1s linear infinite;
                }

                @keyframes spin {
                    0% {
                        transform: rotate(0deg);
                    }

                    100% {
                        transform: rotate(360deg);
                    }
                }
            </style>

            <div style="width: 200px; height: 200px; position: relative;">
                <div id="Overlay" class="overlay" style="justify-content: center; align-items: center; display: flex; width: 100%; height: 100%; top: 0; left: 0;">
                    <div class="spinner"></div>
                </div>
            </div>

            <div style="width: 200px; height: 200px;">
                <%--<div id="Overlay" class="overlay" style="justify-content: center; align-items: center; display: flex; width: 100%; height: 100%; top: 0; left: 0;">
                    <div class="spinner"></div>
                </div>--%>
                <sati-spinner></sati-spinner>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
