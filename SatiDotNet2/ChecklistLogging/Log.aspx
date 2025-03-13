<%@ Page Title="" Language="VB" MaintainScrollPositionOnPostback="true" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="Log.aspx.vb" Inherits="MR_OpenTicketStatusBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%--    Select Area:&nbsp;
                <br />
    <asp:DropDownList ID="LogAreasDropDownList" runat="server" AppendDataBoundItems="True"
        AutoPostBack="True" DataSourceID="LogAreasSqlDataSource" DataTextField="Area"
        DataValueField="Key" OnSelectedIndexChanged="LogAreasDropDownList_SelectedIndexChanged"
        Width="400px">
        <asp:ListItem Selected="True">Select Area...</asp:ListItem>
    </asp:DropDownList>
    <asp:SqlDataSource ID="LogAreasSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
        SelectCommand="SELECT Area, [Key] FROM T_LogArea"></asp:SqlDataSource>

    <br />--%>

    <%--subtracted 20px from height of control below to leave room for a little bit of padding at top and bottom of webpage--%>
    <asp:UpdatePanel ID="UpdatePanel" class="SymmetricalGapping" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="CreateButton" />
        </Triggers>

        <ContentTemplate>
            <script type="text/javascript">
                let labels;
                let textbox;
                let textboxArr;
                let focusTextbox;
                let HeaderPanel;
                let FooterPanel;
                let ItemsPanel;
                let UpdatePanel;
                let toSyncArr = [];
                let idToSync;
                let yPosToSync;
                let X_onHover = 0;
                let PreviewPanel_iframe;
                let EditPreviewPanel;
                let AddNoteCounter;

                window.addEventListener("load", function () {
                    let fileUpload = document.getElementById('<%= Uploader.ClientID %>');
                    AddNoteCounter = 0;

                    window.iframeEnabled = iframeEnabled;

                    window.addEventListener("beforeunload", function () { //in case user click 'Go' on soft keyboard
                        if (document.getElementById('<%= CommentTextBox.ClientID %>').value && addNoteToLog()) {
                            document.getElementById('<%= AddCommentButton.ClientID %>').click();
                        }
                    })

                    fileUpload.addEventListener("change", function (event) {
                        var file = event.target.files[0];
                        if (file && file.type.startsWith("image/")) {
                            document.getElementById("<%=CreateButton.ClientID%>").click();
                        }
                    });
                })

                window.addEventListener("orientationchange", function () {
                    window.location.reload(); //cause a full postback
                })

                function addNoteToLog() {
                    if (document.getElementById('<%= CommentTextBox.ClientID %>').value && AddNoteCounter == 0) {
                        AddNoteCounter++;
                        return true;
                    }

                    window.location.reload(); //hinder click, to prevent note from being added to DB

                }

                function iframeEnabled(bit) {
                    EditPreviewPanel = document.getElementById("<%=ItemsPanel.ClientID%>");
                    PreviewPanel_iframe = getAspControl("PreviewPanel_iframe");

                    if (!bit) {
                        window.location.href = window.location.href; //redirect to current url, to prevent 'Confirm Form Resubmission' alert window
                    }
                    else {
                        setItemsPanel();
                        PreviewPanel_iframe.style.height = document.getElementById("<%=ItemsPanel.ClientID%>").style.maxHeight;
                        PreviewPanel_iframe.style.display = "block";
                        getAspControl("ItemsPanel").style.display = "none"; //only works if I call getAspControl function on ItemsPanel
                        disableElement.call(getAspControl("HeaderPanel"));
                        disableElement.call(getAspControl("FooterPanel"));
                    }
                }

                function disableElement() {
                    this.style.opacity = .5;
                    this.style.pointerEvents = "none";
                    this.style.userSelect = "none";
                }

                function emToPx(em, element = document.documentElement) {
                    let fontSize = parseFloat(getComputedStyle(element).fontSize);
                    return em * fontSize;
                }

                function setFooterAtBottom() {
                    setItemsPanel();
                    for (const toSync of toSyncArr) getAspControl(toSync.idToSync).scrollTo(0, toSync.yPosToSync);
                }

                function setItemsPanel() { //workaround to asp.net architecture
                    HeaderPanel = getAspControl("HeaderPanel");
                    FooterPanel = getAspControl("FooterPanel");
                    ItemsPanel = getAspControl("ItemsPanel");
                    UpdatePanel = getAspControl("UpdatePanel");
                    let UWhitespaceInPx = emToPx(parseFloat(getComputedStyle(UpdatePanel).getPropertyValue('--UWhitespace')));

                    window.scrollTo(0, 0); //to fix screen jumping bug

                    UpdatePanel = getAspControl("UpdatePanel");
                    UpdatePanel.style.height = window.innerHeight + "px";

                    document.getElementById("ctl00_MasterPagePanelTop").style.display = "none"; //hide header
                    document.getElementById("ctl00_MasterPagePanelBottom").style.display = "none"; //hide footer
                    document.getElementById("ctl00_MasterPagePanel").style.minWidth = "unset"; //prevent min-width on div with id of 'ctl00_MasterPagePanel'
                    document.getElementById("ctl00_MasterPagePanelMain").style.padding = "0 10px"; //previously padding: 10px;

                    //modify styles placed on html body
                    document.body.style.background = "none";
                    document.body.style.margin = "0";
                    document.body.style.overflow = "hidden";

                    if (ItemsPanel.style.maxHeight) ItemsPanel.style.maxHeight = "none";

                    ItemsPanel.style.maxHeight = (window.innerHeight - (UWhitespaceInPx * 2)) - (FooterPanel.offsetHeight + HeaderPanel.offsetHeight + UWhitespaceInPx) + "px";
                }

                function getAspControl(id) {
                    return document.querySelector('[id$="' + id + '"]');
                }

                //function textboxFocus(id) {
                //    textboxArr = document.querySelectorAll('input[type="text"]');
                //    for (const textbox of textboxArr) {
                //        if (textbox.id.endsWith("_" + id)) {
                //            textbox.focus();
                //            textbox.setSelectionRange(textbox.value.length, textbox.value.length);
                //        }
                //    }
                //}

                function isSTC() {
                    let element = this;

                    if (this.getAttribute("stc")) return true;

                    for (const child of element.children) {
                        if (isSTC.call(child)) return true; //traverse through element
                    }

                    return false;
                }

                function getTempTbx(typeOf) {
                    let element = this;

                    if (this.id.includes(typeOf)) return this;

                    for (const child of element.children) {
                        let res = getTempTbx.call(child, typeOf);
                        if (res) return res; //do NOT return res if it is null
                    }

                    return null;
                }

                function getInputElement() { //get parent Input element (Panel0, Panel1, etc)
                    let element = this;

                    while (!element.id.includes("Panel")) element = element.parentElement;

                    return element;
                }

                function textboxFocus(id) {
                    let currInputElement;
                    let isCurrSTC;
                    let focusInputElement;
                    let isFocusSTC;
                    let currBathTempTbx;
                    let currIrGunTempTbx;
                    let nextBathTempTbx;
                    let nextIrGunTempTbx;
                    let textboxFromArg;
                    let focusTextbox;

                    textboxArr = document.querySelectorAll('input[type="text"]');
                    for (const textbox of textboxArr) {
                        if (textbox.id.endsWith("_" + id) && textbox.style.display !== "none") {
                            textboxFromArg = textbox;
                        }
                    }

                    if (typeof textboxFromArg == "undefined") return;

                    currInputElement = getInputElement.call(textboxFromArg);
                    focusInputElement = currInputElement;

                    if (!isSTC.call(currInputElement)) focusTextbox = textboxFromArg;
                    else {
                        do { //get next STC Input element. If user happens to be on the last STC Input element, get the first one
                            let nextInputElement = focusInputElement.nextElementSibling;

                            if (!focusInputElement.nextElementSibling) { //in case user is on last STC Input element. 
                                for (const child of focusInputElement.parentElement.children) {
                                    if (isSTC.call(child)) {
                                        nextInputElement = child;
                                        break;
                                    }
                                }
                            }

                            focusInputElement = getInputElement.call(nextInputElement);
                            isFocusSTC = isSTC.call(focusInputElement);
                        }
                        while (!isFocusSTC)

                        currBathTempTbx = getTempTbx.call(currInputElement, "BathTemp");
                        currIrGunTempTbx = getTempTbx.call(currInputElement, "IrGunTemp");
                        nextBathTempTbx = getTempTbx.call(focusInputElement, "BathTemp");
                        nextIrGunTempTbx = getTempTbx.call(focusInputElement, "IrGunTemp");

                        //determine where to place cursor

                        /**
                         * if curr bath tbx has value 
                         *      if next bath tbx has no value
                         *          next bath tbx focus
                         *      else
                         *          curr ir gun tbx focus
                         * else
                         *      curr bath tbx focus
                         * 
                         * */

                        if (currBathTempTbx.value) {
                            if (!nextBathTempTbx.value && !currIrGunTempTbx.value) focusTextbox = null; //do NOT call cursor focus, b/c tech will not necessarily check temp for next solution
                            else if (!currIrGunTempTbx.value) focusTextbox = currIrGunTempTbx;
                            else {
                                while (!nextBathTempTbx.value) { //write subroutine so focusTextbox is the element of the next Input that HAS a curr bath temp
                                    focusInputElement = focusInputElement.nextElementSibling;

                                    if (!focusInputElement) return; //in case user is on last STC Input element

                                    nextBathTempTbx = getTempTbx.call(focusInputElement, "BathTemp")
                                    nextIrGunTempTbx = getTempTbx.call(focusInputElement, "IrGunTemp")
                                }

                                focusTextbox = nextIrGunTempTbx
                            }
                        }
                        else focusTextbox = currBathTempTbx;
                    }

                    if (focusTextbox && focusTextbox.style.display !== "none") {
                        focusTextbox.focus();
                        focusTextbox.setSelectionRange(textboxFromArg.value.length, textboxFromArg.value.length); //set cursor after any existing characters in textbox
                    }
                }

                function showSpinner() {
                    document.getElementById("loadingSpinner").style.visibility = "visible";
                }

                function setScrollPos() {
                    document.getElementById("<%=ItemsPanel_HiddenField.ClientID%>").value = this.scrollTop;
                }

                function syncScrollPos(id, yPos) {
                    toSyncArr.push({ "idToSync": id, "yPosToSync": yPos });
                }

                function GetDefaultX(anchorTag) {
                    let screenMidX = document.body.offsetWidth / 2;
                    if (anchorTag.getBoundingClientRect().left < screenMidX)
                        return "left"
                    else
                        return "right"

                }

                function SetHoverEffect(ID, ImageUrl) {
                    let anchorTag = getAspControl(ID);
                    let imgElem = anchorTag.previousElementSibling ? anchorTag.previousElementSibling : anchorTag.nextElementSibling; //get img element whether it's the previous or next sibling element
                    let defaultX = GetDefaultX(anchorTag);

                    anchorTag.style.padding = "1px";

                    imgElem.style.bottom = anchorTag.offsetHeight + "px";
                    imgElem.style.position = "absolute";
                    imgElem.style[defaultX] = 0;
                    imgElem.src = ImageUrl;

                    anchorTag.addEventListener("click", function (e) {
                        e.preventDefault();
                    });

                    anchorTag.addEventListener("mouseover", function (e) {
                        imgElem.style.display = "block";
                        X_onHover = e.clientX;
                    });

                    anchorTag.addEventListener("mouseout", function (e) {
                        imgElem.style.display = "none";
                        imgElem.style[defaultX] = 0; //reset

                    });

                    //anchorTag.addEventListener("mousemove", function (e) {
                    //    let currX = e.clientX - X_onHover;

                    //    if (currX > 0)
                    //        imgElem.style.left = currX + "px";
                    //    else
                    //        imgElem.style.left = 0
                    //});

                }

                function underlayTbxValue() {
                    let idSplit = this.id.split("_")
                    let underlyingTbx = getAspControl("TextBox_" + idSplit[idSplit.length - 1]);
                    let temps = underlyingTbx.value.split("/");
                    let temp1 = temps[0] && temps[0] != "undefined" ? temps[0] : "";
                    let temp2 = temps[1] && temps[1] != "undefined" ? temps[1] : "";

                    if (this.id.includes("Bath")) underlyingTbx.value = this.value + "/" + temp2;
                    else underlyingTbx.value = temp1 + "/" + this.value;

                    return underlyingTbx;
                }

                function STC_TbxOverlay(id) {
                    let elem = getAspControl(id);
                    if (!elem) return; //if elem is undefined

                    elem.addEventListener("keydown", function (e) {
                        if (event.key !== "Enter" && event.key !== "Tab") return;
                        callCodeBehindEvent.call(underlayTbxValue.call(this));
                    });
                    elem.addEventListener("blur", function (e) {
                        callCodeBehindEvent.call(underlayTbxValue.call(this));
                    });
                }

                function underlayTbxValue2() {
                    let idSplit = this.id.split("_")
                    let underlyingTbx = getAspControl("TextBox_" + idSplit[idSplit.length - 1]);
                    let DPs = underlyingTbx.value.split("/");
                    let DP1 = DPs[0] && DPs[0] != "undefined" ? DPs[0] : 0;
                    let DP2 = DPs[1] && DPs[1] != "undefined" ? DPs[1] : 0;
                    let DBvalue = this.checked ? 1 : 0;

                    if (this.id.includes("Dp1")) underlyingTbx.value = DBvalue + "/" + DP2;
                    else underlyingTbx.value = DP1 + "/" + DBvalue;

                    return underlyingTbx;
                }

                function DP_TbxOverlay(id) {
                    let elem = getAspControl(id);
                    if (!elem) return; //if elem is undefined

                    elem.addEventListener("change", function (e) {
                        callCodeBehindEvent.call(underlayTbxValue2.call(this));
                    });
                }

                function SetDBConnection(id) {
                    let elem = getAspControl(id);
                    let InputElement = getInputElement.call(elem);
                    if (!elem) return; //if elem is undefined

                    //checkbox & stc fieldtype controls do NOT have a colored background. Change this
                    if (isSTC.call(InputElement)) {
                        getTempTbx.call(InputElement, "BathTemp").style.backgroundColor = InputElement.style.backgroundColor;
                        getTempTbx.call(InputElement, "IrGunTemp").style.backgroundColor = InputElement.style.backgroundColor;
                    }

                    if (elem.id.includes("TextBox")) {
                        elem.addEventListener("keydown", function (e) {
                            if (event.key !== "Enter" && event.key !== "Tab") return;
                            callCodeBehindEvent.call(this);
                        });
                        elem.addEventListener("blur", function (e) {
                            if (this.value === "") return;
                            callCodeBehindEvent.call(this);
                        });
                    }
                    else elem.addEventListener("change", callCodeBehindEvent.bind(elem));
                }

                function callCodeBehindEvent() {
                    let value;
                    let url = new URL(window.location.href);
                    let id = this.id.split("ctl00_ContentPlaceHolder1_")[1];
                    const self = this;

                    switch (this.type) {
                        case "checkbox":
                            if (this.checked) value = 1;
                            else value = "";
                            break;
                        default: //textbox or select (ddl)
                            value = this.value;
                    }

                    PageMethods.DbWrite(id, value, function (ChangeInValue) {
                        if (ChangeInValue.toLowerCase() === "true") {
                            let IP_ScrollPos = getAspControl("ItemsPanel").scrollTop;
                            if (IP_ScrollPos == 0) IP_ScrollPos = url.searchParams.get("IP_ScrollPos"); //safeguarding in case ItemsPanel.scrollTop is 0 (happens with 'DP' fieldtype in certain cases)
                            url.searchParams.set("IP_ScrollPos", IP_ScrollPos);
                            window.location.href = url.toString();
                        }
                    }, function (error) {
                        console.error("Error writing to DB: " + error.get_message());
                    });
                }

                function OpenFileUpload() {
                    let FileUploadControl = document.getElementById('<%=Uploader.ClientID%>');
                    FileUploadControl.click();
                }

                function showSpinner() {
                    document.getElementById("loadingSpinner").style.display = "block";
                }
            </script>
            <style>
                :root {
                    --UWhitespace: 0.5em;
                    --UFontSize: (calc(var(--UWhitespace) * 3.25));
                    --AddButtonWidth: 50px;
                }

                .LogTextBox {
                    width: 100%; /*textbox control takes as much space as it can WITHOUT causing weird css behavior*/
                    padding: calc(var(--UWhitespace) / 2);
                    font-size: var(--UFontSize);
                    text-align: center;
                }

                .LogPanel {
                    display: flex;
                    justify-content: space-evenly;
                    flex-direction: column;
                    align-items: normal;
                    font-size: calc(var(--UFontSize)* .6);
                }

                .EverythingExceptTitle {
                    display: flex;
                    align-items: center;
                    font-size: calc(var(--UFontSize));
                    margin: var(--UWhitespace);
                }

                .LogCheckBox {
                    display: flex;
                    align-items: center;
                }

                    .LogCheckBox input { /*input to hit asp CheckBox control*/
                        transform: scale(1.5);
                        margin: var(--UWhitespace);
                    }

                .SymmetricalGapping {
                    display: flex;
                    flex-direction: column;
                    gap: var(--UWhitespace);
                }

                /*this css fixes bug causing height > screen height*/
                .MasterPageContentPanel { /*override padding property set in masterpage*/
                    padding-top: 0;
                    padding-bottom: 0;
                }
                /*this css fixes bug causing height > screen height*/

                .disabled {
                    opacity: 0.5;
                    pointer-events: none; /* Prevent interaction with the div */
                    user-select: none; /*disable text selection of an element*/
                }

                .HeaderPanelButtons {
                    padding: var(--UWhitespace);
                    font-size: var(--UFontSize);
                }
            </style>
            <div style="display: flex; flex-direction: column;">
                <asp:Panel ID="HeaderPanel" Style="display: flex; flex-direction: column; gap: var(--UWhitespace);" runat="server">
                    <div style="display: flex; justify-content: space-between; flex-direction: row-reverse;">
                        <asp:Label ID="DateLabel" runat="server" Style="text-wrap: nowrap; font-style: italic;"></asp:Label>
                        <asp:Panel ID="StampPanel" runat="server" Style="display: flex; gap: var(--UWhitespace);"></asp:Panel>
                    </div>

                    <asp:Panel runat="server" Style="display: flex; align-items: flex-start; justify-content: space-between; flex-direction: row-reverse; width: 100%;">
                        <asp:Button ID="WrongFormButton" Text="Wrong Form" CssClass="HeaderPanelButtons" BackColor="Red" runat="server" OnClick="ResetLog_OnClick" />

                        <asp:Panel ID="CommentPanel" CssClass="SymmetricalGapping" Style="display: flex; flex-direction: column; gap: var(--UWhitespace);" runat="server">
                            <%--<asp:Label ID="CommentPanelLabel" runat="server" Font-Size="X-Large" Font-Bold="true"></asp:Label>--%>
                        </asp:Panel>
                    </asp:Panel>

                    <div class="SymmetricalGapping" style="display: flex; align-items: baseline; flex-direction: column;">
                        <div style="display: flex; justify-content: space-between; align-items: center; width: 100%;">
                            <asp:Label ID="TitleLabel" runat="server" Style="font-weight: bolder;"></asp:Label>
                            <asp:Label ID="MessageUserLabel" Text="" runat="server" Style="font-weight: bolder; color: red;"></asp:Label>
                        </div>
                        <asp:Panel ID="ErrorMessagePanel" runat="server" Style="display: flex; align-items: center; width: 100%; justify-content: space-between; font-size: var(--UFontSize)">
                            <div style="display: flex; align-items: center; justify-content: center;">
                                <div style="display: flex; align-items: center; justify-content: center;">
                                    <svg style="width: 15px; margin-right: var(--UWhitespace);" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1 1">
                                        <path d="m0,0h1v1H0" fill="#F00" />
                                    </svg>
                                    <p style="margin: 0">= invalid value</p>
                                </div>
                                <div style="display: flex; align-items: center; justify-content: center;">
                                    <svg style="width: 15px; margin: 0 10px;" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1 1">
                                        <path d="m0,0h1v1H0" fill="#E6E600" />
                                    </svg>
                                    <p style="margin: 0">= out of range value</p>
                                </div>
                            </div>
                            <div style="display: flex; align-items: center; gap: var(--UWhitespace);" >
                                <asp:CheckBox Visible="False" AutoPostBack="True" ID="MarkAsDoneCheckBox" OnCheckedChanged="MarkAsDoneCheckBox_OnCheckedChanged" runat="server" Text="← Some logs are invalid. Mark as done." CssClass="LogCheckBox" Style="color: red; margin: 0 10px"></asp:CheckBox>
                                <asp:Button ID="UndoDoneButton" CssClass="HeaderPanelButtons" OnClick="DoneButton_Click" Text="Undo Done" Enabled="False" runat="server"></asp:Button>
                                <asp:Button ID="DoneButton" CssClass="HeaderPanelButtons" OnClick="DoneButton_Click" Text="Done" runat="server"></asp:Button>
                            </div>
                        </asp:Panel>
                    </div>

                </asp:Panel>

                <asp:HiddenField ID="ItemsPanel_HiddenField" runat="server" Value="0" />

                <iframe id="PreviewPanel_iframe" runat="server" style="display: none; border: 2px solid black; background: white;"></iframe>

                <asp:Panel runat="server" ID="ItemsPanel" onscroll="setScrollPos.call(this)" Style="display: grid; grid-template-columns: 49% 49%; justify-content: space-between; gap: var(--UWhitespace); overflow: auto;">

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel0" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel1" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel2" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel3" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel4" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel5" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel6" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel7" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel8" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel9" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel10" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel11" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel12" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel13" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel14" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel15" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel16" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel17" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel18" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel19" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel20" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel21" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel22" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel23" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel24" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel25" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel26" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel27" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel28" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel29" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel30" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel31" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel32" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel33" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel34" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel35" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel36" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel37" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel38" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel39" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel40" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel41" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel42" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel43" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel44" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel45" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel46" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel47" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel48" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel49" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel50" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel51" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel BackColor="#F5F5F5" CssClass="LogPanel" ID="Panel52" Visible="False" runat="server" Style="padding: var(--UWhitespace) 0; border: 1px solid black;">
                        <asp:Button BackColor="#F5F5F5" runat="server" Style="margin: 0 var(--UWhitespace); padding: var(--UWhitespace); font-size: var(--UFontSize); text-align: left; pointer-events: none; text-wrap: auto;"></asp:Button>
                        <div class="EverythingExceptTitle" style="display: flex; font-size: calc(var(--UFontSize)); gap: var(--UWhitespace);">

                            <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server"></asp:TextBox>

                            <asp:Panel Visible="False" Checkbox="False" runat="server">
                                <asp:CheckBox CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" Text="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace); width: 100%;">
                                <asp:TextBox class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="width: 100%; text-align: left;"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel Visible="False" STC="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="Bath Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Text="IR Gun Temp" Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:TextBox AutoPostBack="True" class="LogTextBox" BackColor="#F5F5F5" runat="server" Style="w"></asp:TextBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" DP="False" runat="server" Style="display: flex; align-items: center; gap: var(--UWhitespace)">
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                                <div style="display: flex; flex-direction: column">
                                    <asp:Label Style="font-size: calc(var(--UFontSize) / 2);" runat="server" />
                                    <asp:CheckBox Style="display: flex; flex-direction: column-reverse;" AutoPostBack="True" CssClass="LogCheckBox" runat="server"></asp:CheckBox>
                                </div>
                            </asp:Panel>

                            <asp:Panel Visible="False" HOA="False" runat="server">
                                <asp:DropDownList runat="server">
                                    <asp:ListItem Selected="True" Text="Switch Select..." />
                                    <asp:ListItem Text="Hand" Value="Hand" />
                                    <asp:ListItem Text="Off" Value="Off" />
                                    <asp:ListItem Text="Auto" Value="Auto" />
                                </asp:DropDownList>
                            </asp:Panel>

                            <div style="display: flex; flex-direction: column; font-size: 40%;">
                                <asp:Label runat="server" ColorBlindMessage="True" Style="text-wrap: nowrap; margin: 0 var(--UWhitespace);"></asp:Label>
                                <asp:CheckBox Visible="False" OnCheckedChanged="VerifyValue_Check" CssClass="LogCheckBox" Text=" ← Check if correct" runat="server" AutoPostBack="True" />
                            </div>
                        </div>
                    </asp:Panel>

                </asp:Panel>

                <%--<asp:Timer ID="DbUploadTimer" OnTick="DbUploadTimer_Tick" Interval="15000" runat="server"></asp:Timer>--%>

                <asp:Panel runat="server" ID="FooterPanel" Style="position: fixed; bottom: 0; width: calc(100% - 20px); display: flex; flex-direction: column;">

                    <%-- max-width of 100vw b/c setFooterAtBottom is called AFTER SetHoverEffect function--%>
                    <asp:Panel ID="ImageHoverLinkPanel" Style="display: flex; flex-wrap: wrap; align-items: center; gap: var(--UWhitespace); max-width: 100vw;" runat="server">
                        <%--<asp:ImageButton OnClick="AddPhotoButton_OnClick" Style="border: 2px solid black; border-radius: 50%; padding: var(--UWhitespace); width: 18px;" ImageUrl="data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIzMiIgaGVpZ2h0PSIzMiIgZmlsbD0iIzAwMDAwMCIgdmlld0JveD0iMCAwIDI1NiAyNTYiPjxwYXRoIGQ9Ik0xNjgsMTM2YTgsOCwwLDAsMS04LDhIMTM2djI0YTgsOCwwLDAsMS0xNiwwVjE0NEg5NmE4LDgsMCwwLDEsMC0xNmgyNFYxMDRhOCw4LDAsMCwxLDE2LDB2MjRoMjRBOCw4LDAsMCwxLDE2OCwxMzZabTY0LTU2VjE5MmEyNCwyNCwwLDAsMS0yNCwyNEg0OGEyNCwyNCwwLDAsMS0yNC0yNFY4MEEyNCwyNCwwLDAsMSw0OCw1Nkg3NS43Mkw4NywzOS4xMkExNiwxNiwwLDAsMSwxMDAuMjgsMzJoNTUuNDRBMTYsMTYsMCwwLDEsMTY5LDM5LjEyTDE4MC4yOCw1NkgyMDhBMjQsMjQsMCwwLDEsMjMyLDgwWm0tMTYsMGE4LDgsMCwwLDAtOC04SDE3NmE4LDgsMCwwLDEtNi42Ni0zLjU2TDE1NS43Miw0OEgxMDAuMjhMODYuNjYsNjguNDRBOCw4LDAsMCwxLDgwLDcySDQ4YTgsOCwwLDAsMC04LDhWMTkyYTgsOCwwLDAsMCw4LDhIMjA4YTgsOCwwLDAsMCw4LThaIj48L3BhdGg+PC9zdmc+" runat="server" />--%>

                        <asp:Panel runat="server" ID="UploadPanel" Style="display: flex;">
                            <asp:ImageButton OnClientClick="OpenFileUpload(); return false;" Style="border: 2px solid black; border-radius: 50%; padding: var(--UWhitespace); width: 18px;" ImageUrl="data:image/svg+xml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHdpZHRoPSIzMiIgaGVpZ2h0PSIzMiIgZmlsbD0iIzAwMDAwMCIgdmlld0JveD0iMCAwIDI1NiAyNTYiPjxwYXRoIGQ9Ik0xNjgsMTM2YTgsOCwwLDAsMS04LDhIMTM2djI0YTgsOCwwLDAsMS0xNiwwVjE0NEg5NmE4LDgsMCwwLDEsMC0xNmgyNFYxMDRhOCw4LDAsMCwxLDE2LDB2MjRoMjRBOCw4LDAsMCwxLDE2OCwxMzZabTY0LTU2VjE5MmEyNCwyNCwwLDAsMS0yNCwyNEg0OGEyNCwyNCwwLDAsMS0yNC0yNFY4MEEyNCwyNCwwLDAsMSw0OCw1Nkg3NS43Mkw4NywzOS4xMkExNiwxNiwwLDAsMSwxMDAuMjgsMzJoNTUuNDRBMTYsMTYsMCwwLDEsMTY5LDM5LjEyTDE4MC4yOCw1NkgyMDhBMjQsMjQsMCwwLDEsMjMyLDgwWm0tMTYsMGE4LDgsMCwwLDAtOC04SDE3NmE4LDgsMCwwLDEtNi42Ni0zLjU2TDE1NS43Miw0OEgxMDAuMjhMODYuNjYsNjguNDRBOCw4LDAsMCwxLDgwLDcySDQ4YTgsOCwwLDAsMC04LDhWMTkyYTgsOCwwLDAsMCw4LDhIMjA4YTgsOCwwLDAsMCw4LThaIj48L3BhdGg+PC9zdmc+" runat="server" />
                            <asp:FileUpload ID="Uploader" Style="display: none;" runat="server" />
                            <asp:Button ID="CreateButton" runat="server" Font-Bold="True" Style="display: none;" OnClick="UploadFile" OnClientClick="showSpinner(); return true;" Text="Upload" />
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

                    </asp:Panel>

                    <asp:Panel ID="AddCommentPanel" runat="server" Style="margin-top: var(--UWhitespace);">
                        <asp:Label runat="server">Add note: </asp:Label>
                        <asp:Label runat="server" ID="NoteErrorLabel" Style="color: red"></asp:Label>
                        <br />
                        <div style="display: flex;">
                            <asp:TextBox ID="CommentTextBox" runat="server" Style="width: calc(100% - var(--AddButtonWidth));"></asp:TextBox>
                            <asp:Button runat="server" ID="AddCommentButton" OnClientClick="addNoteToLog();" OnClick="AddCommentButton_Click" Text="Add" Style="width: var(--AddButtonWidth);"></asp:Button>
                        </div>
                    </asp:Panel>

                    <asp:GridView ID="CommentGridView" Visible="False" runat="server" AllowSorting="True" AutoGenerateColumns="False" DataSourceID="CommentSqlDataSource" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical">
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:BoundField DataField="Comment" HeaderText="Notes" SortExpression="Comment" />
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" />
                        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#808080" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#383838" />
                    </asp:GridView>

                    <asp:SqlDataSource ID="CommentSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:ALTSConnectionString %>"
                        SelectCommand=""
                        UpdateCommand="UPDATE [ALTS].[dbo].[T_LogOperatorComments] SET Comment=@Comment WHERE [Key]=@Key"></asp:SqlDataSource>

                    <asp:LinkButton ID="StatusBoardAnchor" runat="server" OnClick="BackToStatusBoard_OnClick" Text="← Status Board" Style="padding: var(--UWhitespace) 0;"></asp:LinkButton>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

