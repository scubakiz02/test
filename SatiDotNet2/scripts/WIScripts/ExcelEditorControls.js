var indexRow;
var indexCol;

var lastSelectedRow;
var lastSelectedCol;

var fg_CW; 
var hl_CW;
var bg_CW;
var fb_CW;
var tb_CW;
var bb_CW;
var lb_CW;
var rb_CW;

var fs_CL;
var rh_CL;
var bsf_CL;
var bst_CL;
var bsl_CL;
var bsr_CL;
var bsb_CL;

var isDragging = false;
var ctrSelected = false;
var startCell = null;
var startClsN = "";

var numCheck = false;
var letCheck = false;

// ===============================================================================================
// Start up function
// ===============================================================================================
function startTab() {
    setTimeout(function () {
        var currWIList;
        var currWI;

        if (document.getElementById('WC_0').style.display == "block") {
            currWIList = document.getElementsByClassName("tablinks active");
            if (currWIList == null) {
                currWI = 'WB_0';
                document.getElementById(currWI).className = "tablinks active";
            } else if (currWIList.length == 0) {
                currWI = 'WB_0';
                document.getElementById(currWI).className = "tablinks active";
            } else {
                currWI = currWIList[0].id;
            }
                
            if (currWI != 'WB_0') {
                document.getElementById('WC_0').style.display = "none";
            }
        } else {
            currWIList = document.getElementsByClassName("tablinks active");
            currWI = currWIList[0].id;
        }

        document.getElementById("ctl00_ContentPlaceHolder1_LoadLotID").focus();
        setFrameContainer(currWI.replace("B", "C"));
        setFrameName(currWI.replace("B", "F"));
        resizeFrame("down", 0);
        initDoc();
        setListeners();
    }, 250);
}

function setFrameName(currFrame){
    var rogueWI = document.getElementById("RogueWI");
    rogueWI.value = currFrame;
}

function setFrameContainer(currContainer) {
    var rogueFC = document.getElementById("RogueFC");
    rogueFC.value = currContainer;
}

function resizeFrame(type, crd) {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frameCont = document.getElementById('RogueFC').value;
            
        var frame = document.getElementById(frameName);
        var frameC = document.getElementById(frameCont);
        var frameW = frame.contentWindow.document.body.scrollWidth;
        var frameH = frame.contentWindow.document.body.scrollHeight;

        if (type == "down") {
            if (frameW > 950) {
                frameC.style['height'] = (frameH + 18) + "px";
            } else {
                frameC.style['height'] = frameH + "px";
            }
        } else if (type == "up") {
            if (frameW > 950) {
                frameC.style['height'] = (frameH - (15 * crd)) + "px";
            } else {
                frameC.style['height'] = (frameH - (20 * crd)) + "px";
            }
        }
    } catch {
        return false;
    }
}

function setListeners() {
    fg_CW = document.getElementById('fontCW');
    fg_CW.addEventListener("input", colorWells, false);

    hl_CW = document.getElementById('highCW');
    hl_CW.addEventListener("input", colorWells, false);

    bg_CW = document.getElementById('backCW');
    bg_CW.addEventListener("input", colorWells, false);

    fb_CW = document.getElementById('fullCW');
    fb_CW.addEventListener("input", colorWells, false);

    tb_CW = document.getElementById('topCW');
    tb_CW.addEventListener("input", colorWells, false);

    bb_CW = document.getElementById('bottomCW');
    bb_CW.addEventListener("input", colorWells, false);

    lb_CW = document.getElementById('leftCW');
    lb_CW.addEventListener("input", colorWells, false);

    rb_CW = document.getElementById('rightCW');
    rb_CW.addEventListener("input", colorWells, false);

    fs_CL = document.getElementById('CustomFS');
    fs_CL.addEventListener('keypress', customTextInputs, false);

    rh_CL = document.getElementById('CustomRH');
    rh_CL.addEventListener('keypress', customTextInputs, false);

    bsf_CL = document.getElementById('CustomBF');
    bsf_CL.addEventListener('keypress', customTextInputs, false);

    bst_CL = document.getElementById('CustomBT');
    bst_CL.addEventListener('keypress', customTextInputs, false);

    bsl_CL = document.getElementById('CustomBL');
    bsl_CL.addEventListener('keypress', customTextInputs, false);

    bsr_CL = document.getElementById('CustomBR');
    bsr_CL.addEventListener('keypress', customTextInputs, false);

    bsb_CL = document.getElementById('CustomBB');
    bsb_CL.addEventListener('keypress', customTextInputs, false);
}


// ===============================================================================================
// Mouse Controls
// ===============================================================================================
document.onselectstart = function () {
    return false;
}

function clickMouse(inputEvent, currentCell) {
    if (inputEvent.button == 2) {
        return false;
    } else {
        if (inputEvent.ctrlKey) {
            isDragging = true;
            ctrSelected = true;

            setStartCell(currentCell);
            setEndCell(currentCell);
            clearAllImages();
        } else if (inputEvent.shiftKey) {
            isDragging = false;
            ctrSelected = false;

            setEndCell(currentCell);
            clearAllImages();
        } else {
            isDragging = true;
            setStartCell(currentCell);
            setEndCell(currentCell);
            clearAllImages();
        }
    }
}

function moveMouse(currentCell) {
    if (!isDragging) return;
    setEndCell(currentCell);
}

function unclickMouse(inputEvent, currentCell) {
    if (inputEvent.button == 2) {
        return false;
    } else if (inputEvent.ctrlKey) {
        isDragging = false;
        ctrSelected = false;
    } else if (inputEvent.shiftKey) {
        isDragging = false;
        ctrSelected = false;
    } else {
        isDragging = false;
        ctrSelected = false;
    }
}

function moveOutMouse() {
    if (isDragging) {
        isDragging = false;
        ctrSelected = false;
    }
}


// ===============================================================================================
// Cell selection and clearing
// ===============================================================================================
function setStartCell(el) {
    startCell = el;
     
    if ($(el).css("outline") == "rgb(0, 0, 0) none 0px") {
        startClsN = "green dashed 1px";
    } else {
        startClsN = "";
    }
}

function setEndCell(el) {
    clearAll();

    indexRow = [startCell.parentNode.rowIndex, el.parentNode.rowIndex];
    indexCol = [startCell.cellIndex, el.cellIndex];

    $(cellsBetween(startCell, el)).each(function () {
        var el = angular.element(this);
        if (ctrSelected == true) {
            el.css("outline", startClsN);
        } else {
            el.css("outline", "green dashed 1px");
        }
    });
}

function cellsBetween(start, end) {
    var frameName = document.getElementById('RogueWI').value;
    var frame = document.getElementById(frameName);
    var window = $(frame.contentWindow);
    var range = { minX: 0, minY: 0, maxX: 0, maxY: 0 };

    range.minX = window[0].Math.min($(start).offset().left, $(end).offset().left);
    range.minY = window[0].Math.min($(start).offset().top, $(end).offset().top);
    range.maxX = window[0].Math.max($(end).offset().left + $(end).width(), $(start).offset().left + $(start).width());
    range.maxY = window[0].Math.max($(end).offset().top + $(end).height(), $(start).offset().top + $(start).height());

    var initSelectedCells = rectangleSelect("td", range.minX, range.maxX, range.minY, range.maxY);

    for (var i = 0; i < initSelectedCells.length; i++) {
        if ($(initSelectedCells[i]).offset().left < range.minX) {
            range.minX = $(initSelectedCells[i]).offset().left;
        }
        if ($(initSelectedCells[i]).offset().left + $(initSelectedCells[i]).width() > range.maxX) {
            range.maxX = $(initSelectedCells[i]).offset().left + $(initSelectedCells[i]).width();
        }
        if ($(initSelectedCells[i]).offset().top < range.minY) {
            range.minY = $(initSelectedCells[i]).offset().top;
        }
        if ($(initSelectedCells[i]).offset().top + $(initSelectedCells[i]).height() > range.maxY) {
            range.maxY = $(initSelectedCells[i]).offset().top + $(initSelectedCells[i]).height();
        }
    }
    return rectangleSelect("td", range.minX, range.maxX, range.minY, range.maxY);
}

function rectangleSelect(selector, x1, x2, y1, y2) {
    var elements = [];
    var frameName = document.getElementById('RogueWI').value;
    var frame = document.getElementById(frameName);
    var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
    var lock = false;
    var tmpY2 = y2;
    var strX;
    var strY;
    
    $(excelTable).find(selector).each(function () {
        var $this = jQuery(this);
        var offset = $this.offset();
        var x = offset.left;
        var y = offset.top;
        //var w = $this.width();
        //var h = $this.height();

        if (x >= x1 && x <= x2 && y >= y1 && y <= y2) {
            // this element fits inside the selection rectangle
            elements.push($this.get(0));
        } else if (x >= x1 && x <= x2 && y >= y1 && y <= (y2 + 1)) {
            // This is only for checking if the cell height is too short
            elements.push($this.get(0));
            y2 = tmpY2;
        }
    });
    return elements;
}

function clearAll() {
    var frameName = document.getElementById('RogueWI').value;
    var frame = document.getElementById(frameName);
    var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
    var element = $(excelTable);

    if (ctrSelected == false) {
        element.find('td').css("outline", "");
    }
}


// ===============================================================================================
// Editor Function
// ===============================================================================================
function dropFunctions(optPrim, optSeco, input) {
    var tmpType = ["down" , 0];

    if (optPrim == "formatting") {
        tmpType = dropFunFormatting(optSeco, input);
    } else if (optPrim == "fontname") {
        formatDoc(optPrim, input);
    } else if (optPrim == "fontSize") {
        dropFunFontSize(optSeco, input);
    } else if (optPrim == "forecolor") {
        formatDoc(optPrim, input);
    } else if (optPrim == "borderSize") {
        dropFunBorderSize(optSeco, input);
    } else if (optPrim == "borderColor") {
        dropFunBorderColor(optSeco, input);
    } else if (optPrim == "coloring") {
        dropFunColoring(optSeco, input);
    } else if (optPrim == "rowHeight") {
        dropFunRowHeight(optSeco, input);
    } else {
        alert("Input was not recognized.");
    }

    resizeFrame(tmpType[0], tmpType[1]);
    return false;
}

function dropFunFormatting(opt, input) {
    var tmpType = ["down", 0];

    if (opt == "transferred") {
        insertExcelRow(opt, input);
    } else if (opt == "department") {
        insertExcelRow(opt, input);
    } else if (opt == "special") {
        insertExcelRow(opt, input);
    } else if (opt == "sPanel") {
        insertExcelRow(opt, input);
    } else if (opt == "pBreak") {
        insertExcelRow(opt, input);
    } else if (opt == "addRow") {
        insertExcelRow(opt, input);
    } else if (opt == "addCell") {
        insertExcelCell(input);
    } else if (opt == "delete") {
        if (input == "deleteRow") {
            tmpType[0] = "up";
            tmpType[1] = deleteExcelRow();
        } else {
            deleteExcelCell();
        }
    } else if (opt == "textWarp") {
        warpCellText(input);
    } else if (opt == "resetClassFormat") {
        resetClassFormat(input);
    }

    return tmpType;
}

function dropFunFontSize(opt, input) {
    if (opt == 'fontsize') {
        formatDoc(opt, input);
    } else if (opt == 'customFontSize') {
        customFontSize(input);
    }
}

function dropFunBorderSize(opt, input) {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
            var selectedCells = excelTable.querySelectorAll('td[style~=dashed]');
            var optColor = opt.replace('Width', 'Color');
            var optBord = opt.replace('Width', '');

            if (selectedCells.length > 0 && selectedCells != undefined) {
                for (var i = selectedCells.length; i--;) {
                    if (selectedCells[i].style.outline == "green dashed 1px") {
                        if (input == "") {
                            selectedCells[i].style[optBord] = "solid";
                            selectedCells[i].style[opt] = input + "px";
                            selectedCells[i].style[optColor] = input;
                        } else if (selectedCells[i].style[optColor] != "") {
                            selectedCells[i].style[opt] = input + "px";
                        } else {
                            selectedCells[i].style[optBord] = "solid";
                            selectedCells[i].style[opt] = input + "px";
                        }
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in DropFunBorderSize - Error:" + ex);
    }
}

function dropFunBorderColor(opt, input) {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
            var selectedCells = excelTable.querySelectorAll('td[style~=dashed]');
            var optWeight = opt.replace('Color', 'Width');

            if (selectedCells.length > 0 && selectedCells != undefined) {
                for (var i = selectedCells.length; i--;) {
                    if (selectedCells[i].style.outline == "green dashed 1px") {
                        if (input == "") {
                            selectedCells[i].style[opt] = "#f3f3f3";
                        } else if (selectedCells[i].style[optWeight] == "") {
                            selectedCells[i].style[optWeight] = "1px";
                            selectedCells[i].style[opt] = input;
                        } else {
                            selectedCells[i].style[opt] = input;
                        }                   
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in DropFunBorderColor - Error:" + ex);
    } 
}

function dropFunColoring(opt, input) {
    try {
        if (opt == "cellFill") {
            var frameName = document.getElementById('RogueWI').value;
            var frame = document.getElementById(frameName);

            if (frame != null) {
                var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
                var selectedCells = excelTable.querySelectorAll('td[style~=dashed]');

                if (selectedCells.length > 0 && selectedCells != undefined) {
                    for (var i = selectedCells.length; i--;) {
                        if (selectedCells[i].style.outline == "green dashed 1px") {
                            selectedCells[i].style["background"] = input;
                        }
                    }
                }
            }
        } else if (opt == "backcolor") {
            if (input == "") {
                clearHighlights();
            } else {
                formatDoc(opt, input);
            }
        }
    } catch (ex) {
        console.error("Error in DropFunColoring - Error:" + ex);
    }
}

function dropFunRowHeight(opt, input) {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
            var selectedCells = excelTable.querySelectorAll('td[style~=dashed]');

            if (selectedCells.length > 0 && selectedCells != undefined) {
                for (var i = selectedCells.length; i--;) {
                    if (selectedCells[i].style.outline == "green dashed 1px") {
                        selectedCells[i].parentNode.style[opt] = input + "px";
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in DropFunRowHeight - Error:" + ex);
    } 
}

function clearHighlights() {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
            var selectedCells = excelTable.querySelectorAll('td[style~=dashed]');

            if (selectedCells.length > 0 && selectedCells != undefined) {
                for (var i = selectedCells.length; i--;) {
                    if (selectedCells[i].style.outline == "green dashed 1px") {
                        if (selectedCells[i].children[0].outerHTML.includes("<span")) {
                            var tempInner = selectedCells[i].children[0].innerHTML;
                            selectedCells[i].innerHTML = tempInner;
                        } else {
                            var selectedTag = selectedCells[i].querySelectorAll('*[style]');
                            if (selectedTag.length > 0 && selectedTag != undefined) {
                                for (var j = selectedTag.length; j--;) {
                                    if (selectedTag[j].style.backgroundColor != "") {
                                        selectedTag[j].style.backgroundColor = "";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in ClearHighLights - Error:" + ex);
    } 
}

function customTextInputs(curEvent) {
    if (curEvent.keyCode == 13) {
        event.preventDefault();

        var inputID = curEvent.currentTarget.id;
        var inputVal = curEvent.target.value;

        if (inputVal <= 0) {
            inputVal = 1;
        }

        if (inputID == "CustomFS") {
            dropFunctions('fontSize', 'customFontSize', inputVal);
        } else if (inputID == "CustomBF") {
            dropFunctions('borderSize', 'borderWidth', inputVal);
        } else if (inputID == "CustomBT") {
            dropFunctions('borderSize', 'borderTopWidth', inputVal);
        } else if (inputID == "CustomBL") {
            dropFunctions('borderSize', 'borderLeftWidth', inputVal);
        } else if (inputID == "CustomBR") {
            dropFunctions('borderSize', 'borderRightWidth', inputVal);
        } else if (inputID == "CustomBB") {
            dropFunctions('borderSize', 'borderBottomWidth', inputVal);
        } else if (inputID == "CustomRH") {
            dropFunctions('rowHeight', 'height', inputVal);
        }
        resetCustomTextInputs();
    }
}

function resetCustomTextInputs() {
    document.getElementById('CustomFS').value = "";
    document.getElementById('CustomBF').value = "";
    document.getElementById('CustomBT').value = "";
    document.getElementById('CustomBL').value = "";
    document.getElementById('CustomBR').value = "";
    document.getElementById('CustomBB').value = "";
    document.getElementById('CustomRH').value = "";
}

function colorWells(well) {
    var currWell = well.currentTarget.id;
    var color = well.target.value;

    if (currWell == "fontCW") {
        formatDoc("forecolor", color);
    } else if (currWell == "fullCW") {
        dropFunBorderColor("borderColor", color);
    } else if (currWell == "topCW") {
        dropFunBorderColor("borderTopColor", color);
    } else if (currWell == "leftCW") {
        dropFunBorderColor("borderLeftColor", color);
    } else if (currWell == "rightCW") {
        dropFunBorderColor("borderRightColor", color);
    } else if (currWell == "bottomCW") {
        dropFunBorderColor("borderBottomColor", color);
    } else if (currWell == "backCW") {
        dropFunColoring("cellFill", color);
    } else if (currWell == "highCW") {
        formatDoc("backcolor", color);
    }
    resizeFrame("down", 0); 
}

function insertExcelRow(type, position) {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
            var selectedCells = excelTable.querySelectorAll('td[style~=dashed]');
            var addedRowNum = [];

            if (selectedCells.length > 0 && selectedCells != undefined) {
                for (var i = selectedCells.length; i--;) {
                    if (selectedCells[i].style.outline == "green dashed 1px") {
                        var curRow = selectedCells[i].parentNode.rowIndex;

                        if (addedRowNum.includes(curRow) == false) {
                            if (position == "above") {
                                excelTable.insertRow(curRow);
                                 excelTable.rows[curRow].className = "excelRow";
                                buildRowType(type, curRow);
                                curRow++;
                            } else {
                                excelTable.insertRow(curRow + 1);
                                excelTable.rows[curRow + 1].className = "excelRow";
                                buildRowType(type, curRow + 1);
                            }
                        }
                        addedRowNum.push(curRow);
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in InsertExcelRow - Error:" + ex);
    } 
}

function buildRowType(type, curRow) {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
            var addEngCell = false;

            for (var i = 0; i < 38; i++) {
                var inputText = "";

                if (addEngCell == true) {
                    excelTable.rows[curRow].insertCell(i);
                    buildCellHelper("", excelTable.rows[curRow].cells[i], addEngCell, inputText);
                    break;
                } else {
                    if (type == "transferred") {
                        inputText = "TRANSFERRED FROM ID: " + prompt("What is the ID that this low transferred from?");
                        excelTable.rows[curRow].insertCell(i);
                        excelTable.rows[curRow].style["height"] = 50 + "px";

                        buildCellHelper(type, excelTable.rows[curRow].cells[i], addEngCell, inputText);
                        addEngCell = true;
                    } else if (type == "department") {
                        inputText = prompt("What is the department name?");
                        excelTable.rows[curRow].insertCell(i);
                        excelTable.rows[curRow].style["height"] = 35 + "px";

                        buildCellHelper(type, excelTable.rows[curRow].cells[i], addEngCell, inputText);
                        addEngCell = true;
                    } else if (type == "special") {
                        excelTable.rows[curRow].insertCell(i);
                        excelTable.rows[curRow].style['height'] = 16.506 + "px";
                    
                        if (i == 0) {
                            inputText = "Special Instructions";
                            excelTable.rows[curRow].cells[i].colSpan = 6;
                            buildCellHelper(type, excelTable.rows[curRow].cells[i], addEngCell, inputText);
                            excelTable.rows[curRow].cells[i].style['borderLeft'] = "3px solid black";
                        } else if (i == 31) {
                            addEngCell = true;
                            buildCellHelper(type, excelTable.rows[curRow].cells[i], addEngCell, inputText);
                            excelTable.rows[curRow].cells[i].style['borderRight'] = "3px solid black";
                        } else {
                            buildCellHelper(type, excelTable.rows[curRow].cells[i], addEngCell, inputText);
                        }
                    } else if (type == "sPanel") {
                        excelTable.rows[curRow].insertCell(i);
                        excelTable.rows[curRow].style['height'] = 4.85 + "px";
                        if (i == 0) {
                            buildCellHelper(type, excelTable.rows[curRow].cells[i], addEngCell, inputText);
                            excelTable.rows[curRow].cells[i].style['borderLeft'] = "3px solid black";
                        } else if (i == 36) {
                            addEngCell = true;
                            buildCellHelper(type, excelTable.rows[curRow].cells[i], addEngCell, inputText);
                            excelTable.rows[curRow].cells[i].style['borderRight'] = "3px solid black";
                        } else {
                            buildCellHelper(type, excelTable.rows[curRow].cells[i], addEngCell, inputText);
                        }
                    } else if (type == "pBreak") {
                        inputText = " --- PAGE-BREAK --- --- THIS-WAS-LEFT-BLANK-ON-PURPOSE --- ";
                        excelTable.rows[curRow].insertCell(i);
                        excelTable.rows[curRow].setAttribute('contenteditable', 'false');
                        excelTable.rows[curRow].style['height'] = 10 + "px";
                        buildCellHelper(type, excelTable.rows[curRow].cells[i], addEngCell, inputText);
                        break;
                    } else if (type == "addRow") {
                        if (i == 36) {
                            excelTable.rows[curRow].insertCell(i);
                            buildCellHelper("", excelTable.rows[curRow].cells[i], addEngCell, inputText);
                            addEngCell = true;
                        } else {
                            excelTable.rows[curRow].insertCell(i);
                            buildCellHelper(type, excelTable.rows[curRow].cells[i], addEngCell, inputText);
                        }
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in BuildRowType - Error:" + ex);
    }
}

function insertExcelCell(position) {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
            var selectedCells = excelTable.querySelectorAll('td[style~=dashed]');

            if (selectedCells.length > 0 && selectedCells != undefined) {
                for (var i = selectedCells.length; i--;) {
                    if (selectedCells[i].style.outline == "green dashed 1px") {
                        var curRow = selectedCells[i].parentNode.rowIndex;
                        var curCel = selectedCells[i].cellIndex;

                        if (excelTable.rows[curRow].childElementCount < 38) {
                            if (position == "left") {
                                if (curCel == 36) {
                                    excelTable.rows[curRow].insertCell(curCel);
                                    buildCellHelper("", excelTable.rows[curRow].cells[curCel], false, "");
                                    buildCellHelper("", excelTable.rows[curRow].cells[curCel + 1], true, "");
                                } else {
                                    excelTable.rows[curRow].insertCell(curCel);
                                    buildCellHelper("", excelTable.rows[curRow].cells[curCel], false, "");
                                }
                            } else {
                                if (curCel == 36) {
                                    excelTable.rows[curRow].insertCell(curCel + 1);
                                    buildCellHelper("", excelTable.rows[curRow].cells[curCel + 1], true, "");
                                } else {
                                    excelTable.rows[curRow].insertCell(curCel + 1);
                                    buildCellHelper("", excelTable.rows[curRow].cells[curCel + 1], false, "");
                                }                        
                            }
                        }
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in InsertExcelCell - Error:" + ex);
    }
}

function buildCellHelper(type, curCell, engCell, inputText) {
    if (engCell == true) {
        curCell.className = 'engineeringCell';
    } else {
        curCell.className = 'excelCell';
    }

    if (type == "pBreak") {
        curCell.className = "pageBreak";
        curCell.style['background'] = "black";
        curCell.setAttribute("ondblclick", "passPageBreak(this);");
        curCell.setAttribute("title", " --- HOLD CONTROL AND DOUBLE CLICK TO REMOVE --- ");
        curCell.style['textAlign'] = "center";
        curCell.colSpan = 38;

        if (inputText != "") {
            curCell.innerHTML = inputText;
        }
    } else {
        curCell.setAttribute('onmousedown', 'passClick(this);');
        curCell.setAttribute('onmousemove', 'passMove(this);');
        curCell.setAttribute('onmouseup', 'passUnclick(this);');

        if (type == "transferred") {
            curCell.colSpan = 37;
            curCell.style['border'] = "3px solid black";
            curCell.style['textAlign'] = "center";
            curCell.style['color'] = "blue";
            curCell.style['fontFamily'] = "Arial";
            curCell.style['fontSize'] = "X-Large";
            curCell.style['fontWeight'] = "bold";

            if (inputText != "") {
                curCell.innerHTML = inputText.toUpperCase();
            }
        } else if (type == "department") {
            curCell.colSpan = 37;
            curCell.style['color'] = "blue";
            curCell.style['fontSize'] = 14 + "pt";
            curCell.style['fontWeight'] = "bold";
            curCell.style['fontStyle'] = "normal";
            curCell.style['fontFamily'] = "Arial";
            curCell.style['textAlign'] = "center";
            curCell.style['verticalAlign'] = "bottom";
            curCell.style["whiteSpace"] = "nowrap";
            curCell.style['borderTop'] = "3px solid black";
            curCell.style['borderLeft'] = "3px solid black";
            curCell.style['borderRight'] = "3px solid black";

            if (inputText != "") {
                curCell.innerHTML = inputText.toUpperCase();
            }
        } else if (type == "special" || type == "sPanel") {
            curCell.style['background'] = "#ffff99";
            curCell.style['color'] = "black";
            curCell.style['fontSize'] = 10 + "pt";
            curCell.style['fontWeight'] = "bold";
            curCell.style['fontStyle'] = "normal";
            curCell.style['fontFamily'] = "Arial";
            curCell.style['textAlign'] = "left";
            curCell.style['verticalAlign'] = "bottom";
            curCell.style["whiteSpace"] = "nowrap";
            curCell.style['border'] = "1px solid #ffff99";

            if (type == "special") {
                curCell.style['borderTop'] = "3px solid black";
            } else {
                curCell.style['borderTop'] = "1px solid black";
            }

            if (inputText != "") {
                curCell.innerHTML = inputText;
            }
        }
    }
}

function deleteExcelRow() {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);
        var countDeletedRows = 0;

        if (frame != null) {
            var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
            var selectedCells = excelTable.querySelectorAll('td[style~=dashed]');
            var addedRowNum = [];

            if (selectedCells.length > 0 && selectedCells != undefined) {
                for (var i = selectedCells.length; i--;) {
                    if (selectedCells[i].style.outline == "green dashed 1px") {
                        var curRow = selectedCells[i].parentNode.rowIndex;

                        if (addedRowNum.includes(curRow) == false) {
                            excelTable.deleteRow(curRow);
                            countDeletedRows++;
                        }
                        addedRowNum.push(curRow);
                    }
                }
            }
        }

        return countDeletedRows;
    } catch (ex) {
        console.error("Error in DeleteExcelRow - Error:" + ex);
        return 0;
    }
}

function deleteExcelCell() {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
            var selectedCells = excelTable.querySelectorAll('td[style~=dashed]');

            if (selectedCells.length > 0 && selectedCells != undefined) {
                for (var i = selectedCells.length; i--;) {
                    if (selectedCells[i].style.outline == "green dashed 1px") {
                        var curRow = selectedCells[i].parentNode.rowIndex;
                        var curCel = selectedCells[i].cellIndex;

                        if (excelTable.rows[curRow].childElementCount == 1) {
                            excelTable.deleteRow(curRow);
                        } else {
                            excelTable.rows[curRow].deleteCell(curCel);
                        }
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in DeleteExcelCell - Error:" + ex);
    }
}

function warpCellText(input) {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
            var selectedCells = excelTable.querySelectorAll('td[style~=dashed]');

            if (selectedCells.length > 0 && selectedCells != undefined) {
                for (var i = selectedCells.length; i--;) {
                    if (selectedCells[i].style.outline == "green dashed 1px") {
                        if (input == "on") {
                            selectedCells[i].style["whiteSpace"] = "normal";
                        } else {
                            selectedCells[i].style["whiteSpace"] = "nowrap";
                        }
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in WarpCellText - Error:" + ex);
    }
}

function mergeCells() {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);
        var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
        var currentRows = [indexRow[0], indexRow[indexRow.length - 1]];
        var currentCols = [indexCol[0], indexCol[indexCol.length - 1]];

        currentRows.sort(function (a, b) {
            return a - b;
        });
        currentCols.sort(function (a, b) {
            return a - b;
        });

        var rowLenSpan = 0;
        var colLenSpan = 0;

        var mergedText = "";
        var foundText  = false;

        var sr = 1;
        var sc = 0;

        if (currentRows[0] != currentRows[1] || currentCols[0] != currentCols[1]) {
            var selectedCells = excelTable.querySelectorAll('td[style~=dashed]');
            var mergedCRS = querySelectFromList("td[rowspan], td[colspan]", selectedCells);
            var selectedLength = selectedCells.length;

            if ((mergedCRS.length - 1) >= 0) {
                unmergeCells();
                selectedCells = excelTable.querySelectorAll('td[style~=dashed]');
                selectedLength = selectedCells.length;
            }

            if (selectedCells.length > 0 && selectedCells != undefined) {
                var lastTmpRow = currentRows[0];
                var lastTmpCol = currentCols[0];
                var colLock = false;

                for (var i = selectedCells.length; i--;) { 
                    if (selectedCells[i].style.outline == "green dashed 1px") {
                        var tmpRow = selectedCells[i].parentNode.rowIndex
                        var tmpCol = selectedCells[i].cellIndex

                        if (selectedCells[i].innerHTML != "") {
                            mergedText = selectedCells[i].innerHTML;

                            if (foundText == false) {
                                alert("Note: Only the most first cell's text will be saved into the merged cell.");
                                foundText = true;
                            }
                        }

                        if (lastTmpRow != tmpRow) {
                            rowLenSpan++;
                            lastTmpRow = tmpRow;
                            colLenSpan = 0;
                        }

                        if (lastTmpCol != tmpCol && colLock == false) {
                            colLenSpan++;
                            lastTmpCol = tmpCol;
                        }

                        if (tmpRow == currentRows[0] && tmpCol == currentCols[0] || selectedLength == 1) {
                            if (rowLenSpan != 0) {
                                excelTable.rows[tmpRow].cells[tmpCol].rowSpan = rowLenSpan;
                            }
                            if (colLenSpan != 0) {
                                excelTable.rows[tmpRow].cells[tmpCol].colSpan = colLenSpan;
                            }
                                
                            sr = tmpRow;
                            sc = tmpCol;
                            selectedCells[i].innerHTML = mergedText;
                        } else {
                            excelTable.rows[tmpRow].deleteCell(tmpCol);
                            selectedLength--;
                        }
                    }
                }
            }

            clearAll();
            indexRow = [];
            indexCol = [];
            indexRow[0] = currentRows[0];
            indexCol[0] = currentCols[0];
            lastSelectedRow = currentRows[0];
            lastSelectedCol = currentCols[0];

            excelTable.rows[sr].cells[sc].style.outline = 'green dashed 1px';
        } else {
            alert("Merging can only happen when you drag select cells.");
        }

        resizeFrame("down", 0);
    } catch (ex) {
        console.error("Error in MergeCells - Error:" + ex);
    }
}

function unmergeCells() {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
            var selectedCells = excelTable.querySelectorAll('td[style~=dashed]');

            if (selectedCells.length > 0 && selectedCells != undefined) {
                for (var i = selectedCells.length; i--;) {
                    if (selectedCells[i].style.outline == "green dashed 1px") {
                        var curRow = selectedCells[i].parentNode.rowIndex;
                        var curCel = selectedCells[i].cellIndex;
                        var curRS = selectedCells[i].rowSpan;
                        var curCS = selectedCells[i].colSpan;

                        var cRowSpan = [curRow, curRow + curRS];
                        var cColSpan = [curCel, curCel + curCS];

                        indexRow = [cRowSpan[0], cRowSpan[1] - 1];
                        indexCol = [cColSpan[0], cColSpan[1] - 1];

                        for (var j = cRowSpan[0]; j < cRowSpan[1]; j++) {
                            for (var k = cColSpan[0]; k < cColSpan[1]; k++) {
                                if (j == cRowSpan[0] && k == cColSpan[0]) {
                                    excelTable.rows[j].cells[k].rowSpan = 1;
                                    excelTable.rows[j].cells[k].colSpan = 1;
                                } else {
                                    excelTable.rows[j].insertCell(k);
                                    excelTable.rows[j].cells[k].className = "excelCell";
                                    excelTable.rows[j].cells[k].style.outline = 'green dashed 1px';
                                    excelTable.rows[j].cells[k].setAttribute('onmousedown', 'passClick(this);');
                                    excelTable.rows[j].cells[k].setAttribute('onmousemove', 'passMove(this);');
                                    excelTable.rows[j].cells[k].setAttribute('onmouseup', 'passUnclick(this);');
                                }
                            }
                        }
                    }
                }
            }
            resizeFrame("down", 0);
        }
    } catch (ex) {
        console.error("Error in UnmergeCells - Error:" + ex);
    }
}

function querySelectFromList(selector, elements) {
    return [].filter.call(elements, function (elements) {
        return elements.matches("td[rowspan], td[colspan]");
    })
}

function removePageBreak(inputEvent, pageBreaker) {
    try {
        if (inputEvent.button == 2) {
            return false;
        } else {
            if (inputEvent.ctrlKey) {
                var frameName = document.getElementById('RogueWI').value;
                var frame = document.getElementById(frameName);

                if (frame != null) {
                    var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
                    var curRow = pageBreaker.parentNode.rowIndex;

                    excelTable.deleteRow(curRow);
                }
            }
        }
        resizeFrame("up", 1);
    } catch (ex) {
        console.error("Error in RemovePageBreak - Error:" + ex);
    }
}

function customFontSize(input) {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
            var selectedCells = excelTable.querySelectorAll('td[style~=dashed]');

            if (selectedCells.length > 0 && selectedCells != undefined) {
                for (var i = selectedCells.length; i--;) {
                    if (selectedCells[i].style.outline == "green dashed 1px") {
                        var cellsKids
                        if (selectedCells[i].childNodes.length > 0) {
                            cellsKids = selectedCells[i].childNodes;
                            for (var j = cellsKids.length; j--;) {
                                if (cellsKids[j].nodeName == 'FONT') {
                                    cellsKids[j].size = "";
                                }
                            }
                        }

                        if (input <= 0) {
                            selectedCells[i].style.fontSize = 10 + "px";
                        } else {
                            selectedCells[i].style.fontSize = parseInt(input) + "px";
                        }
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in CustomFontSize - Error:" + ex);
    }
}

function resetClassFormat(input) {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
            var selectedCells = excelTable.querySelectorAll('td[style~=dashed]');

            if (selectedCells.length > 0 && selectedCells != undefined) {
                for (var i = selectedCells.length; i--;) {
                    if (selectedCells[i].style.outline == "green dashed 1px") {
                        selectedCells[i].className = input;
                    }
                }
            }

            cleanCell();
        }
    } catch (ex) {
        console.error("Error in ResetClassFormat - Error:" + ex);
    }
}


// ===============================================================================================
// Image and Textbox Controls
// ===============================================================================================
function setImage(e) {
    try {
        var img = document.getElementById('ctl00_ContentHolder_ClientImage');
        var hold = document.getElementById('ctl00_ContentHolder_serverImage');

        img.src = e.getAttribute('src');
        hold.value = e.getAttribute('src');

        return true;
        resizeFrame("down", 0);
    } catch (ex) {
        console.error("Error in SetImage - Error:" + ex);
    }
}

function openImagePopUp() {
    try {
        var popup = document.getElementById("myPopup");
        popup.classList.toggle("show");
        resizeFrame("down", 0);
    } catch (ex) {
        console.error("Error in OpenImagePopUp - Error:" + ex);
    }
}

function insertImage() {
    try {
        var frame = document.getElementById('WIImgUploader');
        var ImageDropDown = frame.contentWindow.document.getElementById('SelectImages');

        if (ImageDropDown != null) {
            var frameName = document.getElementById('RogueWI').value;
            var frame = document.getElementById(frameName);
            var selectedText;
            var selectedValue;

            for (var i = 0; i < ImageDropDown.children.length; i++) {
                if (ImageDropDown.children[i].selected == true) {
                    selectedText = ImageDropDown.children[i].text;
                    selectedValue = ImageDropDown.children[i].value;
                }
            }

            if (frame != null) {
                var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
                var selectedCells = excelTable.querySelectorAll('td[style~=dashed]');

                if (selectedCells.length > 0 && selectedCells != undefined) {
                    for (var i = selectedCells.length; i--;) {
                        if (selectedCells[i].style.outline == "green dashed 1px") {
                            var addDiv = document.createElement('div');

                            addDiv.className = "imgContainer";
                            addDiv.title = 'Shift click to delete';
                            addDiv.setAttribute('disable', 'true');
                            addDiv.setAttribute('onclick', 'passSelectImage(this); return false;');

                            if (selectedValue == "textarea") {
                                var addTxt = document.createElement('textarea');

                                addTxt.style.width = '-webkit-fill-available';
                                addTxt.style.height = '-webkit-fill-available';
                                addTxt.setAttribute('white-space', 'normal');

                                addDiv.appendChild(addTxt);
                            } else {
                                var addImg = document.createElement('img');

                                addImg.src = selectedValue;
                                addImg.alt = selectedText;
                                addImg.setAttribute('width', '95%');
                                addImg.setAttribute('height', '95%');

                                addDiv.appendChild(addImg);
                            }

                            selectedCells[i].appendChild(addDiv);
                        }
                    }
                }
            }
        }
    
        resizeFrame("down", 0);
    } catch (ex) {
        console.error("Error in InsertImage - Error:" + ex);
    }
}

function deleteImage(curImg) {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            clearAll();
            curImg.remove();
        }

        resizeFrame("down", 0);
    } catch (ex) {
        console.error("Error in DeleteImage - Error:" + ex);
    }
}

function imgUploadLotInfo(lotInfo, type) {
    try {
        setTimeout(function () {
            var frame = document.getElementById('WIImgUploader');
            var curLot = frame.contentWindow.document.getElementById('ImgUpTextBox');

            if (curLot != null) {
                if (type == 'sideHTML') {
                    if (lotInfo.value.includes('-')) {
                        curLot.value = lotInfo.value.substring(0, 4);
                    } else {
                        curLot.value = lotInfo.value;
                    }
                } else if (type == 'sideJS') {
                    curLot.value = lotInfo.substring(0, 4);
                }
                frame.contentWindow.document.getElementById('NinjaListUpdater').click();
            }
        }, 250);
    } catch (ex) {
        console.error("Error in imgUploadLotInfo - Error:" + ex);
    }
}

function selectImage(inputEvent, curDiv) {
    try {
        if (inputEvent.shiftKey) {
            deleteImage(curDiv);
        } else {
            if (inputEvent.ctrlKey) {
                if (curDiv.style.outline == 'red dashed 1px') {
                    curDiv.style.outline = '';
                } else {
                    curDiv.style.outline = 'red dashed 1px';
                }
            } else {
                clearAllImages();

                if (curDiv.style.outline == 'red dashed 1px') {
                    curDiv.style.outline = '';
                } else {
                    curDiv.style.outline = 'red dashed 1px';
                }
            }
        }
    } catch (ex) {
        console.error("Error in SelectImages - Error:" + ex);
    }
}

function clearAllImages() {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var selectedImage = frame.contentWindow.document.querySelectorAll('div[style~=dashed]');

            if (selectedImage.length > 0 && selectedImage != undefined) {
                for (var i = selectedImage.length; i--;) {
                    if (selectedImage[i].style.outline == "red dashed 1px") {
                        selectedImage[i].style.outline = "";
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in ClearAllImages - Error:" + ex);
    }
}

function moveImgHolderSingle(curDirBut) {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var selectedImage = frame.contentWindow.document.querySelectorAll('div[style~=dashed]');

            if (selectedImage.length > 0 && selectedImage != undefined) {
                for (var i = selectedImage.length; i--;) {
                    if (selectedImage[i].style.outline == "red dashed 1px") {
                        var curMT = selectedImage[i].style.marginTop;
                        var curML = selectedImage[i].style.marginLeft;
                        var inShift = document.getElementById('inputShift');

                        inShift.style.backgroundColor = "white";
                        if (inShift.value == "") {
                            inShift.value = 1;
                            inShift.style.backgroundColor = "#ffc5c5";
                        } else if (inShift.value < 1) {
                            inShift.value = 1;
                            inShift.style.backgroundColor = "#ffc5c5";
                        } else if (inShift.value > 100) {
                            inShift.value = 100;
                            inShift.style.backgroundColor = "#ffc5c5";
                        }

                        if (curMT == "") {
                            curMT = 0;
                        } else {
                            curMT = parseInt(curMT.substring(0, curMT.length - 2));
                        }

                        if (curML == "") {
                            curML = 0;
                        } else {
                            curML = parseInt(curML.substring(0, curML.length - 2));
                        }

                        var newIM;
                        if (curDirBut.title == "Move Up") {
                            newIM = curMT - parseInt(inShift.value);
                            selectedImage[i].style.marginTop = newIM + "pt";
                        } else if (curDirBut.title == "Move Down") {
                            newIM = curMT + parseInt(inShift.value);
                            selectedImage[i].style.marginTop = newIM + "pt";
                        } else if (curDirBut.title == "Move Left") {
                            newIM = curML - parseInt(inShift.value);
                            selectedImage[i].style.marginLeft = newIM + "pt";
                        } else if (curDirBut.title == "Move Right") {
                            newIM = curML + parseInt(inShift.value);
                            selectedImage[i].style.marginLeft = newIM + "pt";
                        }
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in MoveImgHolderSingle - Error:" + ex);
    }
}

function moveImgLevel(curLevelBut, lvlOpt) {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var selectedImage = frame.contentWindow.document.querySelectorAll('div[style~=dashed]');

            if (selectedImage.length > 0 && selectedImage != undefined) {
                for (var i = selectedImage.length; i--;) {
                    if (selectedImage[i].style.outline == "red dashed 1px") {
                        var curZLevel = selectedImage[i].style.zIndex;

                        if (curZLevel == "") {
                            curZLevel = 0;
                        } else {
                            curZLevel = parseInt(curZLevel);
                        }

                        if (lvlOpt == true) {
                            selectedImage[i].style.zIndex = curZLevel + 1;
                        } else if (lvlOpt == false) {
                            if (curZLevel == 0) {
                                selectedImage[i].style.zIndex = 0;
                            } else {
                                selectedImage[i].style.zIndex = curZLevel - 1;
                            }
                        }
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in MoveImgLevel - Error:" + ex);
    }
}

function dropTextFunctions(primOption, secoOption) {
    if (primOption == "fontStyle") {
        textBoxFontStyle(secoOption);
    } else if (primOption == "fontSize") {
        textBoxFontSize(secoOption);
    } else if (primOption == "fontColor") {
        textBoxFontColor(secoOption);
    } else if (primOption == "backColor") {
        textBoxBackColor(secoOption);
    } else if (primOption == "fontWeight") {
        textBoxFontBold(secoOption);
    } else {
        alert("Input was not recognized.");
    }
}

function textBoxFontStyle(opt) {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var selectTextbox = frame.contentWindow.document.querySelectorAll('div[style~=dashed]');

            if (selectTextbox.length > 0 && selectTextbox != undefined) {
                for (var i = selectTextbox.length; i--;) {
                    if (selectTextbox[i].style.outline == "red dashed 1px") {
                        for (var j = 0; j < selectTextbox[i].children.length; j++) {
                            if (selectTextbox[i].children[j].nodeName == "TEXTAREA") {
                                selectTextbox[i].children[j].style.fontFamily = opt;
                            }
                        }
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in TextBoxFontStyle - Error:" + ex);
    }
}

function textBoxFontSize(opt) {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var selectTextbox = frame.contentWindow.document.querySelectorAll('div[style~=dashed]');

            if (selectTextbox.length > 0 && selectTextbox != undefined) {
                for (var i = selectTextbox.length; i--;) {
                    if (selectTextbox[i].style.outline == "red dashed 1px") {
                        for (var j = 0; j < selectTextbox[i].children.length; j++) {
                            if (selectTextbox[i].children[j].nodeName == "TEXTAREA") {
                                selectTextbox[i].children[j].style.fontSize = opt;
                            }
                        }
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in TextBoxFontSize - Error:" + ex);
    }
}

function textBoxFontColor(opt) {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var selectTextbox = frame.contentWindow.document.querySelectorAll('div[style~=dashed]');

            if (selectTextbox.length > 0 && selectTextbox != undefined) {
                for (var i = selectTextbox.length; i--;) {
                    if (selectTextbox[i].style.outline == "red dashed 1px") {
                        for (var j = 0; j < selectTextbox[i].children.length; j++) {
                            if (selectTextbox[i].children[j].nodeName == "TEXTAREA") {
                                selectTextbox[i].children[j].style.color = opt;
                            }
                        }
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in textBoxFontColor - Error:" + ex);
    }
}

function textBoxBackColor(opt) {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var selectTextbox = frame.contentWindow.document.querySelectorAll('div[style~=dashed]');

            if (selectTextbox.length > 0 && selectTextbox != undefined) {
                for (var i = selectTextbox.length; i--;) {
                    if (selectTextbox[i].style.outline == "red dashed 1px") {
                        for (var j = 0; j < selectTextbox[i].children.length; j++) {
                            if (selectTextbox[i].children[j].nodeName == "TEXTAREA") {
                                selectTextbox[i].children[j].style.background = opt;
                            }
                        }
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in TextBoxBackColor - Error:" + ex);
    }
}

function textBoxFontBold(opt) {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var selectTextbox = frame.contentWindow.document.querySelectorAll('div[style~=dashed]');

            if (selectTextbox.length > 0 && selectTextbox != undefined) {
                for (var i = selectTextbox.length; i--;) {
                    if (selectTextbox[i].style.outline == "red dashed 1px") {
                        for (var j = 0; j < selectTextbox[i].children.length; j++) {
                            if (selectTextbox[i].children[j].nodeName == "TEXTAREA") {
                                selectTextbox[i].children[j].style.fontWeight = opt;
                            }
                        }
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in textBoxFontBold - Error:" + ex);
    }
}


// ===============================================================================================
// VB Interactions
// ===============================================================================================
function resetAfterPost() {
    resizeFrame("down", 0);
    imgUploadLotInfo(document.getElementsByClassName('tablinks active')[0].title, 'sideJS');
}

function loadComfirmation(inputButton) {
    try {
        var currWI = document.getElementsByClassName("tablinks active")[0].id;
        clearAll();

        if (inputButton == 'LOAD') {
            var loLot = document.getElementById("ctl00_ContentPlaceHolder1_LoadLotID");

            if (loLot.value != "") {
                var loadComfirm = confirm("Are you sure you want to load the Work Instruction?\n"
                                        + "Anything changed on this page will be removed after "
                                        + "we load the work instructions");

                if (loadComfirm == true) {
                    document.getElementById(currWI).className = "tablinks";
                    document.getElementById(currWI.replace("B", "C")).style.display = "none";

                    document.getElementById("ctl00_ContentPlaceHolder1_HiddenLoadWI").click();
                } else {
                    return false;
                }
            } else {
                loLot.placeholder = '***  THIS CANNOT BE BLANK  ***';
                loLot.style["backgroundColor"] = "#ffc5c5";
            }
        } else if (inputButton == 'UPLOAD') {
            var upLot = document.getElementById("ctl00_ContentPlaceHolder1_UpLoadLotID");
            var upRev = document.getElementById("ctl00_ContentPlaceHolder1_UpLoadRevID");

            if (upLot.value != "" && upRev.value != "") {
                var loadComfirm = confirm("Are you sure you want to upload a new Work Instruction?\n"
                                        + "Anything changed on this page will be removed after "
                                        + "we upload a new work instructions");

                if (loadComfirm == true) {
                    document.getElementById(currWI).className = "tablinks";
                    document.getElementById(currWI.replace("B", "C")).style.display = "none";

                    document.getElementById("ctl00_ContentPlaceHolder1_HiddenUploadWI").click();
                } else {
                    return false;
                }
            } else {
                upLot.placeholder = '***  THIS CANNOT BE BLANK  ***';
                upLot.style["backgroundColor"] = "#ffc5c5";
                upRev.placeholder = '***  THIS CANNOT BE BLANK  ***';
                upRev.style["backgroundColor"] = "#ffc5c5";
            }
        } else {
            return false;
        } 
    } catch (ex) {
        console.error("Error in LoadComfirmation - Error:" + ex);
    }
}

function adjustButtonAfterPost() {
    actTab = document.getElementsByClassName("tablinks active");
    setActiveFileButtons(actTab[0].innerHTML);
}

function checkNums(input) {
    try {
        var tmpInput
        const isValidStart = v => /^\d{0,9}$/.test(v);

        if (input.value.includes("-")) {
            tmpInput = input.value.substring(0, 4)
        } else {
            tmpInput = input.value
        }
    
        if (isValidStart(tmpInput) == false) {
            numCheck = false;
            input.style.backgroundColor = '#ffc5c5';
        } else {
            numCheck = true;
            input.style.backgroundColor = 'white';
        }
    } catch (ex) {
        console.error("Error in CheckNums - Error:" + ex);
    }
}

function getNumChecks() {
    return numCheck;
}

function checkLets(input) {
    try {
        const isValidStart = v => /[^a-zA-Z]/g.test(v);
        if (isValidStart(input.value) == true) {
            letCheck = false;
            input.style.backgroundColor = '#ffc5c5';
        } else {
            letCheck = true;
            input.style.backgroundColor = 'white';
        }
    } catch (ex) {
        console.error("Error in CheckLets - Error:" + ex);
    }

}

// ===============================================================================================
// These function control the Tabs
// ===============================================================================================
function openWI(evt, wiHolder) {
    if (validateMode() == true) {
        var i, tabcontent, tablinks;

        tabcontent = document.getElementsByClassName("tabcontent");
        for (i = 0; i < tabcontent.length; i++) {
            if (tabcontent[i].style.display == "block") {
                clearAll();
            }

            tabcontent[i].style.display = "none";
        }
        tablinks = document.getElementsByClassName("tablinks");
        for (i = 0; i < tablinks.length; i++) {
            tablinks[i].className = tablinks[i].className.replace(" active", "");
        }
        document.getElementById(wiHolder).style.display = "block";
        evt.currentTarget.className += " active";

        var frameName = wiHolder;
        frameName = frameName.replace('C', 'F');

        setFrameName(frameName);
        setFrameContainer(wiHolder);
        resizeFrame("down", 0);
        setCurrentFileName(evt.currentTarget.title);
        setActiveFileButtons(evt.currentTarget.innerHTML);
        imgUploadLotInfo(evt.currentTarget.title, 'sideJS');
        initDoc();
    }
}

function openEditor(evt, editorName) {
    if (validateMode() == true) {
        var i, tabcontent, tablinks;

        tabcontent = document.getElementsByClassName("editorContent");
        for (i = 0; i < tabcontent.length; i++) {
            tabcontent[i].style.display = "none";
        }

        tablinks = document.getElementsByClassName("editorTab");
        for (i = 0; i < tablinks.length; i++) {
            tablinks[i].className = tablinks[i].className.replace(" active", "");
        }

        document.getElementById(editorName).style.display = "block";
        evt.currentTarget.className += " active";
    }
}

window.onscroll = function () { sticky() }
function sticky() {
    var editorBar = document.getElementById("ctl00_ContentPlaceHolder1_InputPanels");
    var divideBar = document.getElementById("ctl00_ContentPlaceHolder1_DividerPanel1");
    var CPanelBar = document.getElementById("ctl00_ContentPlaceHolder1_MainContentPanel");
    var stick = CPanelBar.offsetTop;

    if (window.pageYOffset >= stick) {
        editorBar.classList.add("sticky");
        divideBar.style.height = 160 + "px";
    } else {
        editorBar.classList.remove("sticky");
        divideBar.style.height = 10 + "px";
    }
}

function setCurrentFileName(tabName) {
    var lotName;
    var revName;
    var filName;

    if (tabName == "TEMPLATE") {
        lotName = "Lot ID";
        revName = "Rev ID";
        filName = "TEMPLATE";
    } else {
        lotName = tabName.substring(0, 4);
        revName = tabName.substring(4, tabName.indexOf("_"));
        filName = tabName.substring(tabName.indexOf("_") + 1);
    }

    document.getElementById("ctl00_ContentPlaceHolder1_LotLabel").innerHTML = lotName.toUpperCase();
    document.getElementById("ctl00_ContentPlaceHolder1_RevLabel").innerHTML = revName.toUpperCase();
    document.getElementById("ctl00_ContentPlaceHolder1_NamLabel").innerHTML = filName.toUpperCase();
    document.getElementById("ctl00_ContentPlaceHolder1_LotHid").value = lotName.toUpperCase();
    document.getElementById("ctl00_ContentPlaceHolder1_RevHid").value = revName.toUpperCase();
    document.getElementById("ctl00_ContentPlaceHolder1_NamHid").value = filName.toUpperCase();
}

function setActiveFileButtons(tabName) {
    var CertB = document.getElementById("CertCurrWI");
    var ReNaB = document.getElementById("ReNaCurrWI");
    var SaveB = document.getElementById("SaveCurrWI");
    var RestB = document.getElementById("RestCurrWI");

    if (tabName.includes("UNCERTIFIED")) {
        enableSaveCertButtons();
        ReNaB.disabled = true;

        lotRevInputToggles(false);
        document.getElementById('ctl00_ContentPlaceHolder1_NamText').value = "UNCERTIFIED";
        document.getElementById('ctl00_ContentPlaceHolder1_NamText').disabled = true;
    } else if (tabName.includes("TEMPLATE")) {
        enableSaveCertButtons();
        CertB.disabled = true;
        ReNaB.disabled = true;
        RestB.disabled = true;

        lotRevInputToggles(false);
        document.getElementById('ctl00_ContentPlaceHolder1_NamText').value = "UNCERTIFIED";
        document.getElementById('ctl00_ContentPlaceHolder1_NamText').disabled = true;
    } else {
        enableSaveCertButtons();
        CertB.disabled = true;
        SaveB.disabled = true;
        RestB.disabled = true;

        lotRevInputToggles(true);
        document.getElementById('ctl00_ContentPlaceHolder1_NamText').value = "";
        document.getElementById('ctl00_ContentPlaceHolder1_NamText').disabled = false;
    }
}

function lotRevInputToggles(editable) {
    document.getElementById('ctl00_ContentPlaceHolder1_LotText').value
        = document.getElementById('ctl00_ContentPlaceHolder1_LotHid').value;
    document.getElementById('ctl00_ContentPlaceHolder1_LotText').disabled = editable;

    document.getElementById('ctl00_ContentPlaceHolder1_RevText').value
        = document.getElementById('ctl00_ContentPlaceHolder1_RevHid').value;
    document.getElementById('ctl00_ContentPlaceHolder1_RevText').disabled = editable;
}

function enableSaveCertButtons() {
    document.getElementById("CertCurrWI").disabled = false;
    document.getElementById("ReNaCurrWI").disabled = false;
    document.getElementById("SaveCurrWI").disabled = false;
    document.getElementById("RestCurrWI").disabled = false;
}


// ===============================================================================================
// HTML JSON DATA BUILDER
// ===============================================================================================
function passFrameData(type) {
    var id_Text = document.getElementById("ctl00_ContentPlaceHolder1_LotText");
    var revText = document.getElementById("ctl00_ContentPlaceHolder1_RevText");
    var namText = document.getElementById("ctl00_ContentPlaceHolder1_NamText");
    var curRevL = document.getElementById("ctl00_ContentPlaceHolder1_RevLabel");
    var activeTab = document.getElementsByClassName("tablinks active");
    var tabName = activeTab[0].innerHTML;
    var WF = activeTab[0].value;
    var WC = WF.replace("F", "C");

    checkNums(id_Text);
    checkLets(revText);
    clearAll();

    if (id_Text.value != "" && revText.value != "") {
        if (letCheck == true && numCheck == true) {
            var comText;

            if (type == "Cert") {
                comText = "Certified this file?\nThis will certify the uncertified file and break it apart for the operators to use.";
            } else if (type == "Name") {
                if (namText.value != "") {
                    if (checkValidName(namText) == true) {
                        comText = "Rename this Certified file?\nThis will change the file name, this function is used if the program did not find the file name during the certify process";
                    } else {
                        namText.placeholder = "* INVALID NAME (a-z, 0-9, '-') *";
                        namText.style["backgroundColor"] = '#ffc5c5';
                        return false;
                    }
                } else {
                    namText.value = "";
                    namText.placeholder = '* NEEDS A NAME *';
                    namText.style["backgroundColor"] = '#ffc5c5';
                    return false;
                }
            } else if (type == "Save") {
                if (curRevL.value == revText.value) {
                    revText.value = "";
                    revText.placeholder = '* NEEDS NEW REV *';
                    revText.style["backgroundColor"] = '#ffc5c5';
                } else {
                    if (checkNextRevLetter(curRevL.innerHTML, revText.value)) {
                        comText = "Save this file?\nThis will save the current uncertify file.\nThis requires a new Revision letter";
                    } else if (tabName == "TEMPLATE") {
                        comText = "Save this template file?\nThis will be saved as a uncertify file.\nIt will be named: " + id_Text.value + revText.value + "_UNCERTIFIED";
                    } else {
                        revText.value = "";
                        revText.placeholder = '* NEEDS NEW REV *';
                        revText.style["backgroundColor"] = '#ffc5c5';
                        return false;
                    }
                }
            } else if (type == "Rest") {
                comText = "Restore the file with the last Revision letter file?\nThis will override the existing uncertified file with the archieved one.";
            }

            if (confirm(comText) == true) {
                document.getElementById("ctl00_ContentPlaceHolder1_passedWF").value = WF;
                document.getElementById("ctl00_ContentPlaceHolder1_passedWC").value = WC;
                document.getElementById("ctl00_ContentPlaceHolder1_passedTp").value = type;

                if (JSONStringBuilder(document.getElementById(WF).contentDocument, type) == true) {
                    var fileManButton = document.getElementById("ctl00_ContentPlaceHolder1_FileManipulation");
                    fileManButton.click()
                }
            }
        } else {
            alert("One of the inputs have an invalid input.\n\nLot Id cannot contain letters.\nRev Id cannot contain numbers.");
        }
    } else {
        document.getElementById("ctl00_ContentPlaceHolder1_passedWF").value = WF;
        document.getElementById("ctl00_ContentPlaceHolder1_passedWC").value = WC;

        id_Text.placeholder = '* INPUT REQUIRED *';
        id_Text.style["backgroundColor"] = "#ffc5c5";
        revText.placeholder = '* INPUT REQUIRED *';
        revText.style["backgroundColor"] = "#ffc5c5";
    }

    setFrameName(WF);
    setFrameContainer(WC);
    resizeFrame("down", 0);
}

function checkValidName(input) {
    const isValidName = v => /[`!@#$%^*()_+\=\[\]{};':"\\|,.<>\/?~]/g.test(v);

    if (isValidName(input.value) == true) {
        input.placeholder = "* INVALID NAME (a-z, 0-9, '-') *";
        input.style["backgroundColor"] = "#ffc5c5";
        return false;
    } else {
        input.placeholder = "File Name";
        input.style["backgroundColor"] = "white";
        return true;
    }
}

function checkNextRevLetter(curRev, inpRev) {
    if (curRev == 'Rev ID') {
        return false;
    }

    if (curRev == inpRev) {
        return false;
    } else {
        var cArray = [];
        var nArray = [];
        var tArray = [];

        for (var i = 0; i < curRev.length; i++) { cArray.push(curRev.toUpperCase().charCodeAt(i)); }
        for (var i = 0; i < inpRev.length; i++) { nArray.push(inpRev.toUpperCase().charCodeAt(i)); }
        for (var i = 0; i < curRev.length; i++) { tArray.push(curRev.toUpperCase().charCodeAt(i)); }

        for (var i = tArray.length; i--;) {
            if (i == 0 && tArray[i] == 90) {
                var temp = [];
                temp.push(65);
                tArray[i] = 65;
                for (var j = 0; j < tArray.length; j++) {
                    temp.push(tArray[j]);
                }
                tArray = [];
                tArray = temp;
            } else {
                if (i == 0 && tArray[i] == 91) {
                    var temp = [];
                    temp.push(65);
                    tArray[i] = 65;
                    for (var j = 0; j < tArray.length; j++) {
                        temp.push(tArray[j]);
                    }
                    tArray = temp;
                }

                if (i == tArray.length - 1) {
                    tArray[i] += 1;
                }

                if (tArray[i] == 91 && i != 0) {
                    tArray[i] = 65;
                    tArray[i - 1] += 1;
                }
            }
        }

        var compLen = 0;
        var compAry = [];
        var compRet = false;

        if (tArray.length > nArray.length) {
            compLen = tArray.length;
        } else {
            compLen = nArray.length;
        }
        for (var i = 0; i < compLen; i++) {
            if (nArray[i] == tArray[i]) {
                compAry.push(true);
            } else {
                compAry.push(false);
            }
        }

        for (var i = 0; i < compAry.length; i++) {
            if (compAry[i] == true) {
                compRet = true;
            } else {
                compRet = false;
            }
        }

        if (compRet == true) {
            return true;
        } else {
            return false;
        }
    }
}

function JSONStringBuilder(frameData, certCheck) {
    let frameEles = frameData.children[0].outerHTML;
    var encodedFrame;

    if (certCheck == "Cert") {
        if (frameEles.includes(" --- PAGE-BREAK --- --- THIS-WAS-LEFT-BLANK-ON-PURPOSE --- ")) {
            encodedFrame = escape(frameEles);
            document.getElementById("ctl00_ContentPlaceHolder1_currWIData").value = "";
            document.getElementById("ctl00_ContentPlaceHolder1_currWIData").value = encodedFrame;
            return true;
        } else {
            alert("The current uncertified document does not contain a page break.\n\n"
                + "Please add page breaks between the areas, where you want the document "
                + "to be broken up into tabs. Try to break the document into departments.\n\n"
                + "You can find the page break option in the 'Editor Menu', under the 'Formatting' " 
                + "drop-down menu, named 'Page Break'.");
            return false;
        }    
    } else {
        encodedFrame = escape(frameEles);
        document.getElementById("ctl00_ContentPlaceHolder1_currWIData").value = "";
        document.getElementById("ctl00_ContentPlaceHolder1_currWIData").value = encodedFrame;
        return true;
    }
}


// ===============================================================================================
// These functions control the bottom row button for the edit. 
// ===============================================================================================
var oDoc, sDefTxt;

function initDoc() {
    var curFrame = document.getElementById("RogueWI");
    var frame = document.getElementById(curFrame.value);
    oDoc = frame.contentDocument;
    sDefTxt = oDoc.childNodes[0].outerHTML;

    if (document.getElementById('switchBox').checked)
    {
        setDocMode(true);
    }
}

function formatDoc(sCmd, sValue) {
    if (validateMode()) {
        var curFrame = document.getElementById("RogueWI");
        var frame = document.getElementById(curFrame.value);
        frame.contentWindow.document.execCommand(sCmd, false, sValue);
    }
}

function validateMode() {
    var switchMode = document.getElementById('switchBox');
    if (!switchMode.checked) { return true; }
    alert("Uncheck the source checkbox before changing tabs.\nIt looks like this: \"</>\".");
    return false;
}

function setDocMode(bToSource) {
    var frameName = document.getElementById("RogueWI");
    var curCont = document.getElementById(frameName.value.replace("F", "C"));
    var curFrame = document.getElementById(frameName.value);
    var oContent;

    if (bToSource) {
        oContent = document.createTextNode(oDoc.childNodes[0].outerHTML);
        curFrame.style["display"] = "none";
        curFrame.contentEditable = false;

        var srcEdit = document.createElement("textarea");
        srcEdit.id = "sourceText";
        srcEdit.maxLength = oContent.length * 1.5;
        srcEdit.appendChild(oContent);
        srcEdit.style["overflow-x"] = "hidden";
        srcEdit.style["overflow-y"] = "scroll";
        srcEdit.style["width"] = "calc(100% - 7px)";
        srcEdit.style["height"] = "500px";
        srcEdit.style["whiteSpace"] = "nowarp";
        curCont.appendChild(srcEdit);
        document.execCommand("defaultParagraphSeparator", false, "div");
        document.getElementById('switchBox').title = "View/Edit Render Work Instructions";
        resizeScrFrame();
    } else {
        if (document.all) {
            oDoc.innerHTML = oDoc.innerText;
        } else {
            oContent = document.getElementById('sourceText');

            curFrame.contentWindow.document.open();
            curFrame.contentWindow.document.write("");
            curFrame.contentWindow.document.write(oContent.value);
            curFrame.contentWindow.document.close();
            oContent.remove();

            curFrame.style["display"] = "block";
            curFrame.contentEditable = true
            document.getElementById('switchBox').title = "View/Edit Source Code";
        }
        resizeFrame("down", 0);
    }
}

function cleanDoc() {
    if (validateMode() && confirm('Are you sure you want to clean the document?'
        + '\nThis will reset the document to the last load state.'
        + '\n\nLoad states included changing the bottom tabs as well as changing the view mode between render and source.')) {
        var frameName = document.getElementById("RogueWI");
        var curFrame = document.getElementById(frameName.value);

        curFrame.contentWindow.document.open();
        curFrame.contentWindow.document.write("");
        curFrame.contentWindow.document.write(sDefTxt);
        curFrame.contentWindow.document.close();
    }
}

function cleanCell() {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frame = document.getElementById(frameName);

        if (frame != null) {
            var excelTable = frame.contentWindow.document.getElementById('MainExcelTable');
            var selectedCells = excelTable.querySelectorAll('td[style~=dashed]');

            if (selectedCells.length > 0 && selectedCells != undefined) {
                for (var i = selectedCells.length; i--;) {
                    if (selectedCells[i].style.outline == "green dashed 1px") {
                        var cellsText = selectedCells[i].innerText;
                        var cellsHTML = selectedCells[i].outerHTML;
                        var resetStyle;

                        if (cellsHTML.includes("style=")) {
                            var temp = cellsHTML.substring(cellsHTML.indexOf('style="') + 7);
                            temp = temp.substring(0, temp.indexOf('"'));
                            resetStyle = temp;
                        } else if (cellsHTML.includes("style =")) {
                            var temp = cellsHTML.substring(cellsHTML.indexOf('style ="') + 8);
                            temp = temp.substring(0, temp.indexOf('"'));
                            resetStyle = temp;
                        }
                        selectedCells[i].innerHTML = cellsText;
                        selectedCells[i].outerHTML = selectedCells[i].outerHTML.replace(resetStyle, "outline: green dashed 1px;");
                    }
                }
            }
        }
    } catch (ex) {
        console.error("Error in CleanCell - Error:" + ex);
    }
}

function printDoc() {
    if (!validateMode()) { return; }
    var oPrntWin = window.open("", "_blank", "width=450,height=470,left=400,top=100,menubar=yes,toolbar=no,location=no,scrollbars=yes");
    var prntDoc = oDoc.childNodes[0].outerHTML;

    prntDoc = prntDoc.replace('<body>', '<body onload=\"print();\">');
    oPrntWin.document.open();
    oPrntWin.document.write(prntDoc);
    oPrntWin.document.close();
}

function resizeScrFrame() {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frameCont = document.getElementById('RogueFC').value;

        var frame = document.getElementById(frameName);
        var frameContainer = document.getElementById(frameCont);
        var frameWidth = frame.contentWindow.document.body.scrollWidth;

        if (frameWidth > 950) {
            frameContainer.style['height'] = "";
            frameContainer.style['height'] = 518 + "px";
        } else {
            frameContainer.style['height'] = "";
            frameContainer.style['height'] = 510 + "px";
        }
    } catch {
        return false;
    }
}