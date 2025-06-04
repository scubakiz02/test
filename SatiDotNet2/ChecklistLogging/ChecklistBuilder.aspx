<%@ Page Title="" Language="VB" MaintainScrollPositionOnPostback="true" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="ChecklistBuilder.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script defer type="text/javascript">
        let PreviewPanel;
        let EditPreviewPanel;
        let PreviewPanel_iframe;
        let toSyncArr = [];
        let idToSync;
        let yPosToSync;

        window.addEventListener("load", function () {
            const openModalButtons = document.querySelectorAll('[data-modal-open]');
            const closeModalButtons = document.querySelectorAll('[data-modal-close]');
            let labelDdl = document.getElementById("<%=LabelDropDownList.ClientID%>");
            let labelDdlIdx = labelDdl.selectedIndex;
            let iframeDoc = getAspControl("PreviewPanel_iframe").contentDocument || getAspControl("PreviewPanel_iframe").contentWindow.document; //get window within iframe
            const areaCloneCreateButton = document.getElementById("area-clone-create-button");
            let inputPanel;
            let ItemsPanel;

            document.getElementById("Overlay").style.display = "none"; //hide loading wheel overlay on PreviewPanel_iframe

            for (const toSync of toSyncArr) getAspControl(toSync.idToSync).scrollTo(0, toSync.yPosToSync);

            window.iframeEnabled = iframeEnabled;

            //hightlight and programmatically scroll to input end user is focusing on within Log.aspx iframe
            iterateChildren(function () {
                const id = this.id;

                if (id && id.includes("ItemsPanel")) {
                    ItemsPanel = this;
                    return;
                }
            }, iframeDoc);
            inputPanel = ItemsPanel.querySelectorAll(".LogPanel")[labelDdlIdx]; //calling this function to account for phase/bunch titles
            hightlightCurrInput(inputPanel);
            ItemsPanel.scrollTo(0, inputPanel.offsetTop - ItemsPanel.offsetTop);

            //set event listeners relative to modal(s)
            openModalButtons.forEach(button => {
                button.addEventListener('click', e => {
                    const modal = document.querySelector(button.dataset.modalOpen);

                    if (modal == null) return;

                    e.preventDefault(); // <-- stops the form submit/postback
                    modal.classList.add('active');
                    overlay.classList.add('active');

                    document.body.style.overflow = "hidden"; //to prevent scrolling outside of modal
                })
            })

            closeModalButtons.forEach(button => {
                button.addEventListener('click', () => {
                    const modal = button.closest('.modal');

                    if (modal == null) return;
                    modal.classList.remove('active');
                    overlay.classList.remove('active');

                    document.body.style.overflow = "visible"; //to re-enable scrolling outside of modal
                })
            })

            areaCloneCreateButton.addEventListener("click", async function (e) {
                e.preventDefault(); // <-- stops the form submit/postback, so async routine is NOT interrupted by postback
                return await createClone();
            })
        })

        async function createClone() {
            const areaDdl = document.getElementById("<%=AreaDropDownList.ClientID%>");
            const areaCloneModalTextbox = document.getElementById("<%=AreaCloneNameTextBox.ClientID%>");
            const area_key_to_clone = areaDdl[areaDdl.selectedIndex].value;
            const new_area_name = areaCloneModalTextbox.value;

            try {
                const response = await fetch('ClonePM.ashx', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ areaKeyToClone: area_key_to_clone, newAreaName: new_area_name })
                });
                let result;
                let cloneResults;

                if (!response.ok) {
                    throw new Error(`Server error: ${response.status}`);
                }

                result = await response.json();
                console.log('createClone() POST result with async/await:', result);

                cloneResults = result["AreaTable"];
                if (cloneResults["Success"].toLowerCase() === "false") {
                    const errorLabel = document.getElementById("<%=AreaCloneErrorLabel.ClientID%>");
                    errorLabel.innerText = cloneResults["Message"];
                }
                else {
                    //redirect user to clone
                    PageMethods.Area_Change(cloneResults["CloneKey"], function (newUrl) {
                        window.location.href = newUrl;
                    });

                }
            } catch (err) {
                console.error('POST error:', err);
            }
        }

        function iterateChildren(callback, elem) { //traverse through all child elements and invoke callback function on them
            callback.call(elem);
            for (const child of elem.children) iterateChildren(callback, child);
        }

        function hightlightCurrInput(inputContainer) {
            //color background of entire input container to Sati blue
            //color font of entire input container to white
            iterateChildren(function () {
                this.style.backgroundColor = "#80BEFD"; //Sati blue
                this.style.color = "white";
            }, inputContainer);
        }

        function getAspControl(id) {
            return document.querySelector('[id$="' + id + '"]');
        }

        function setScrollPos() {
            let scrollTo;

            if (arguments.length > 0) scrollTo = arguments[0];
            else scrollTo = this.scrollTop;

            document.getElementById("<%=EditPreviewPanel_HiddenField.ClientID%>").value = scrollTo;
        }

        function syncScrollPos(id, yPos) {
            toSyncArr.push({ "idToSync": id, "yPosToSync": yPos });
        }

        function iframeEnabled(bit) {
            PreviewPanel_iframe = getAspControl("PreviewPanel_iframe");
            EditPreviewPanel = document.getElementById("<%=EditPreviewPanel.ClientID%>");
            document.getElementById("Overlay").style.display = "flex"; //display loading wheel over PreviewPanel_iframe

            if (!bit) {
                window.location.href = window.location.href; //redirect to current url, to prevent 'Confirm Form Resubmission' alert window
                EditPreviewPanel.classList.remove("disabled");
                PreviewPanel_iframe.style.width = "166%";
                PreviewPanel_iframe.style.height = "166%";
                PreviewPanel_iframe.style.transform = "scale(.6)";
                PreviewPanel_iframe.style.origin = "left top";
            }
            else {
                EditPreviewPanel.classList.add("disabled");
                PreviewPanel_iframe.style.width = "100%";
                PreviewPanel_iframe.style.height = "100%";
                PreviewPanel_iframe.style.transform = "unset";
                PreviewPanel_iframe.style.origin = "unset";
            }
        }

    </script>
    <style>
        :root {
            --UWhitespace: 0.5em;
            --UFontSize: (calc(var(--UWhitespace) * 2));
            --Width: 400px;
        }

        .Width {
            width: var(--Width);
        }

        .LogTextBox {
            width: calc(var(--UWhitespace) * 6);
            padding: var(--UWhitespace);
            font-size: var(--UFontSize);
            text-align: center;
        }

        .LogPanel {
            display: flex;
            justify-content: space-evenly;
            flex-direction: column;
            align-items: normal;
            font-size: var(--UFontSize);
        }

        .EverythingExceptLabel {
            display: flex;
            align-items: center;
            font-size: var(--UFontSize);
            margin-top: var(--UWhitespace);
        }

        .LogCheckBox input { /*input to hit asp CheckBox control*/
            transform: scale(2);
            margin: var(--UWhitespace);
        }

        .InputLabel {
            text-overflow: ellipsis;
            background-color: #F5F5F5;
        }

            .InputLabel:hover {
                background-color: white;
                text-overflow: unset;
            }

        .RangeOrderInterfacePanel_TextBox {
            width: 50px;
        }

        .InterfacePanel {
            border: 2px solid black;
            padding: var(--UWhitespace);
        }

        .disabled {
            opacity: 0.5;
            pointer-events: none; /* Prevent interaction with the div */
            user-select: none; /*disable text selection of an element*/
        }

        .SymmetricalGapping {
            display: flex;
            flex-direction: column;
            gap: var(--UWhitespace);
        }

        .EditPreviewPanel {
            gap: var(--UWhitespace);
            overflow-y: auto;
            height: 95%;
            overflow-x: hidden;
        }

        .RangeOrderInterfacePanel_Label {
            font-weight: bolder;
            font-family: monospace;
            font-size: var(--UFontSize);
        }

        .iframePanel {
            width: 600px;
        }

        .spinner {
            width: 50px;
            height: 50px;
            border: 6px solid #fff;
            border-top: 6px solid transparent;
            border-radius: 50%;
            animation: spin 1s linear infinite;
        }

        .overlay {
            position: absolute;
            width: 100%;
            height: 100%;
            background-color: black;
            opacity: .9;
        }

        /* ======== #area-ddl-inline-container ========= */
        #area-ddl-inline-container {
            display: flex;
            align-items: center;
            gap: var(--UWhitespace);
            width: var(--Width);
        }

        .area-ddl {
            flex: 1 1 auto; /* grow to fill remaining space */
            width: 0; /* allow shrinking below intrinsic width */
            text-overflow: ellipsis;
            text-wrap: nowrap;
        }

        /* ======= #area-clone-modal ============= */
        .modal {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%) scale(0);
            border: 1px solid black;
            border-radius: 10px;
            z-index: 10;
            background-color: white;
            font-size: calc(var(--UFontSize));
            text-wrap: nowrap;
            visibility: hidden; /* Keeps the box “out of flow” for clicks */
            opacity: 0; /* Fully transparent */
            transition: opacity 0.3s ease, /* Fade in/out over 0.3s */
            visibility 0s 0.3s; /* Delay hiding until after opacity transition */
        }

            .modal.active {
                transform: translate(-50%, -50%) scale(1);
                visibility: visible; /* Make it “there” immediately */
                opacity: 1; /* Fade to fully opaque */
                transition: opacity 0.3s ease, /* Fade in over 0.3s */
                visibility 0s 0s; /* No delay when showing—visibility becomes visible right away */
            }

        .modal-header {
            padding: var(--UWhitespace);
            display: flex;
            align-items: center;
            border-bottom: 1px solid black;
            font-weight: bold;
        }

            .modal-header .close-button {
                cursor: pointer;
                border: none;
                outline: none;
                background: none;
                font-weight: bold;
                font-size: 30px;
            }

        .modal-body {
            padding: var(--UWhitespace);
            border: none;
        }

        .modal-footer {
            padding: 0 var(--UWhitespace);
            margin-bottom: var(--UWhitespace);
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: var(--UWhitespace);
            width: 100%;
            box-sizing: border-box; /* padding is counted within 100% width */
        }

        #overlay {
            position: fixed;
            opacity: 0;
            transition: 200ms ease-in-out;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: rgba(0, 0, 0, .5);
            pointer-events: none;
        }

            #overlay.active {
                opacity: 1;
                z-index: 1;
            }

        .area-clone-modal-textbox {
            border: 2px solid black;
            width: var(--Width);
        }

        #area-clone-cancel-button {
        }

        #area-clone-create-button {
            background-color: #80BEFD;
            color: white;
        }

        .modal-footer-error-label {
            color: red;
        }

        #modal-footer-buttons {
            display: flex;
            align-items: center;
            gap: var(--UWhitespace);
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }

        @media (min-width: 1920px) {
            .EditPreviewPanel {
                display: flex;
            }

            .iframePanel {
                width: 800px;
            }
        }
    </style>

    <%--120px for header, 80.5px for footer (footer is actually 161px, so it's divided by 2 to reach desired effect)--%>
    <asp:Panel runat="server" Style="display: flex; justify-content: space-between; height: calc(100vh - (120px + 80.5px));">
        <asp:HiddenField ID="EditPreviewPanel_HiddenField" runat="server" Value="0" />
        <%--height is 95% to prevent weird overlap with footer--%>
        <asp:Panel ID="EditPreviewPanel" CssClass="EditPreviewPanel" onscroll="setScrollPos.call(this)" runat="server" Style="">
            <div>
                <asp:Panel runat="server" BackColor="#FFA07A" ID="AreaInterfacePanel" CssClass="InterfacePanel" Style="display: flex; gap: var(--UWhitespace); flex-direction: column;">

                    <div style="display: flex; flex-direction: column; gap: var(--UWhitespace);">

                        <div style="display: flex; justify-content: space-between; gap: var(--UWhitespace);">
                            <asp:Label Text="Select Checklist:" runat="server" />

                            <div>
                                <asp:Label Text="Interval:" runat="server" />
                                <asp:DropDownList ID="AreaIntervalDropDownList" AppendDataBoundItems="True" AutoPostBack="True"
                                    DataTextField="Interval" DataValueField="Key" OnSelectedIndexChanged="AreaInterval_OnSelectedIndexChanged" runat="server" DataSourceID="IntervalDropDownList_SqlDataSource">
                                    <asp:ListItem Text="All" Value="All" Selected="True" />
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div style="display: flex; align-items: center;">
                            <div style="display: flex; align-items: center; justify-content: center;">
                                <svg style="width: 15px; margin-right: var(--UWhitespace);" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1 1">
                                    <path d="m0,0h1v1H0" fill="#D3D3D3" />
                                </svg>
                                <p style="margin: 0">= disabled</p>
                            </div>
                            <div style="display: flex; align-items: center; justify-content: center;">
                                <svg style="width: 15px; margin: 0 var(--UWhitespace);" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1 1">
                                    <path d="m0,0h1v1H0" fill="#FF0000" />
                                </svg>
                                <p style="margin: 0">= missing requirements</p>
                            </div>
                        </div>

                    </div>

                    <div id="area-ddl-inline-container">
                        <asp:DropDownList ID="AreaDropDownList" CssClass="area-ddl" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                            DataSourceID="AreaDropDownList_SqlDataSource" DataTextField="Area"
                            DataValueField="Key" OnSelectedIndexChanged="AreaDropDownList_SelectedIndexChanged">
                            <asp:ListItem Selected="True">Select Checklist...</asp:ListItem>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="AreaDropDownList_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                            SelectCommand="SELECT A.Area, A.[Key] FROM [ALTS].[dbo].[T_LogArea] A LEFT JOIN [ALTS].[dbo].[T_LogAreaInterval] I ON A.IntervalKey=I.[Key] WHERE OneTimeDate IS NULL OR (OneTimeDate IS NOT NULL AND ((SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key])=0 OR (SELECT CompleteLog FROM [ALTS].[dbo].[T_LogData] WHERE AreaKey=A.[Key]) IS NULL)) ORDER BY A.Area"></asp:SqlDataSource>

                        <div id="area-clone-container">
                            <asp:Button ID="AreaCloneButton" Text="Clone" Enabled="False" data-modal-open="#area-clone-modal" runat="server" />
                            <div class="modal" id="area-clone-modal">
                                <div class="modal-header">
                                    PM/Checklist Clone Name
                                </div>
                                <div class="modal-body">
                                    <span id="area-clone-modal-name-label">Name:</span>
                                    <asp:TextBox CssClass="area-clone-modal-textbox" ID="AreaCloneNameTextBox" runat="server" />
                                </div>
                                <div class="modal-footer">
                                    <asp:Label ID="AreaCloneErrorLabel" CssClass="modal-footer-error-label" runat="server" />
                                    <div id="modal-footer-buttons">
                                        <asp:Button CssClass="area-clone-cancel-button" Text="Cancel" OnClick="CancelClone_onClick" runat="server" data-modal-close />
                                        <button id="area-clone-create-button">Create</button>
                                    </div>
                                </div>
                            </div>
                            <div id="overlay"></div>
                        </div>
                        <asp:Button ID="DeleteCloneButton" Text="Delete" OnClick="DeleteButton_onClick" Enabled="False" runat="server" />
                    </div>

                    <asp:FormView ID="AreaFormView" CssClass="Width" runat="server" DataKeyNames="Key" DataSourceID="AreaFormView_SqlDataSource" CellPadding="4" ForeColor="#333333">
                        <EmptyDataTemplate>
                            <asp:Panel runat="server" BackColor="#F7F6F3" ForeColor="#333333">
                                Checklist: No data loaded yet...
                            <br />
                                <asp:LinkButton Enabled="False" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" />
                                &nbsp;<asp:LinkButton Enabled="False" runat="server" CausesValidation="False" CommandName="Delete" Text="Disable" />
                                &nbsp;<asp:LinkButton ID="EmptyAreaNewButton" OnClick="NewButton_onClick" Enabled="True" runat="server" CausesValidation="False" CommandName="New" Text="New" />
                            </asp:Panel>
                        </EmptyDataTemplate>
                        <EditItemTemplate>
                            Checklist:
                    <asp:TextBox Style="width: 400px" ID="AreaTextBox" runat="server" Text='<%# Bind("Area") %>' />
                            <br />
                            <asp:LinkButton ID="AreaUpdateButton" OnClick="UpdateButton_onClick" runat="server" CausesValidation="True" CommandName="Update" Text="Update" />
                            &nbsp;<asp:LinkButton ID="AreaUpdateCancelButton" OnClick="UpdateCancelButton_OnClick" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                        </EditItemTemplate>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <InsertItemTemplate>
                            Checklist:
                    <asp:TextBox Style="width: 400px" ID="AreaTextBox" runat="server" Text='<%# Bind("Area") %>' />
                            <br />
                            <asp:LinkButton ID="AreaInsertButton" OnClick="InsertButton_onClick" runat="server" CausesValidation="True" CommandName="Insert" Text="Insert" />
                            &nbsp;<asp:LinkButton ID="AreaInsertCancelButton" OnClick="InsertCancelButton_onClick" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                        </InsertItemTemplate>
                        <ItemTemplate>
                            Checklist: 
                        <asp:Label ID="AreaLabel" runat="server" Text='<%# Bind("Area") %>' />
                            <br />
                            <asp:LinkButton ID="AreaEditButton" OnClick="EditButton_OnClick" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" />
                            &nbsp;<asp:LinkButton ID="AreaDisableButton" OnClick="DisableButton_onClick" runat="server" CausesValidation="False" CommandName="Delete" Text="Disable" />
                            &nbsp;<asp:LinkButton ID="AreaNewButton" OnClick="NewButton_onClick" Enabled="True" runat="server" CausesValidation="False" CommandName="New" Text="New" />
                        </ItemTemplate>
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    </asp:FormView>
                    <asp:Label ID="AreaErrorLabel" Text="" Style="color: red" runat="server" />

                    <%--InsertCommand value is a select query, because it's a workaround on the asp.net architecture to prevent empty TextBox values from creating a record in DB--%>
                    <%--DeleteCommand is placeholder so the assoociated click event can run underneath the asp.net architecture--%>
                    <asp:SqlDataSource ID="AreaFormView_SqlDataSource" runat="server" ConflictDetection="OverwriteChanges" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                        DeleteCommand="SELECT * FROM T_LogArea"
                        InsertCommand="SELECT * FROM T_LogArea"
                        SelectCommand=""
                        UpdateCommand="UPDATE [T_LogArea] SET [Area] = @Area WHERE [Key] = @original_Key AND [Area] = @original_Area">
                        <DeleteParameters>
                            <asp:Parameter Name="original_Key" Type="Int32" />
                            <asp:Parameter Name="original_Area" Type="String" />
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:Parameter Name="Area" Type="String" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="Area" Type="String" />
                        </UpdateParameters>
                    </asp:SqlDataSource>
                </asp:Panel>

                <asp:Panel runat="server" BackColor="#90EE90" ID="LabelInterfacePanel" CssClass="InterfacePanel" Style="display: flex; flex-direction: column; align-items: baseline; gap: var(--UWhitespace);">
                    <div style="display: flex; flex-direction: column;">
                        <div style="display: flex; justify-content: space-between; align-items: center;">

                            <div style="display: flex; align-items: center;">
                                <asp:Label runat="server" Text="Select Label:"></asp:Label>
                                <asp:Label runat="server" Text="*Required*" Style="color: red; font-style: italic;"></asp:Label>
                            </div>

                            <div>
                                <asp:Label Text="Field Type:" runat="server" />
                                <asp:DropDownList ID="FieldType_DropDownList" Enabled="False" runat="server" AutoPostBack="True" OnSelectedIndexChanged="FieldType_OnSelectedIndexChanged">
                                    <asp:ListItem Text="Number" Value="" Selected="True" />
                                    <asp:ListItem Text="Checkbox" Value="Checkbox" />
                                    <asp:ListItem Text="Solution Temp Comp" Value="STC" />
                                    <asp:ListItem Text="Text" Value="Text" />
                                    <asp:ListItem Text="HOA" Value="HOA" />
                                    <asp:ListItem Text="Distribution Pumps" Value="DP" />
                                    <asp:ListItem Text="Date" Value="Date" />
                                </asp:DropDownList>
                            </div>

                        </div>

                        <asp:DropDownList ID="LabelDropDownList" Enabled="False" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                            DataSourceID="LabelDropDownList_SqlDataSource" DataTextField="Label"
                            DataValueField="Key" OnSelectedIndexChanged="LabelDropDownList_SelectedIndexChanged"
                            CssClass="Width">
                            <asp:ListItem Selected="True">Select Label...</asp:ListItem>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="LabelDropDownList_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                            SelectCommand="SELECT Label, [Key] FROM T_LogLabel"></asp:SqlDataSource>

                        <div style="display: flex; align-items: center; justify-content: space-between;">
                            <asp:FormView ID="LabelFormView" Style="width: calc(var(--Width) - 100px); margin: var(--UWhitespace) 0;" runat="server" DataKeyNames="Key" DataSourceID="LabelFormView_SqlDataSource" CellPadding="4" ForeColor="#333333">
                                <EmptyDataTemplate>
                                    <asp:Panel runat="server" BackColor="#F7F6F3" ForeColor="#333333">
                                        Label: No data loaded yet...
                                    <br />
                                        <asp:LinkButton Enabled="False" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" />
                                        <%--                                        &nbsp;<asp:LinkButton Enabled="False" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" />--%>
                                        &nbsp;<asp:LinkButton ID="EmptyLabelNewButton" OnClick="NewButton_onClick" Enabled="True" runat="server" CausesValidation="False" CommandName="New" Text="New" />
                                    </asp:Panel>
                                </EmptyDataTemplate>

                                <EditItemTemplate>
                                    Label:
                    <asp:TextBox Style="width: 400px" ID="LabelTextBox" runat="server" Text='<%# Bind("Label") %>' />
                                    <br />
                                    <asp:LinkButton ID="LabelUpdateButton" OnClick="UpdateButton_onClick" runat="server" CausesValidation="True" CommandName="Update" Text="Update" />
                                    &nbsp;<asp:LinkButton ID="LabelUpdateCancelButton" OnClick="UpdateCancelButton_OnClick" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                                </EditItemTemplate>
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <InsertItemTemplate>
                                    Label:
                    <asp:TextBox Style="width: 400px" ID="LabelTextBox" runat="server" Text='<%# Bind("Label") %>' />
                                    <br />
                                    <asp:LinkButton ID="LabelInsertButton" OnClick="InsertButton_onClick" runat="server" CausesValidation="True" CommandName="Insert" Text="Insert" />
                                    &nbsp;<asp:LinkButton ID="LabelInsertCancelButton" OnClick="InsertCancelButton_onClick" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                                </InsertItemTemplate>
                                <ItemTemplate>
                                    Label:
                    <asp:Label ID="LabelLabel" runat="server" Text='<%# Bind("Label") %>' />
                                    <br />
                                    <asp:LinkButton ID="LabelEditButton" OnClick="EditButton_OnClick" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" />
                                    <%--                                    &nbsp;<asp:LinkButton ID="LabelDeleteButton" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" />--%>
                                    &nbsp;<asp:LinkButton ID="LabelNewButton" OnClick="NewButton_onClick" runat="server" CausesValidation="False" CommandName="New" Text="New" />
                                </ItemTemplate>
                                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            </asp:FormView>

                            <%--InsertCommand value is a select query, because it's a workaround on the asp.net architecture to prevent empty TextBox values from creating a record in DB--%>
                            <asp:SqlDataSource ID="LabelFormView_SqlDataSource" runat="server" ConflictDetection="OverwriteChanges" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                                DeleteCommand="DELETE FROM [T_LogLabel] WHERE [Key] = @original_Key AND [Label] = @original_Label"
                                InsertCommand="SELECT * FROM T_LogLabel"
                                SelectCommand=""
                                UpdateCommand="UPDATE [T_LogLabel] SET [Label] = @Label WHERE [Key] = @original_Key AND [Label] = @original_Label">
                                <DeleteParameters>
                                    <asp:Parameter Name="original_Key" Type="Int32" />
                                    <asp:Parameter Name="original_Label" Type="String" />
                                </DeleteParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="Label" Type="String" />
                                </InsertParameters>
                                <UpdateParameters>
                                    <asp:Parameter Name="Label" Type="String" />
                                </UpdateParameters>
                            </asp:SqlDataSource>

                            <asp:Panel ID="LabelOrderInterfacePanel" Enabled="false" runat="server" Style="display: flex; flex-direction: column; align-items: normal; gap: var(--UWhitespace);">
                                <asp:Button ID="UpInOrderLabelButton" OnClick="LabelOrderInterface_onClick" Text="up" runat="server" />
                                <asp:Button ID="DownInOrderLabelButton" OnClick="LabelOrderInterface_onClick" Text="down" runat="server" />
                            </asp:Panel>
                        </div>
                    </div>

                    <asp:CheckBox Enabled="False" Style="display: flex; flex-direction: row;" Text="Show/Hide Phases: " ID="PhaseShowHide_CheckBox" OnCheckedChanged="PhaseShowHide_OnCheckedChanged" TextAlign="Left" runat="server" AutoPostBack="true" />
                    <asp:Panel Visible="False" ID="PhaseInterfacePanel" runat="server" Style="display: flex; flex-direction: column;">
                        <div style="display: flex; gap: var(--UWhitespace);">
                            <asp:Label runat="server" Text="Select Phase:"></asp:Label>
                            <asp:Button Text="Edit" runat="server" OnClick="EditPhasesButton_OnClick" OnClientClick="iframeEnabled(true);" />
                        </div>

                        <asp:DropDownList
                            ID="PhaseDropDownList"
                            runat="server"
                            DataSourceID="PhaseDropDownList_SqlDataSource"
                            DataTextField="Phase"
                            DataValueField="Key"
                            CssClass="Width"
                            AutoPostBack="True"
                            OnSelectedIndexChanged="PhaseDropDownList_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="PhaseDropDownList_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"></asp:SqlDataSource>
                    </asp:Panel>

                    <div style="display: flex; gap: var(--UWhitespace); flex-direction: column;">
                        <asp:Panel runat="server" ID="RangeOrderInterfacePanel" Enabled="false" Style="display: flex; flex-direction: column; gap: var(--UWhitespace);">
                            <asp:Label runat="server" ID="RangeOrderLabel" Text="Range Order:"></asp:Label>
                            <asp:Panel ID="RangeOrderMenu" runat="server" Style="display: flex; gap: var(--UWhitespace);">
                                <asp:Button Text="? - ?" ID="RangePickButton" InterfacePanel="RangePanel" OnClick="RangeOrderMenu_onClick" runat="server" />
                                <asp:Button Text="+/- ?" ID="DiffButton" InterfacePanel="DiffPanel" OnClick="RangeOrderMenu_onClick" runat="server" />
                                <asp:Button Text="Less Than (<)" ID="LessThanPickButton" InterfacePanel="LessThanPanel" OnClick="RangeOrderMenu_onClick" runat="server" />
                                <asp:Button Text="Greater Than (>)" ID="GreaterThanPickButton" InterfacePanel="GreaterThanPanel" OnClick="RangeOrderMenu_onClick" runat="server" />
                            </asp:Panel>
                            <asp:Panel runat="server" ID="DynamicRangeBoxPanel" Style="position: relative; display: flex; justify-content: center; align-items: center; border: 2px solid black; padding: var(--UWhitespace); height: 100px;">
                                <asp:Panel runat="server" ID="RangePanel" Visible="False" Style="display: flex; justify-content: center; gap: var(--UWhitespace);">
                                    <asp:TextBox CssClass="RangeOrderInterfacePanel_TextBox" runat="server" ID="LowerBoundTextbox" />
                                    <asp:Label CssClass="RangeOrderInterfacePanel_Label" Text="-" runat="server" />
                                    <asp:TextBox CssClass="RangeOrderInterfacePanel_TextBox" runat="server" ID="UpperBoundTextbox" />
                                </asp:Panel>

                                <asp:Panel runat="server" ID="DiffPanel" Visible="False" Style="display: flex; justify-content: center; gap: var(--UWhitespace);">
                                    <asp:Label CssClass="RangeOrderInterfacePanel_Label" Text="+/-" runat="server" />
                                    <asp:TextBox CssClass="RangeOrderInterfacePanel_TextBox" runat="server" ID="DiffTextbox" />
                                </asp:Panel>

                                <asp:Panel runat="server" ID="DpPanel" Visible="False" Style="display: flex; justify-content: center; gap: var(--UWhitespace);">
                                    <div>
                                        <asp:Label CssClass="RangeOrderInterfacePanel_Label" Text="Pump 1:" runat="server" />
                                        <br />
                                        <asp:TextBox CssClass="RangeOrderInterfacePanel_TextBox" runat="server" ID="Pump1TextBox" />
                                    </div>

                                    <div>
                                        <asp:Label CssClass="RangeOrderInterfacePanel_Label" Text="Pump 2:" runat="server" />
                                        <br />
                                        <asp:TextBox CssClass="RangeOrderInterfacePanel_TextBox" runat="server" ID="Pump2TextBox" />
                                    </div>
                                </asp:Panel>

                                <asp:Panel runat="server" ID="LessThanPanel" Visible="False">
                                    <asp:Label CssClass="RangeOrderInterfacePanel_Label" Text="<" runat="server" />
                                    <asp:TextBox CssClass="RangeOrderInterfacePanel_TextBox" runat="server" ID="LessThanTextbox" />
                                </asp:Panel>

                                <asp:Panel runat="server" ID="GreaterThanPanel" Visible="False">
                                    <asp:Label CssClass="RangeOrderInterfacePanel_Label" Text=">" runat="server" />
                                    <asp:TextBox CssClass="RangeOrderInterfacePanel_TextBox" runat="server" ID="GreaterThanTextbox" />
                                </asp:Panel>

                                <div runat="server" style="position: absolute; bottom: var(--UWhitespace); right: var(--UWhitespace); display: flex; gap: var(--UWhitespace);">
                                    <asp:Label CssClass="RangeOrderInterfacePanel_Label" ID="InvalidInputLabel" Text="Invalid Input(s)" Visible="False" runat="server" Style="color: red;" />
                                    <asp:Button Text="Set" ID="SetRangeButton" OnClick="SetRangeButton_onClick" Enabled="False" runat="server" />
                                </div>

                                <asp:ImageButton ID="ResetRangeButton" OnClick="ResetRangeButton_onClick" Enabled="False" runat="server" Style="width: 25px; position: absolute; top: var(--UWhitespace); right: var(--UWhitespace); gap: var(--UWhitespace);" ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAjFJREFUWEft1surzVEUwPHPnTFRGBlIUfKakGQkikIGniPySCkppQhFXoWUKBIpURgg/4ABJQPkkYiBvPKYSJSJEb+ldXTucR6/c0/d3+Su2p1z9llr7+9ea++1Vp+Kpa/i/Q0BDMQDi7AWEzEqxy9cxy3cwfeyoS0LMAa7sRRjOyz+Cadz/OwEUg9wO5XnNRgtwdHihFNz/iYe40mOmJ6eYwaWp96LhDjbDqIe4Hcq1s/tK+b25/wr7MWNDqdaiUOYlHrbcLKVTTuAY9iRhhewB186ubTu/wjZ4fy9AReb2bYCiBhuSYOdCJiByGacScMI09PGRZoBXMK6VNyIOH0vEiFbkWGIcPSTZgCh8BUL8aiXndN2Mu7xN+mFF97Vr9kKoHHfeNuNr6MbtstYnRf6QCuAeIZzW6zaK8AuHMEVrGkF0M2JutWdn1nyPmZXATACP/ANo6sAmFC4/jU+YFwVAMsQKfxqXsZ/DGWLUbcxb9SPdB5pfWsRglNVeCBOH16Yg7uDDRDFKXqFj4ik1K9ED0YIXmZl3I7j7VJxr3FuZl+riA8xq5lCJw9swrm8ONGUfO6Csr4Sji8K3NuBAKzCtTR8k53R+Q4QEedoSKIChkTurzU1/5l28kAYRBo9UXRC09I6Gs/nRev1DJFaowcM985MncUYmc3perxvB1wGIOyH42Bxg+MilZHSTUxZgNqmC/IpTcnPcPcwPMgRly2+h1dKSbcApRbtRmkIoHIP/AFn7WAh9AkzDQAAAABJRU5ErkJggg==" />
                            </asp:Panel>
                        </asp:Panel>

                    </div>
                    <div>
                        <asp:Panel runat="server" Enabled="false" ID="UnitInterfacePanel">
                            <asp:Label runat="server" Text="Select Unit:"></asp:Label>&nbsp;
                        <br />
                            <asp:DropDownList ID="UnitDropDownList" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                DataSourceID="UnitDropDownList_SqlDataSource" DataTextField="Unit"
                                DataValueField="Key" OnSelectedIndexChanged="UnitDropDownList_SelectedIndexChanged"
                                CssClass="Width">
                                <%--first record in table is null, to supply users the option to reset the unit--%>
                                <asp:ListItem Value="0" Text="Nothing" Selected="True" style="color: blue;"></asp:ListItem>
                            </asp:DropDownList>

                            <%--first record in table is null, to supply users the option to reset the unit--%>
                            <asp:SqlDataSource ID="UnitDropDownList_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                                SelectCommand="SELECT Unit, [Key] FROM [ALTS].[dbo].[T_LogUnit] WHERE Unit IS NOT NULL ORDER BY Unit"></asp:SqlDataSource>
                        </asp:Panel>
                    </div>

                </asp:Panel>

            </div>

            <div>
                <asp:Panel runat="server" BackColor="#AFEEEE" ID="CommentInterfacePanel" CssClass="InterfacePanel">
                    <asp:Label runat="server" Text="Select Comment:"></asp:Label>&nbsp;
                    <br />
                    <asp:DropDownList ID="CommentDropDownList" Enabled="False" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                        DataSourceID="CommentDropDownList_SqlDataSource" DataTextField="Comment"
                        DataValueField="Key" OnSelectedIndexChanged="CommentDropDownList_SelectedIndexChanged"
                        CssClass="Width">
                        <asp:ListItem Selected="True">Select Checklist...</asp:ListItem>
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="CommentDropDownList_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                        SelectCommand="SELECT Comment, [Key] FROM T_LogCommentList"></asp:SqlDataSource>

                    <div style="display: flex; gap: var(--UWhitespace); align-items: center; justify-content: space-between;">
                        <asp:FormView ID="CommentFormView" runat="server" DataKeyNames="Key" DataSourceID="CommentFormView_SqlDataSource" CellPadding="4" ForeColor="#333333" Style="width: calc(var(--Width) - 100px); margin: var(--UWhitespace) 0;">
                            <EmptyDataTemplate>
                                <asp:Panel runat="server" BackColor="#F7F6F3" ForeColor="#333333">
                                    Comment: No data loaded yet...
                                    <br />
                                    <asp:LinkButton Enabled="False" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" />
                                    <%--                                    &nbsp;<asp:LinkButton Enabled="False" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" />--%>
                                    &nbsp;<asp:LinkButton ID="EmptyCommentNewButton" OnClick="NewButton_onClick" Enabled="True" runat="server" CausesValidation="False" CommandName="New" Text="New" />
                                </asp:Panel>
                            </EmptyDataTemplate>

                            <EditItemTemplate>
                                Comment:
                    <asp:TextBox Style="width: 400px" ID="CommentTextBox" runat="server" Text='<%# Bind("Comment") %>' />
                                <br />
                                <asp:LinkButton ID="CommentUpdateButton" OnClick="UpdateButton_onClick" runat="server" CausesValidation="True" CommandName="Update" Text="Update" />
                                &nbsp;<asp:LinkButton ID="CommentUpdateCancelButton" OnClick="UpdateCancelButton_OnClick" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />
                            </EditItemTemplate>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <InsertItemTemplate>
                                Comment:
                    <asp:TextBox Style="width: 400px" ID="CommentTextBox" runat="server" Text='<%# Bind("Comment") %>' />
                                <br />
                                <asp:LinkButton ID="CommentInsertButton" OnClick="InsertButton_onClick" runat="server" CausesValidation="True" CommandName="Insert" Text="Insert" />
                                &nbsp;<asp:LinkButton ID="CommentInsertCancelButton" OnClick="InsertCancelButton_onClick" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancel" />

                            </InsertItemTemplate>
                            <ItemTemplate>
                                Comment:
                    <asp:Label ID="CommentLabel" runat="server" Text='<%# Bind("Comment") %>' />
                                <br />
                                <asp:LinkButton ID="CommentEditButton" OnClick="EditButton_OnClick" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" />
                                <%--                                &nbsp;<asp:LinkButton ID="CommentDeleteButton" runat="server" CausesValidation="False" CommandName="Delete" Text="Delete" />--%>
                                &nbsp;<asp:LinkButton ID="CommentNewButton" OnClick="NewButton_onClick" runat="server" CausesValidation="False" CommandName="New" Text="New" />

                            </ItemTemplate>
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        </asp:FormView>

                        <%--InsertCommand value is a select query, because it's a workaround on the asp.net architecture to prevent empty TextBox values from creating a record in DB--%>
                        <asp:SqlDataSource ID="CommentFormView_SqlDataSource" runat="server" ConflictDetection="OverwriteChanges" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                            DeleteCommand="DELETE FROM [T_LogCommentList] WHERE [Key] = @original_Key AND [Comment] = @original_Comment"
                            InsertCommand="SELECT * FROM T_LogCommentList"
                            SelectCommand=""
                            UpdateCommand="UPDATE [T_LogCommentList] SET [Comment] = @Comment WHERE [Key] = @original_Key AND [Comment] = @original_Comment">
                            <DeleteParameters>
                                <asp:Parameter Name="original_Key" Type="Int32" />
                                <asp:Parameter Name="original_Comment" Type="String" />
                            </DeleteParameters>
                            <InsertParameters>
                                <asp:Parameter Name="Comment" Type="String" />
                            </InsertParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="Comment" Type="String" />
                            </UpdateParameters>
                        </asp:SqlDataSource>

                        <asp:Panel ID="CommentOrderInterface" Enabled="false" runat="server" Style="display: flex; flex-direction: column; align-items: normal; gap: var(--UWhitespace);">
                            <asp:Button ID="UpInOrderCommentButton" OnClick="CommentOrderInterface_onClick" Text="up" runat="server" />
                            <asp:Button ID="DownInOrderCommentButton" OnClick="CommentOrderInterface_onClick" Text="down" runat="server" />
                        </asp:Panel>
                    </div>

                </asp:Panel>

                <asp:Panel runat="server" BackColor="#DAB1DA" ID="StampInterfacePanel" Enabled="False" CssClass="InterfacePanel">
                    <asp:Label runat="server" Text="Stamps:"></asp:Label>
                    <asp:Button Text="Create" runat="server" Enabled="False" />
                    <asp:Button Text="Edit" runat="server" OnClick="EditStampsButton_OnClick" OnClientClick="iframeEnabled(true);" />
                </asp:Panel>

                <div class="InterfacePanel" style="display: flex; flex-direction: column; gap: var(--UWhitespace); background-color: #FFFFCC;">
                    <asp:Panel runat="server" Enabled="false" ID="DepartmentInterfacePanel">

                        <div style="display: flex; align-items: center;">
                            <asp:Label runat="server" Text="Select Department:"></asp:Label>
                            <asp:Label runat="server" Text="*Required*" Style="color: red; font-style: italic;"></asp:Label>
                        </div>

                        <asp:DropDownList Enabled="False" ID="DepartmentDropDownList" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                            DataSourceID="DepartmentDropDownList_SqlDataSource" DataTextField="Department"
                            DataValueField="Key" OnSelectedIndexChanged="DepartmentDropDownList_SelectedIndexChanged"
                            CssClass="Width">
                            <asp:ListItem Selected="True">Select Department...</asp:ListItem>
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="DepartmentDropDownList_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                            SelectCommand="SELECT Department, [Key] FROM [ALTS].[dbo].[T_LogDepartment]"></asp:SqlDataSource>

                    </asp:Panel>

                    <asp:Panel runat="server" ID="IntervalInterfacePanel" Enabled="False" Style="display: flex; flex-direction: column; gap: var(--UWhitespace);">
                        <div style="display: flex; flex-direction: column;">

                            <div style="display: flex; align-items: center;">
                                <asp:Label runat="server" Text="Select Interval:"></asp:Label>
                                <asp:Label runat="server" Text="*Required*" Style="color: red; font-style: italic;"></asp:Label>
                            </div>

                            <asp:DropDownList ID="IntervalDropDownList" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                DataSourceID="IntervalDropDownList_SqlDataSource" DataTextField="Interval"
                                DataValueField="Key" OnSelectedIndexChanged="IntervalDropDownList_SelectedIndexChanged"
                                CssClass="Width">
                                <asp:ListItem Selected="True">Select Interval...</asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="IntervalDropDownList_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                                SelectCommand="SELECT Interval, [Key] FROM [ALTS].[dbo].[T_LogAreaInterval] ORDER BY DisplayOrder"></asp:SqlDataSource>
                        </div>

                        <asp:Panel runat="server" Visible="false" ID="OneTimeDatepickPanel" CssClass="SymmetricalGapping">
                            <div style="display: flex; align-items: center;">
                                <asp:Label runat="server" Text="Date:"></asp:Label>
                                <asp:TextBox runat="server" ID="DatepickTextBox" ReadOnly="True" placeholder="*Required*" Style="font-style: italic;" />
                                <asp:Button Text="Edit" OnClick="EditDatepickButton_OnClick" runat="server" ID="EditDatepickButton" />
                            </div>
                            <asp:Calendar runat="server" Visible="False" ID="DatepickCalendar" OnDayRender="DatepickCalendar_OnDayRender" OnSelectionChanged="DatepickCalendar_OnSelectionChanged"></asp:Calendar>
                        </asp:Panel>

                        <asp:Panel runat="server" ID="AssigneeInterfacePanel" Style="display: flex; flex-direction: column;">
                            <div style="display: flex; gap: var(--UWhitespace);">
                                <asp:Label runat="server" Text="Assign To:"></asp:Label>
                                <asp:Panel ID="AssignToMenuPanel" runat="server" Style="display: flex; gap: var(--UWhitespace);">
                                    <asp:Button Text="User" ID="UserAssigneeButton" Ddl="UsersDropDownList" OnClick="AssignToMenu_onClick" runat="server" />
                                    <asp:Button Text="Shift" ID="ShiftAssigneeButton" Ddl="ShiftDropDownList" OnClick="AssignToMenu_onClick" runat="server" />
                                </asp:Panel>
                                <asp:Label runat="server" Text="*Required*" Style="color: red; font-style: italic;"></asp:Label>
                            </div>

                            <asp:Panel runat="server" ID="AssigneeDdlPanel">
                                <asp:DropDownList Enabled="False" runat="server" CssClass="Width" ID="GenericDropDownList">
                                    <asp:ListItem Selected="True" Text="" Value="NULL" />
                                </asp:DropDownList>

                                <asp:DropDownList Visible="False" ID="UsersDropDownList" runat="server" AppendDataBoundItems="True" AutoPostBack="True"
                                    DataSourceID="UsersDropDownList_SqlDataSource" DataTextField="UserName"
                                    DataValueField="UserName" OnSelectedIndexChanged="Assignee_SelectedIndexChanged"
                                    CssClass="Width">
                                    <asp:ListItem Selected="True">Assign To User...</asp:ListItem>
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="UsersDropDownList_SqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:SatiUsersConnectionString %>"
                                    SelectCommand="SELECT UserName, UserId FROM aspnet_Users WHERE IsAnonymous = 0 ORDER BY UserName"></asp:SqlDataSource>

                                <asp:DropDownList Visible="False" AutoPostBack="True" ID="ShiftDropDownList" runat="server" CssClass="Width" OnSelectedIndexChanged="Assignee_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Text="Assign To Shift..." />
                                    <asp:ListItem Text="D1" Value="D1" />
                                    <asp:ListItem Text="N1" Value="N1" />
                                    <asp:ListItem Text="D2" Value="D2" />
                                    <asp:ListItem Text="N2" Value="N2" />
                                </asp:DropDownList>
                            </asp:Panel>

                        </asp:Panel>
                    </asp:Panel>
                </div>

            </div>

        </asp:Panel>

        <asp:Panel runat="server" CssClass="iframePanel" Style="border: 2px solid black; overflow: hidden; position: relative;">
            <iframe id="PreviewPanel_iframe" runat="server" style="border: none; width: 166%; height: 166%; transform: scale(.6); transform-origin: left top; margin: 0; max-width: none;"></iframe>
            <div id="Overlay" class="overlay" style="justify-content: center; align-items: center; display: none; width: 100%; height: 100%; position: absolute; top: 0; left: 0;">
                <div class="spinner"></div>
            </div>
        </asp:Panel>
    </asp:Panel>
</asp:Content>

