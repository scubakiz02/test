//Sets up tab setting for switching between them later
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

        setFrameContainer(currWI.replace("B", "C"));
        setFrameName(currWI.replace("B", "F"));
        resizeFrame(currWI.replace("B", "F"), currWI.replace("B", "C"));
    }, 250);
};

//Saves the frame data
function setFrameName(currFrame){
    var rogueWI = document.getElementById("RogueWI");
    rogueWI.value = currFrame;
};

//Saves the frame containers data
function setFrameContainer(currContainer) {
    var rogueFC = document.getElementById("RogueFC");
    rogueFC.value = currContainer;
};

//Resets the size of the frame and container to make it one large document
function resizeFrame() {
    try {
        var frameName = document.getElementById('RogueWI').value;
        var frameCont = document.getElementById('RogueFC').value;
            
        var frame = document.getElementById(frameName);
        var frameContainer = document.getElementById(frameCont);
        var frameWidth = frame.contentWindow.document.body.scrollWidth;

        if (frameWidth > 950) {
            frameContainer.style['height'] = "";
            frameContainer.style['height'] = frame.contentWindow.document.body.scrollHeight + 18 + "px";
        } else {
            frameContainer.style['height'] = "";
            frameContainer.style['height'] = frame.contentWindow.document.body.scrollHeight + 10 + "px";
        }
    } catch {
        return false;
    }
};

//Selection start handle
document.onselectstart = function () {
    return false;
};

//JS function that will be called from VB to reset the frame.
function resetAfterPost(redepart) {
    resizeFrame();
};

//Controls the tab switching
function openWI(evt, wiHolder) {
    var i, tabcontent, tablinks;

    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
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
    resizeFrame(frameName, wiHolder);
};

// Causes a post back while you type - once its over four it will post to update drop down list
function searchLotOnKeyUp(evt, curText) {
    if (curText != "") {
        if (curText.length > 3) {
            document.getElementById("ctl00_ContentPlaceHolder1_SearchRogue").value = true;
            document.getElementById("ctl00_ContentPlaceHolder1_LoadWI").click();
        }
    }
}

function openEditor(evt, editorName) {
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