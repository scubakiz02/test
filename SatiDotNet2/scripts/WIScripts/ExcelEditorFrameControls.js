//Global used to check if Iframe is allowed to be edited
var control;

//Used to enable/diable the editing in the Iframe
function enableControl(able) {
    control = able;

    if (control == null) {
        control = false;
    }

    document.getElementsByTagName("html")[0].contentEditable = control;
    document.getElementById("MMLabel").contentEditable = control;
    document.getElementById("IDTempExcel").contentEditable = control;
    document.getElementById("RevTempExcel").contentEditable = control;

    setTimeout(function () {            
        parent.startTab();
    }, 250);
};

//Passes Table deselect information
function passMoveOut() {
    if (control == true) {
        parent.moveOutMouse();
    }
};

//Passes IFrame data to parent for editing
function passClick(cell) {
    if (control == true) {
        parent.clickMouse(window.event, cell);
    }
};

//Passes IFrame data to parent for editing
function passMove(cell) {
    if (control == true) {
        parent.moveMouse(cell);
    }
};  

//Passes IFrame data to parent for editing
function passUnclick(cell) {
    if (control == true) {
        parent.unclickMouse(window.event, cell);
    }
};

//Passes Page Break Cell
function passPageBreak(cell) {
    if (control == true) {
        parent.removePageBreak(window.event, cell);
    }
}

//Passes the Image selection
function passSelectImage(imgDiv) {
    if (control == true) {
        parent.selectImage(window.event, imgDiv);
    }
}