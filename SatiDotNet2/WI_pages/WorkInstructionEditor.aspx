<%@ Page Title="Work Instruction Editor" Language="VB" MasterPageFile="~/MasterPage1.master" AutoEventWireup="false" CodeFile="WorkInstructionEditor.aspx.vb" Inherits="WI_WorkInstructionEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="MainUpdatePanel" runat="server">
        <ContentTemplate>
            <asp:Panel ID="MainContentPanel" runat="server" CssClass="ContentAutoScaler">
                <asp:Panel ID="InputPanels" runat="server" CssClass="nonSticky" Height="130px" BackColor="lightblue">
                    <table id="EditControlContainer" class="ContentAutoScaler">
                        <tr>
                            <td style="width: calc(100% - 985px)"></td>
                            <td style="width: 985px;">
                                <asp:Panel ID="Panel1" runat="server" BackColor="lightblue">
                                    <div id="editorTabs" class="editorTab">
                                        <button class="editorTab active" onclick="openEditor(event, 'OpenHTMLFiles'); return false;">Open WI Files</button>
                                        <button class="editorTab" onclick="openEditor(event, 'UploadExcelFiles'); return false;">Upload Excel Files</button>
                                        <button class="editorTab" onclick="openEditor(event, 'EditorMenu'); return false;">Editor Menu</button>
                                        <button class="editorTab" onclick="openEditor(event, 'ImageMenu'); return false;">Image/Textbox Menu</button>
                                        <button class="editorTab" onclick="openEditor(event, 'SaveCertifyFiles'); return false;">Save/Certify WI</button>
                                    </div>

                                    <div id="OpenHTMLFiles" class="editorContent" style="display: block;">
                                        <contenttemplate>
                                            <table class="ContentAutoScaler" style="height: 85px; padding-top: 5px;">
                                                <tr>
                                                    <td style="width: 2%"></td>
                                                    <td style="width: 30%">
                                                        <asp:TextBox ID="LoadLotID" runat="server" Width="100%" placeholder="Lot ID Number" Height="24px" AutoPostBack="false" Title="Work Instruction's ID number." onKeyUp="checkNums(this); return false;" onchange="imgUploadLotInfo(this, 'sideHTML'); return false;" ></asp:TextBox>
                                                    </td>
                                                    <td style="width: 2%"></td>
                                                    <td style="width: 30%">
                                                        <asp:TextBox ID="LoadRevID" runat="server" Width="100%" placeholder="Rev ID Letter(s) - (OPTIONAL)" Height="24px" AutoPostBack="false" Title="Work Instruction's Rev letter(S)." onKeyUp="checkLets(this); return false;"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 2%">
                                                        <input type="hidden" id="RogueWI" value="WI Template" />
                                                        <input type="hidden" id="RogueFC" value="WI_0" />
                                                        <asp:Button ID="HiddenLoadWI" Width="2%" Height="29px" runat="server" AutoPostBack="false" Style="visibility: hidden" />
                                                    </td>
                                                    <td style="width: 30%">
                                                        <button id="LoadWI" style="width: 100%; height: 29px;" runat="server" onclick="loadComfirmation('LOAD'); return false;">Load Work Instructions</button>
                                                    </td>
                                                    <td style="width: 2%"></td>
                                                </tr>
                                            </table>
                                        </contenttemplate>
                                    </div>
                                    <div id="UploadExcelFiles" class="editorContent">
                                        <contenttemplate>
                                            <table class="ContentAutoScaler" style="height: 85px; padding-top: 5px;">
                                                <tr>
                                                    <td style="width: 2%"></td>
                                                    <td style="width: 30%">
                                                        <asp:TextBox ID="UpLoadLotID" runat="server" Width="100%" placeholder="Lot ID Number" Height="24px" AutoPostBack="false" Title="Work Instruction's ID number." onKeyUp="checkNums(this); return false;" onchange="imgUploadLotInfo(this, 'sideHTML'); return false;"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 2%"></td>
                                                    <td style="width: 30%">
                                                        <asp:TextBox ID="UpLoadRevID" runat="server" Width="100%" placeholder="Rev ID Letter(s) (REQUIRED)" Height="24px" AutoPostBack="false" Title="Work Instruction's Rev letter(S)." onKeyUp="checkLets(this); return false;"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 2%">
                                                        <asp:Button ID="HiddenUploadWI" Width="2%" Height="29px" runat="server" AutoPostBack="false" Style="visibility: hidden" />
                                                    </td>
                                                    <td style="width: 30%">
                                                        <button id="UploadWI" style="width: 100%; height: 29px;" runat="server" onclick="loadComfirmation('UPLOAD'); return false;">Upload Work Instructions</button>
                                                    </td>
                                                    <td style="width: 2%"></td>
                                                </tr>
                                            </table>
                                        </contenttemplate>
                                    </div>
                                    <div id="EditorMenu" class="editorContent">
                                        <table class="ContentAutoScaler" style="height: 85px; padding-top: 10px;">
                                            <tr>
                                                <td style="width: 100%; text-align: center;">
                                                    <input type="hidden" name="myDoc">
                                                    <div id="toolBar1">
                                                        <ul id="cellEditMenu" class="editmenu">
                                                            <li><a>Formatting</a>
                                                                <span class="darrow">&#9660;</span>
                                                                <ul class="sub1">
                                                                    <li title="This is an empty row with the Tranferred ID information centered in it."><a>Tranferred</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('formatting', 'transferred', 'above')">Above cell's Row</a></li>
                                                                            <li style="height: 50px"><a onclick="dropFunctions('formatting', 'transferred', 'below')">Below cell's Row</a></li>
                                                                        </ul>
                                                                    </li>
                                                                    <li title="This is a empty row with the Department Name centered in it."><a>Department</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('formatting', 'department', 'above')">Above cell's Row</a></li>
                                                                            <li style="height: 50px"><a onclick="dropFunctions('formatting', 'department', 'below')">Below cell's Row</a></li>
                                                                        </ul>
                                                                    </li>
                                                                    <li title="This is an empty row with the Special Instruction Titel on it"><a>Special</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('formatting', 'special', 'above')">Above cell's Row</a></li>
                                                                            <li style="height: 50px"><a onclick="dropFunctions('formatting', 'special', 'below')">Below cell's Row</a></li>
                                                                        </ul>
                                                                    </li>
                                                                    <li title="This is an empty row to put above or below a Special Instruction row."><a>Special Row</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('formatting', 'sPanel', 'above')">Above cell's Row</a></li>
                                                                            <li style="height: 50px"><a onclick="dropFunctions('formatting', 'sPanel', 'below')">Below cell's Row</a></li>
                                                                        </ul>
                                                                    </li>
                                                                    <li title="This is an empty row to tell the program where to break apart files."><a>Page Break</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('formatting', 'pBreak', 'above')">Above cell's Row</a></li>
                                                                            <li style="height: 50px"><a onclick="dropFunctions('formatting', 'pBreak', 'below')">Below cell's Row</a></li>
                                                                        </ul>
                                                                    </li>
                                                                    <li title="This will add a new row either above or below the currently selected cells."><a>Add Row</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('formatting', 'addRow', 'above')">Above cell's Row</a></li>
                                                                            <li style="height: 50px"><a onclick="dropFunctions('formatting', 'addRow', 'below')">Below cell's Row</a></li>
                                                                        </ul>
                                                                    </li>
                                                                    <li title="This will add a cell either to the left or right of the currently selected cells."><a>Add Cell</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('formatting', 'addCell', 'left')">Left of Cell</a></li>
                                                                            <li style="height: 50px"><a onclick="dropFunctions('formatting', 'addCell', 'right')">Right of Cell</a></li>
                                                                        </ul>
                                                                    </li>
                                                                    <li title="This will remove the row where the currently selected cells are."><a onclick="dropFunctions('formatting', 'delete', 'deleteRow')">Delete Row</a></li>
                                                                    <li title="This will remove the cell where the currently selected cells are."><a onclick="dropFunctions('formatting', 'delete', 'deleteCell')">Delete Cell</a></li>
                                                                    <li title="This is will change the text warp affect. It will either turn line warp off or on"><a>Text Warp</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('formatting', 'textWarp', 'on')">On</a></li>
                                                                            <li><a onclick="dropFunctions('formatting', 'textWarp', 'off')">Off</a></li>
                                                                        </ul>
                                                                    </li>
                                                                    <li title="This will set the selected cells default formating. &#013;This will remove all formating and settings you have applied."><a>Cell Format</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('formatting', 'resetClassFormat', 'excelCell')">Default</a></li>
                                                                            <li style="height: 50px"><a onclick="dropFunctions('formatting', 'resetClassFormat', 'engineeringCell')">Engineering</a></li>
                                                                        </ul>
                                                                    </li>
                                                                </ul>
                                                            </li>
                                                            <li><a>Font Style</a>
                                                                <span class="darrow">&#9660;</span>
                                                                <ul class="sub1">
                                                                    <li><a onclick="dropFunctions('fontname', 'nothing', 'Arial');">Arial</a></li>
                                                                    <li><a onclick="dropFunctions('fontname', 'nothing', 'Arial Black');">Arial Black</a></li>
                                                                    <li><a onclick="dropFunctions('fontname', 'nothing', 'Courier New');">Courier New</a></li>
                                                                    <li style="height: 50px"><a onclick="dropFunctions('fontname', 'nothing', 'Times New Roman');" style="font-size: 8pt;">Times New Roman</a></li>
                                                                </ul>
                                                            </li>
                                                            <li><a>Font Size</a>
                                                                <span class="darrow">&#9660;</span>
                                                                <ul class="sub1">
                                                                    <li><a onclick="dropFunctions('fontSize', 'fontsize', '1')">Very small</a></li>
                                                                    <li><a onclick="dropFunctions('fontSize', 'fontsize', '2')">A bit small</a></li>
                                                                    <li><a onclick="dropFunctions('fontSize', 'fontsize', '3')">Normal</a></li>
                                                                    <li><a onclick="dropFunctions('fontSize', 'fontsize', '4')">Medium-large</a></li>
                                                                    <li><a onclick="dropFunctions('fontSize', 'fontsize', '5')">Big</a></li>
                                                                    <li><a onclick="dropFunctions('fontSize', 'fontsize', '6')">Very big</a></li>
                                                                    <li><a onclick="dropFunctions('fontSize', 'fontsize', '7')">Maximum</a></li>
                                                                    <li style="height: 50px" title="Custom Font Size, This cannot be undone by the undo button">
                                                                        <a>
                                                                            <input id="CustomFS" style="width: 65px" placeholder=" -- ">
                                                                            <span>px</span>
                                                                        </a>
                                                                    </li>
                                                                </ul>
                                                            </li>
                                                            <li><a>Font Color</a>
                                                                <span class="darrow">&#9660;</span>
                                                                <ul class="sub1">
                                                                    <li><a onclick="dropFunctions('forecolor', 'nothing', 'white')">White</a></li>
                                                                    <li><a onclick="dropFunctions('forecolor', 'nothing', 'red')">Red</a></li>
                                                                    <li><a onclick="dropFunctions('forecolor', 'nothing', 'blue')">Blue</a></li>
                                                                    <li><a onclick="dropFunctions('forecolor', 'nothing', 'green')">Green</a></li>
                                                                    <li><a onclick="dropFunctions('forecolor', 'nothing', 'black')">Black</a></li>
                                                                    <li style="height: 50px"><a>
                                                                        <input type="color" id="fontCW" title="Font Color" class="colorWell"></a></li>
                                                                </ul>
                                                            </li>
                                                            <li><a>Border Size</a>
                                                                <span class="darrow">&#9660;</span>
                                                                <ul class="sub1">
                                                                    <li><a>Full</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderWidth', '')">Remove</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderWidth', '1')">1 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderWidth', '2')">2 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderWidth', '3')">3 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderWidth', '4')">4 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderWidth', '5')">5 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderWidth', '6')">6 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderWidth', '7')">7 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderWidth', '8')">8 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderWidth', '9')">9 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderWidth', '10')">10 px</a></li>
                                                                            <li style="height: 50px" title="Custom border Size, This cannot be undone by the undo button">
                                                                                <a>
                                                                                    <input id="CustomBF" style="width: 65px" placeholder=" -- ">
                                                                                    <span>px</span>
                                                                                </a>
                                                                            </li>
                                                                        </ul>
                                                                    </li>
                                                                    <li><a>Top</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderTopWidth', '')">Remove</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderTopWidth', '1')">1 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderTopWidth', '2')">2 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderTopWidth', '3')">3 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderTopWidth', '4')">4 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderTopWidth', '5')">5 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderTopWidth', '6')">6 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderTopWidth', '7')">7 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderTopWidth', '8')">8 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderTopWidth', '9')">9 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderTopWidth', '10')">10 px</a></li>
                                                                            <li style="height: 50px" title="Custom border Size, This cannot be undone by the undo button">
                                                                                <a>
                                                                                    <input id="CustomBT" style="width: 65px" placeholder=" -- ">
                                                                                    <span>px</span>
                                                                                </a>
                                                                            </li>
                                                                        </ul>
                                                                    </li>
                                                                    <li><a>Left</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderLeftWidth', '')">Remove</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderLeftWidth', '1')">1 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderLeftWidth', '2')">2 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderLeftWidth', '3')">3 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderLeftWidth', '4')">4 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderLeftWidth', '5')">5 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderLeftWidth', '6')">6 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderLeftWidth', '7')">7 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderLeftWidth', '8')">8 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderLeftWidth', '9')">9 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderLeftWidth', '10')">10 px</a></li>
                                                                            <li style="height: 50px" title="Custom border Size, This cannot be undone by the undo button">
                                                                                <a>
                                                                                    <input id="CustomBL" style="width: 65px" placeholder=" -- ">
                                                                                    <span>px</span>
                                                                                </a>
                                                                            </li>
                                                                        </ul>
                                                                    </li>
                                                                    <li><a>Right</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderRightWidth', '')">Remove</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderRightWidth', '1')">1 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderRightWidth', '2')">2 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderRightWidth', '3')">3 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderRightWidth', '4')">4 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderRightWidth', '5')">5 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderRightWidth', '6')">6 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderRightWidth', '7')">7 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderRightWidth', '8')">8 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderRightWidth', '9')">9 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderRightWidth', '10')">10 px</a></li>
                                                                            <li style="height: 50px" title="Custom border Size, This cannot be undone by the undo button">
                                                                                <a>
                                                                                    <input id="CustomBR" style="width: 65px" placeholder=" -- ">
                                                                                    <span>px</span>
                                                                                </a>
                                                                            </li>
                                                                        </ul>
                                                                    </li>
                                                                    <li style="height: 50px"><a>Bottom</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderBottomWidth', '')">Remove</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderBottomWidth', '1')">1 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderBottomWidth', '2')">2 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderBottomWidth', '3')">3 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderBottomWidth', '4')">4 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderBottomWidth', '5')">5 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderBottomWidth', '6')">6 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderBottomWidth', '7')">7 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderBottomWidth', '8')">8 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderBottomWidth', '9')">9 px</a></li>
                                                                            <li><a onclick="dropFunctions('borderSize', 'borderBottomWidth', '10')">10 px</a></li>
                                                                            <li style="height: 50px" title="Custom border Size, This cannot be undone by the undo button">
                                                                                <a>
                                                                                    <input id="CustomBB" style="width: 65px" placeholder=" -- " />
                                                                                    <span>px</span>
                                                                                </a>
                                                                            </li>
                                                                        </ul>
                                                                    </li>
                                                                </ul>
                                                            </li>
                                                            <li><a>Border Color</a>
                                                                <span class="darrow">&#9660;</span>
                                                                <ul class="sub1">
                                                                    <li><a>Full</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderColor', '')">Remove</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderColor', 'white')">White</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderColor', 'red')">Red</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderColor', 'blue')">Blue</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderColor', 'green')">Green</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderColor', 'black')">Black</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderColor', 'yellow')">Yellow</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderColor', 'gray')">gray</a></li>
                                                                            <li style="height: 50px"><a>
                                                                                <input type="color" id="fullCW" title="Full Border Color" class="colorWell"></a></li>
                                                                        </ul>
                                                                    </li>
                                                                    <li><a>Top</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderTopColor', '')">Remove</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderTopColor', 'white')">White</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderTopColor', 'red')">Red</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderTopColor', 'blue')">Blue</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderTopColor', 'green')">Green</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderTopColor', 'black')">Black</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderTopColor', 'yellow')">Yellow</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderTopColor', 'gray')">gray</a></li>
                                                                            <li style="height: 50px"><a>
                                                                                <input type="color" id="topCW" title="Top Border Color" class="colorWell"></a></li>
                                                                        </ul>
                                                                    </li>
                                                                    <li><a>Left</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderLeftColor', '')">Remove</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderLeftColor', 'white')">White</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderLeftColor', 'red')">Red</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderLeftColor', 'blue')">Blue</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderLeftColor', 'green')">Green</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderLeftColor', 'black')">Black</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderLeftColor', 'yellow')">Yellow</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderLeftColor', 'gray')">gray</a></li>
                                                                            <li style="height: 50px"><a>
                                                                                <input type="color" id="leftCW" title="Left Border Color" class="colorWell"></a></li>
                                                                        </ul>
                                                                    </li>
                                                                    <li><a>Right</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderRightColor', '')">Remove</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderRightColor', 'white')">White</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderRightColor', 'red')">Red</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderRightColor', 'blue')">Blue</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderRightColor', 'green')">Green</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderRightColor', 'black')">Black</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderRightColor', 'yellow')">Yellow</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderRightColor', 'gray')">gray</a></li>
                                                                            <li style="height: 50px"><a>
                                                                                <input type="color" id="rightCW" title="Right Border Color" class="colorWell"></a></li>
                                                                        </ul>
                                                                    </li>
                                                                    <li style="height: 50px"><a>Bottom</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderBottomColor', '')">Remove</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderBottomColor', 'white')">White</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderBottomColor', 'red')">Red</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderBottomColor', 'blue')">Blue</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderBottomColor', 'green')">Green</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderBottomColor', 'black')">Black</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderBottomColor', 'yellow')">Yellow</a></li>
                                                                            <li><a onclick="dropFunctions('borderColor', 'borderBottomColor', 'gray')">gray</a></li>
                                                                            <li style="height: 50px"><a>
                                                                                <input type="color" id="bottomCW" title="Bottom Border Color" class="colorWell"></a></li>
                                                                        </ul>
                                                                    </li>
                                                                </ul>
                                                            </li>
                                                            <li><a>Coloring</a>
                                                                <span class="darrow">&#9660;</span>
                                                                <ul class="sub1">
                                                                    <li><a>Cell Fill</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('coloring', 'cellFill', '')">Remove</a></li>
                                                                            <li><a onclick="dropFunctions('coloring', 'cellFill', 'white')">White</a></li>
                                                                            <li><a onclick="dropFunctions('coloring', 'cellFill', 'red')">Red</a></li>
                                                                            <li><a onclick="dropFunctions('coloring', 'cellFill', 'blue')">Blue</a></li>
                                                                            <li><a onclick="dropFunctions('coloring', 'cellFill', 'green')">Green</a></li>
                                                                            <li><a onclick="dropFunctions('coloring', 'cellFill', 'black')">Black</a></li>
                                                                            <li><a onclick="dropFunctions('coloring', 'cellFill', 'yellow')">Yellow</a></li>
                                                                            <li><a onclick="dropFunctions('coloring', 'cellFill', 'gray')">gray</a></li>
                                                                            <li style="height: 50px"><a>
                                                                                <input type="color" id="backCW" title="Background Color" class="colorWell"></a></li>
                                                                        </ul>
                                                                    </li>
                                                                    <li style="height: 50px"><a>Highlight</a>
                                                                        <span class="rarrow">&#9654;</span>
                                                                        <ul class="sub2">
                                                                            <li><a onclick="dropFunctions('coloring', 'backcolor', '')">Remove</a></li>
                                                                            <li><a onclick="dropFunctions('coloring', 'backcolor', 'white')">White</a></li>
                                                                            <li><a onclick="dropFunctions('coloring', 'backcolor', 'red')">Red</a></li>
                                                                            <li><a onclick="dropFunctions('coloring', 'backcolor', 'blue')">Blue</a></li>
                                                                            <li><a onclick="dropFunctions('coloring', 'backcolor', 'green')">Green</a></li>
                                                                            <li><a onclick="dropFunctions('coloring', 'backcolor', 'black')">Black</a></li>
                                                                            <li><a onclick="dropFunctions('coloring', 'backcolor', 'yellow')">Yellow</a></li>
                                                                            <li><a onclick="dropFunctions('coloring', 'backcolor', 'gray')">gray</a></li>
                                                                            <li style="height: 50px"><a>
                                                                                <input type="color" id="highCW" title="Highlight Color" class="colorWell"></a></li>
                                                                        </ul>
                                                                    </li>
                                                                </ul>
                                                            </li>
                                                            <li><a>Row Height</a>
                                                                <span class="darrow">&#9660;</span>
                                                                <ul class="sub1">
                                                                    <li title="Note: If you make the row height short. &#013;It will be difficult to select the cells in that row &#013;True to drag select"><a onclick="dropFunctions('rowHeight', 'height', '5')">5 px</a></li>
                                                                    <li><a onclick="dropFunctions('rowHeight', 'height', '10')">10 px</a></li>
                                                                    <li><a onclick="dropFunctions('rowHeight', 'height', '15')">15 px</a></li>
                                                                    <li><a onclick="dropFunctions('rowHeight', 'height', '20')">20 px</a></li>
                                                                    <li><a onclick="dropFunctions('rowHeight', 'height', '25')">25 px</a></li>
                                                                    <li><a onclick="dropFunctions('rowHeight', 'height', '30')">30 px</a></li>
                                                                    <li><a onclick="dropFunctions('rowHeight', 'height', '35')">35 px</a></li>
                                                                    <li><a onclick="dropFunctions('rowHeight', 'height', '40')">40 px</a></li>
                                                                    <li><a onclick="dropFunctions('rowHeight', 'height', '45')">45 px</a></li>
                                                                    <li><a onclick="dropFunctions('rowHeight', 'height', '50')">50 px</a></li>
                                                                    <li style="height: 50px" title="Custom Row Height, This cannot be undone by the undo button">
                                                                        <a>
                                                                            <input id="CustomRH" style="width: 65px;" placeholder=" -- " />
                                                                            <span>px</span>
                                                                        </a>
                                                                    </li>
                                                                </ul>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%">
                                                    <div id="toolBar2">
                                                        <img class="intLink" title="Clean" onclick="cleanDoc(); return false;" src="data:image/gif;base64,R0lGODlhFgAWAIQbAD04KTRLYzFRjlldZl9vj1dusY14WYODhpWIbbSVFY6O7IOXw5qbms+wUbCztca0ccS4kdDQjdTLtMrL1O3YitHa7OPcsd/f4PfvrvDv8Pv5xv///////////////////yH5BAEKAB8ALAAAAAAWABYAAAV84CeOZGmeaKqubMteyzK547QoBcFWTm/jgsHq4rhMLoxFIehQQSAWR+Z4IAyaJ0kEgtFoLIzLwRE4oCQWrxoTOTAIhMCZ0tVgMBQKZHAYyFEWEV14eQ8IflhnEHmFDQkAiSkQCI2PDC4QBg+OAJc0ewadNCOgo6anqKkoIQA7" />
                                                        <img class="intLinkCC" title="Clean Cell" onclick="cleanCell(); return false;" src="data:image/gif;base64,R0lGODlhFgAWAIQbAD04KTRLYzFRjlldZl9vj1dusY14WYODhpWIbbSVFY6O7IOXw5qbms+wUbCztca0ccS4kdDQjdTLtMrL1O3YitHa7OPcsd/f4PfvrvDv8Pv5xv///////////////////yH5BAEKAB8ALAAAAAAWABYAAAV84CeOZGmeaKqubMteyzK547QoBcFWTm/jgsHq4rhMLoxFIehQQSAWR+Z4IAyaJ0kEgtFoLIzLwRE4oCQWrxoTOTAIhMCZ0tVgMBQKZHAYyFEWEV14eQ8IflhnEHmFDQkAiSkQCI2PDC4QBg+OAJc0ewadNCOgo6anqKkoIQA7" />
                                                        <img class="intLink" title="Print" onclick="printDoc();" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABYAAAAWCAYAAADEtGw7AAAABGdBTUEAALGPC/xhBQAAAAZiS0dEAP8A/wD/oL2nkwAAAAlwSFlzAAALEwAACxMBAJqcGAAAAAd0SU1FB9oEBxcZFmGboiwAAAAIdEVYdENvbW1lbnQA9syWvwAAAuFJREFUOMvtlUtsjFEUx//n3nn0YdpBh1abRpt4LFqtqkc3jRKkNEIsiIRIBBEhJJpKlIVo4m1RRMKKjQiRMJRUqUdKPT71qpIpiRKPaqdF55tv5vvusZjQTjOlseUkd3Xu/3dPzusC/22wtu2wRn+jG5So/OCDh8ycMJDflehMlkJkVK7KUYN+ufzA/RttH76zaVocDptRxzQtNi3mRWuPc+6cKtlXZ/sddP2uu9uXlmYXZ6Qm8v4Tz8lhF1H+zDQXt7S8oLMXtbF4e8QaFHjj3kbP2MzkktHpiTjp9VH6iHiA+whtAsX5brpwueMGdONdf/2A4M7ukDs1JW662+XkqTkeUoqjKtOjm2h53YFL15pSJ04Zc94wdtibr26fXlC2mzRvBccEbz2kiRFD414tKMlEZbVGT33+qCoHgha81SWYsew0r1uzfNylmtpx80pngQQ91LwVk2JGvGnfvZG6YcYRAT16GFtW5kKKfo1EQLtfh5Q2etT0BIWF+aitq4fDbk+ImYo1OxvGF03waFJQvBCkvDffRyEtxQiFFYgAZTHS0zwAGD7fG5TNnYNTp8/FzvGwJOfmgG7GOx0SAKKgQgDMgKBI0NJGMEImpGDk5+WACEwEd0ywblhGUZ4Hw5OdUekRBLT7DTgdEgxACsIznx8zpmWh7k4rkpJcuHDxCul6MDsmmBXDlWCH2+XozSgBnzsNCEE4euYV4pwCpsWYPW0UHDYBKSWu1NYjENDReqtKjwn2+zvtTc1vMSTB/mvev/WEYSlASsLimcOhOBJxw+N3aP/SjefNL5GePZmpu4kG7OPr1+tOfPyUu3BecWYKcwQcDFmwFKAUo90fhKDInBCAmvqnyMgqUEagQwCoHBDc1rjv9pIlD8IbVkz6qYViIBQGTJPx4k0XpIgEZoRN1Da0cij4VfR0ta3WvBXH/rjdCufv6R2zPgPH/e4pxSBCpeatqPrjNiso203/5s/zA171Mv8+w1LOAAAAAElFTkSuQmCC">
                                                        <img class="intLink" title="Undo" onclick="formatDoc('undo');" src="data:image/gif;base64,R0lGODlhFgAWAOMKADljwliE33mOrpGjuYKl8aezxqPD+7/I19DV3NHa7P///////////////////////yH5BAEKAA8ALAAAAAAWABYAAARR8MlJq7046807TkaYeJJBnES4EeUJvIGapWYAC0CsocQ7SDlWJkAkCA6ToMYWIARGQF3mRQVIEjkkSVLIbSfEwhdRIH4fh/DZMICe3/C4nBQBADs=" />
                                                        <img class="intLink" title="Redo" onclick="formatDoc('redo');" src="data:image/gif;base64,R0lGODlhFgAWAMIHAB1ChDljwl9vj1iE34Kl8aPD+7/I1////yH5BAEKAAcALAAAAAAWABYAAANKeLrc/jDKSesyphi7SiEgsVXZEATDICqBVJjpqWZt9NaEDNbQK1wCQsxlYnxMAImhyDoFAElJasRRvAZVRqqQXUy7Cgx4TC6bswkAOw==" />
                                                        <img class="intLink" title="Remove formatting" onclick="formatDoc('removeFormat')" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABYAAAAWCAYAAADEtGw7AAAABGdBTUEAALGPC/xhBQAAAAZiS0dEAP8A/wD/oL2nkwAAAAlwSFlzAAAOxAAADsQBlSsOGwAAAAd0SU1FB9oECQMCKPI8CIIAAAAIdEVYdENvbW1lbnQA9syWvwAAAuhJREFUOMtjYBgFxAB501ZWBvVaL2nHnlmk6mXCJbF69zU+Hz/9fB5O1lx+bg45qhl8/fYr5it3XrP/YWTUvvvk3VeqGXz70TvbJy8+Wv39+2/Hz19/mGwjZzuTYjALuoBv9jImaXHeyD3H7kU8fPj2ICML8z92dlbtMzdeiG3fco7J08foH1kurkm3E9iw54YvKwuTuom+LPt/BgbWf3//sf37/1/c02cCG1lB8f//f95DZx74MTMzshhoSm6szrQ/a6Ir/Z2RkfEjBxuLYFpDiDi6Af///2ckaHBp7+7wmavP5n76+P2ClrLIYl8H9W36auJCbCxM4szMTJac7Kza////R3H1w2cfWAgafPbqs5g7D95++/P1B4+ECK8tAwMDw/1H7159+/7r7ZcvPz4fOHbzEwMDwx8GBgaGnNatfHZx8zqrJ+4VJBh5CQEGOySEua/v3n7hXmqI8WUGBgYGL3vVG7fuPK3i5GD9/fja7ZsMDAzMG/Ze52mZeSj4yu1XEq/ff7W5dvfVAS1lsXc4Db7z8C3r8p7Qjf///2dnZGxlqJuyr3rPqQd/Hhyu7oSpYWScylDQsd3kzvnH738wMDzj5GBN1VIWW4c3KDon7VOvm7S3paB9u5qsU5/x5KUnlY+eexQbkLNsErK61+++VnAJcfkyMTIwffj0QwZbJDKjcETs1Y8evyd48toz8y/ffzv//vPP4veffxpX77z6l5JewHPu8MqTDAwMDLzyrjb/mZm0JcT5Lj+89+Ybm6zz95oMh7s4XbygN3Sluq4Mj5K8iKMgP4f0////fv77//8nLy+7MCcXmyYDAwODS9jM9tcvPypd35pne3ljdjvj26+H2dhYpuENikgfvQeXNmSl3tqepxXsqhXPyc666s+fv1fMdKR3TK72zpix8nTc7bdfhfkEeVbC9KhbK/9iYWHiErbu6MWbY/7//8/4//9/pgOnH6jGVazvFDRtq2VgiBIZrUTIBgCk+ivHvuEKwAAAAABJRU5ErkJggg==">
                                                        <img class="intLink" title="Bold" onclick="formatDoc('bold');" src="data:image/gif;base64,R0lGODlhFgAWAID/AMDAwAAAACH5BAEAAAAALAAAAAAWABYAQAInhI+pa+H9mJy0LhdgtrxzDG5WGFVk6aXqyk6Y9kXvKKNuLbb6zgMFADs=" />
                                                        <img class="intLink" title="Italic" onclick="formatDoc('italic');" src="data:image/gif;base64,R0lGODlhFgAWAKEDAAAAAF9vj5WIbf///yH5BAEAAAMALAAAAAAWABYAAAIjnI+py+0Po5x0gXvruEKHrF2BB1YiCWgbMFIYpsbyTNd2UwAAOw==" />
                                                        <img class="intLink" title="Underline" onclick="formatDoc('underline');" src="data:image/gif;base64,R0lGODlhFgAWAKECAAAAAF9vj////////yH5BAEAAAIALAAAAAAWABYAAAIrlI+py+0Po5zUgAsEzvEeL4Ea15EiJJ5PSqJmuwKBEKgxVuXWtun+DwxCCgA7" />
                                                        <img class="intLink" title="Left align" onclick="formatDoc('justifyleft');" src="data:image/gif;base64,R0lGODlhFgAWAID/AMDAwAAAACH5BAEAAAAALAAAAAAWABYAQAIghI+py+0Po5y02ouz3jL4D4JMGELkGYxo+qzl4nKyXAAAOw==" />
                                                        <img class="intLink" title="Center align" onclick="formatDoc('justifycenter');" src="data:image/gif;base64,R0lGODlhFgAWAID/AMDAwAAAACH5BAEAAAAALAAAAAAWABYAQAIfhI+py+0Po5y02ouz3jL4D4JOGI7kaZ5Bqn4sycVbAQA7" />
                                                        <img class="intLink" title="Right align" onclick="formatDoc('justifyright');" src="data:image/gif;base64,R0lGODlhFgAWAID/AMDAwAAAACH5BAEAAAAALAAAAAAWABYAQAIghI+py+0Po5y02ouz3jL4D4JQGDLkGYxouqzl43JyVgAAOw==" />
                                                        <img class="intLink" title="Numbered list" onclick="formatDoc('insertorderedlist');" src="data:image/gif;base64,R0lGODlhFgAWAMIGAAAAADljwliE35GjuaezxtHa7P///////yH5BAEAAAcALAAAAAAWABYAAAM2eLrc/jDKSespwjoRFvggCBUBoTFBeq6QIAysQnRHaEOzyaZ07Lu9lUBnC0UGQU1K52s6n5oEADs=" />
                                                        <img class="intLink" title="Dotted list" onclick="formatDoc('insertunorderedlist');" src="data:image/gif;base64,R0lGODlhFgAWAMIGAAAAAB1ChF9vj1iE33mOrqezxv///////yH5BAEAAAcALAAAAAAWABYAAAMyeLrc/jDKSesppNhGRlBAKIZRERBbqm6YtnbfMY7lud64UwiuKnigGQliQuWOyKQykgAAOw==" />
                                                        <img class="intLink" title="Delete indentation" onclick="formatDoc('outdent');" src="data:image/gif;base64,R0lGODlhFgAWAMIHAAAAADljwliE35GjuaezxtDV3NHa7P///yH5BAEAAAcALAAAAAAWABYAAAM2eLrc/jDKCQG9F2i7u8agQgyK1z2EIBil+TWqEMxhMczsYVJ3e4ahk+sFnAgtxSQDqWw6n5cEADs=" />
                                                        <img class="intLink" title="Add indentation" onclick="formatDoc('indent');" src="data:image/gif;base64,R0lGODlhFgAWAOMIAAAAADljwl9vj1iE35GjuaezxtDV3NHa7P///////////////////////////////yH5BAEAAAgALAAAAAAWABYAAAQ7EMlJq704650B/x8gemMpgugwHJNZXodKsO5oqUOgo5KhBwWESyMQsCRDHu9VOyk5TM9zSpFSr9gsJwIAOw==" />
                                                        <img class="intLink" title="Hyperlink" onclick="var sLnk=prompt('Write the URL here','http:\/\/');if(sLnk&&sLnk!=''&&sLnk!='http://'){formatDoc('createlink',sLnk)}" src="data:image/gif;base64,R0lGODlhFgAWAOMKAB1ChDRLY19vj3mOrpGjuaezxrCztb/I19Ha7Pv8/f///////////////////////yH5BAEKAA8ALAAAAAAWABYAAARY8MlJq7046827/2BYIQVhHg9pEgVGIklyDEUBy/RlE4FQF4dCj2AQXAiJQDCWQCAEBwIioEMQBgSAFhDAGghGi9XgHAhMNoSZgJkJei33UESv2+/4vD4TAQA7" />
                                                        <img class="intLink" title="Cut" onclick="formatDoc('cut');" src="data:image/gif;base64,R0lGODlhFgAWAIQSAB1ChBFNsRJTySJYwjljwkxwl19vj1dusYODhl6MnHmOrpqbmpGjuaezxrCztcDCxL/I18rL1P///////////////////////////////////////////////////////yH5BAEAAB8ALAAAAAAWABYAAAVu4CeOZGmeaKqubDs6TNnEbGNApNG0kbGMi5trwcA9GArXh+FAfBAw5UexUDAQESkRsfhJPwaH4YsEGAAJGisRGAQY7UCC9ZAXBB+74LGCRxIEHwAHdWooDgGJcwpxDisQBQRjIgkDCVlfmZqbmiEAOw==" />
                                                        <img class="intLink" title="Copy" onclick="formatDoc('copy');" src="data:image/gif;base64,R0lGODlhFgAWAIQcAB1ChBFNsTRLYyJYwjljwl9vj1iE31iGzF6MnHWX9HOdz5GjuYCl2YKl8ZOt4qezxqK63aK/9KPD+7DI3b/I17LM/MrL1MLY9NHa7OPs++bx/Pv8/f///////////////yH5BAEAAB8ALAAAAAAWABYAAAWG4CeOZGmeaKqubOum1SQ/kPVOW749BeVSus2CgrCxHptLBbOQxCSNCCaF1GUqwQbBd0JGJAyGJJiobE+LnCaDcXAaEoxhQACgNw0FQx9kP+wmaRgYFBQNeAoGihCAJQsCkJAKOhgXEw8BLQYciooHf5o7EA+kC40qBKkAAAGrpy+wsbKzIiEAOw==" />
                                                        <button class="visButton" title="Insert &#xB5;" onclick="formatDoc('insertHTML', '&mu;'); return false;">&#xB5;</button>
                                                        <button class="visButton" title="Merge Cells" onclick="mergeCells(); return false;">&#x2630;</button>
                                                        <button class="visButton" title="Unmerge Cells" onclick="unmergeCells(); return false;">&#x2632;</button>
                                                        <input type="checkbox" name="switchMode" id="switchBox" onchange="setDocMode(this.checked); return false;" style="vertical-align: top; text-align: center" title="View/Edit Source Code" /><label for="switchBox" style="vertical-align: top; text-align: center">< /></label>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="ImageMenu" class="editorContent">
                                        <table class="ContentAutoScaler" style="height: 85px; padding-top: 0px;">
                                            <tr>
                                                <td>
                                                    <iframe id="WIImgUploader" src="WIImageUploader.aspx" class="wiImageUploaderTrick"></iframe>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table style="width: 100%; height: 30px">
                                                        <tr>
                                                            <td style="width: 18%;">
                                                                <asp:Label ID="placeText1" runat="server" Text="Adjust  "></asp:Label>
                                                                <asp:Label ID="placeText2" runat="server" Text="selection's" ForeColor="Red" BorderStyle="Dashed" BorderColor="Red" BorderWidth="1px"></asp:Label>
                                                                <asp:Label ID="placeText3" runat="server" Text="  placement"></asp:Label>
                                                            </td>
                                                            <td style="width: 8%;">
                                                                <input type="text" id="inputShift" placeholder="Pixels (1-100)" title="Shift selected by this many pixels" style="width: 100%" />
                                                            </td>
                                                            <td style="width: 13%; padding-left: 12px; padding-right: 12px;">
                                                                <button class="visButton" style="width: 22%; margin: 0px; padding: 0px;" onclick="moveImgHolderSingle(this); return false;" title="Move Up">&#8593;</button>
                                                                <button class="visButton" style="width: 22%; margin: 0px; padding: 0px;" onclick="moveImgHolderSingle(this); return false;" title="Move Down">&#8595;</button>
                                                                <button class="visButton" style="width: 22%; margin: 0px; padding: 0px;" onclick="moveImgHolderSingle(this); return false;" title="Move Left">&#8592;</button>
                                                                <button class="visButton" style="width: 22%; margin: 0px; padding: 0px;" onclick="moveImgHolderSingle(this); return false;" title="Move Right">&#8594;</button>
                                                            </td>
                                                            <td style="width: 4%"></td>
                                                            <td style="width: 15%">
                                                                <asp:Label ID="depthText1" runat="server" Text="Adjust  "></asp:Label>
                                                                <asp:Label ID="depthText2" runat="server" Text="selection's" ForeColor="Red" BorderStyle="Dashed" BorderColor="Red" BorderWidth="1px"></asp:Label>
                                                                <asp:Label ID="depthText3" runat="server" Text="  depth"></asp:Label>
                                                            </td>
                                                            <td style="width: 5%">
                                                                <button class="visButton" style="width: 45%; margin: 0px; padding: 0px;" onclick="moveImgLevel(this, true); return false;" title="Move up in level - This will move the image towards you">&#x271A;</button>
                                                                <button class="visButton" style="width: 45%; margin: 0px; padding: 0px;" onclick="moveImgLevel(this, false); return false;" title="Move down in level - This will move the image away from you">&#x268A;</button>
                                                            </td>
                                                            <td style="width: 15%;">
                                                                <div id="toolBar3">
                                                                    <ul id="imgTxtEditMenu" class="editmenu" style="width: 235px">
                                                                        <li style="width: 235px"><a style="width: 235px">Textbox Editor Menu</a>
                                                                            <span class="darrow">&#9660;</span>
                                                                            <ul class="sub1">
                                                                                <li><a>Font Style</a>
                                                                                    <span class="rarrow">&#9654;</span>
                                                                                    <ul class="sub2">
                                                                                        <li><a onclick="dropTextFunctions('fontStyle', 'Arial');">Arial</a></li>
                                                                                        <li><a onclick="dropTextFunctions('fontStyle', 'Arial Black');">Arial Black</a></li>
                                                                                        <li><a onclick="dropTextFunctions('fontStyle', 'Courier New');">Courier New</a></li>
                                                                                        <li style="height: 50px"><a onclick="dropTextFunctions('fontStyle', 'Times New Roman');" style="font-size: 8pt;">Times New Roman</a></li>
                                                                                    </ul>
                                                                                </li>
                                                                                <li><a>Font Size</a>
                                                                                    <span class="rarrow">&#9654;</span>
                                                                                    <ul class="sub2">
                                                                                        <li><a onclick="dropTextFunctions('fontSize', 'xx-small')">Very small</a></li>
                                                                                        <li><a onclick="dropTextFunctions('fontSize', 'x-small')">A bit small</a></li>
                                                                                        <li><a onclick="dropTextFunctions('fontSize', 'small')">small</a></li>
                                                                                        <li><a onclick="dropTextFunctions('fontSize', 'medium')">Normal</a></li>
                                                                                        <li><a onclick="dropTextFunctions('fontSize', 'large')">Large</a></li>
                                                                                        <li><a onclick="dropTextFunctions('fontSize', 'x-large')">A bit Large</a></li>
                                                                                        <li><a onclick="dropTextFunctions('fontSize', 'xx-large')">Very Large</a></li>
                                                                                        <li><a onclick="dropTextFunctions('fontSize', 'xxx-large')">Maximum</a></li>
                                                                                    </ul>
                                                                                </li>
                                                                                <li><a>Font Color</a>
                                                                                    <span class="rarrow">&#9654;</span>
                                                                                    <ul class="sub2">
                                                                                        <li><a onclick="dropTextFunctions('fontColor', 'white')">White</a></li>
                                                                                        <li><a onclick="dropTextFunctions('fontColor', 'red')">Red</a></li>
                                                                                        <li><a onclick="dropTextFunctions('fontColor', 'blue')">Blue</a></li>
                                                                                        <li><a onclick="dropTextFunctions('fontColor', 'green')">Green</a></li>
                                                                                        <li><a onclick="dropTextFunctions('fontColor', 'black')">Black</a></li>
                                                                                    </ul>
                                                                                </li>
                                                                                <li><a>Background</a>
                                                                                    <span class="rarrow">&#9654;</span>
                                                                                    <ul class="sub2">
                                                                                        <li><a onclick="dropTextFunctions('backColor', 'white')">White</a></li>
                                                                                        <li><a onclick="dropTextFunctions('backColor', 'red')">Red</a></li>
                                                                                        <li><a onclick="dropTextFunctions('backColor', 'blue')">Blue</a></li>
                                                                                        <li><a onclick="dropTextFunctions('backColor', 'green')">Green</a></li>
                                                                                        <li><a onclick="dropTextFunctions('backColor', 'black')">Black</a></li>
                                                                                        <li><a onclick="dropTextFunctions('backColor', 'yellow')">Yellow</a></li>
                                                                                        <li><a onclick="dropTextFunctions('backColor', 'gray')">gray</a></li>
                                                                                    </ul>
                                                                                </li>
                                                                                <li><a>Bold</a>
                                                                                    <span class="rarrow">&#9654;</span>
                                                                                    <ul class="sub2">
                                                                                        <li><a onclick="dropTextFunctions('fontWeight', 'bold')">Bold</a></li>
                                                                                        <li><a onclick="dropTextFunctions('fontWeight', 'normal')">Normal</a></li>
                                                                                    </ul>
                                                                                </li>
                                                                            </ul>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="SaveCertifyFiles" class="editorContent">
                                        <contenttemplate>
                                            <table class="ContentAutoScaler" style="height: 85px; padding-top: 5px;">
                                                <tr>
                                                    <td style="width: 12%">
                                                        <asp:Label ID="CurrentLabel" runat="server" Width="100%">Current Name: </asp:Label>
                                                    </td>
                                                    <td style="width: 1%"></td>
                                                    <td style="width: 12%">
                                                        <label id="LotLabel" runat="server" Width="100%">Lot ID</label>
                                                        <asp:HiddenField ID="LotHid" runat="server" Value="Lot ID"></asp:HiddenField>
                                                    </td>
                                                    <td style="width: 1%"></td>
                                                    <td style="width: 12%">
                                                        <label id="RevLabel" runat="server" Width="100%">Rev ID</label>
                                                        <asp:HiddenField ID="RevHid" runat="server" Value="Rev ID"></asp:HiddenField>
                                                    </td>
                                                    <td style="width: 1%"></td>
                                                    <td style="width: 1%">
                                                        <asp:Label ID="curUnderScore" runat="server" Width="100%" Text="_"></asp:Label>
                                                    </td>
                                                    <td style="width: 1%"></td>
                                                    <td style="width: 25%">
                                                        <label id="NamLabel" runat="server" Width="100%">Name</label>
                                                        <asp:HiddenField ID="NamHid" runat="server" Value="Name"></asp:HiddenField>
                                                    </td>
                                                    <td style="width: 1%"></td>
                                                    <td style="width: 5%">
                                                        <input type="hidden" runat="server" id="passedWF" name="passedWF" value="nothing" />
                                                        <input type="hidden" runat="server" id="passedWC" name="passedWC" value="nothing" />
                                                        <input type="hidden" runat="server" id="passedTp" name="passedTp" value="nothing" />
                                                    </td>
                                                    <td style="width: 1%"></td>
                                                    <td style="width: 13%">
                                                        <button id="CertCurrWI" OnClick="passFrameData('Cert'); return false;" style="width: 100%; height: 31px;" disabled="disabled" Title="Certified this uncertified file? &#013;This will break aprat the files based on the black break points and Certifiy them.">Certify file</button>
                                                    </td>
                                                    <td style="width: 1%"></td>
                                                    <td style="width: 13%">
                                                        <button id="ReNaCurrWI" OnClick="passFrameData('Name'); return false;" onkeyup="checkValidName(this); return false;" style="width: 100%; height: 31px;" disabled="disabled" Title="Rename this file with a new name? &#013;This will update the current certified file with a new name.">Rename File</button>
                                                    </td>
                                                    <td style="width: 1%"></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 12%">
                                                        <asp:Label ID="NewInputLabel" runat="server" Width="100%">Input Name: </asp:Label>
                                                    </td>
                                                    <td style="width: 1%"></td>
                                                    <td style="width: 12%">
                                                        <asp:TextBox ID="LotText" runat="server" Width="100%" placeholder="Lot ID Number" onKeyUp="checkNums(this); return false;" Height="24px" AutoPostBack="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 1%"></td>
                                                    <td style="width: 12%">
                                                        <asp:TextBox ID="RevText" runat="server" Width="100%" placeholder="Rev ID Letter(s)" onKeyUp="checkLets(this); return false;" Height="24px" AutoPostBack="false"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 1%"></td>
                                                    <td style="width: 1%">
                                                        <asp:Label ID="newUnderscore" runat="server" Width="100%">_</asp:Label>
                                                    </td>
                                                    <td style="width: 1%"></td>
                                                    <td style="width: 25%">
                                                        <asp:TextBox ID="NamText" runat="server" Width="100%" placeholder="File Name"  Height="24px" AutoPostBack="false" Text="UNCERTIFIED" disabled="true" title="File Name: File names can only include A-Z, 0-9, and '-'. &#013;No other special characters are allowed."></asp:TextBox>
                                                    </td>
                                                    <td style="width: 1%"></td>
                                                    <td style="width: 5%">
                                                        <asp:TextBox ID="currWIData" runat="server" style="visibility:hidden; width:4%;" textmode="MultiLine"></asp:TextBox>
                                                        <asp:Button ID="FileManipulation" runat="server" style="width:1%; visibility:hidden" />
                                                    </td>
                                                    <td style="width: 1%">
                                                        <asp:UpdateProgress id="UpdateProgress1" runat="server">
                                                            <progresstemplate>
                                                                <IMG src="../Color/Animated_LoadingBigger.gif" />
                                                            </progresstemplate>
                                                        </asp:UpdateProgress>
                                                    </td>
                                                    <td style="width: 13%">
                                                        <button id="SaveCurrWI" OnClick="passFrameData('Save'); return false;" style="width: 100%; height: 31px;" Title="Save this file? &#013;This will save the current full uncertified file. &#013;This will remove existing certified files.">Save file</button>
                                                    </td>
                                                    <td style="width: 1%"></td>
                                                    <td style="width: 13%">
                                                        <button id="RestCurrWI" OnClick="passFrameData('Rest'); return false;" style="width: 100%; height: 31px;" disabled="disabled" Title="Restore the file with last achieved Work Instuction? &#013;This will override the existing uncertified file with the archieved one and archive this one. &#013;This will remove existing certified files.">Restore last File</button>
                                                    </td>
                                                    <td style="width: 1%"></td>
                                                </tr>
                                            </table>
                                        </contenttemplate>
                                    </div>
                                </asp:Panel>
                            </td>
                            <td style="width: calc(100% - 985px)"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="DividerPanel1" runat="server" Height="10px" CssClass="ContentAutoScaler"></asp:Panel>
                <asp:Panel ID="MessagePanel" runat="server" Height="20px" CssClass="ContentAutoScaler" Style="text-align: center" BackColor="LightBlue">
                    <asp:Label ID="UpdateMessage" runat="server" Text="NOTE: Hover over editing options to learn more. When editing text - Please select it by highlighting it." ForeColor="Black" Font-Size="Large" Font-Bold="true"></asp:Label>
                </asp:Panel>
                <asp:Panel ID="DividerPanel2" runat="server" Height="10px" BackColor="lightblue" CssClass="ContentAutoScaler"></asp:Panel>
                <asp:Panel ID="ExcelMarkUps" runat="server" BackColor="lightblue" Style="width: 100%; height: 100%; padding-bottom: 10px;">
                    <table id="FrameContainer" class="ContentAutoScaler">
                        <tr>
                            <td style="width: calc(100% - 985px)"></td>
                            <td style="width: 985px; background-color: lightblue; border-color: lightblue;">
                                <asp:Panel ID="WorkIntructionEditWindow" runat="server" BackColor="lightblue">
                                    <asp:Panel ID="WorkInstructionHolder" runat="server" BackColor="lightblue">
                                        <div id="WC_0" class="tabcontent" style="display: block;">
                                            <iframe src="/WI/WI_HTML_Files/WI Template.aspx" id="WF_0" title="WI 0" style="width: 950px; height: 100%; border: none;" onload="document.getElementById('WF_0').contentWindow.enableControl(true)"></iframe>
                                        </div><!-- /WI/WI_HTML_Files/ -->

                                    </asp:Panel>
                                    <div id="tabHolder" runat="server" class="tab">
                                        <button type="button" id="WB_0" class="tablinks" value="WF_0" title="TEMPLATE" onclick="openWI(event, 'WC_0'); return false;">TEMPLATE</button>

                                    </div>
                                </asp:Panel>
                            </td>
                            <td style="width: calc(100% - 985px)"></td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <script data-require="jquery" data-semver="2.0.3" src="http://code.jquery.com/jquery-2.0.3.min.js"></script>
            <script data-require="angular.js@1.2.x" src="https://ajax.googleapis.com/ajax/libs/angularjs/1.2.16/angular.min.js" data-semver="1.2.16"></script>
            <script src="../scripts/WIScripts/ExcelEditorControls.js"></script>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

