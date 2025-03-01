var oXHR = window.XMLHttpRequest ? new XMLHttpRequest() : new ActiveXObject('Microsoft.XMLHTTP');
window.onload = RunBuilder();
var MenuBuilt = true

function RunBuilder(User) {
    var RolesMade = document.getElementById("Roles")
    
    if (oXHR != null) {
        if (RolesMade != null) {
            RolesMade = document.getElementById("Roles").value

            if (oXHR.readyState == 4 && MenuBuilt == true) {
                MenuBuilt = false
                getMenuList(this.responseXML);
            }
        }
    }
};

if (oXHR.readyState < 5) {
    oXHR.onreadystatechange = RunBuilder;
    oXHR.open("GET", "/DropDownMenuData.xml", true);
    oXHR.send();
}

function getMenuList(xml) {
    var OptionList = xml.getElementsByTagName('Option');
    var MenuOptions = new Array();
    var MenuButtons = new Array();
    var MenuNumbers = new Array();
    var SubsNumbers = new Array();

    var Count = 0;

    for (var i = 0; i < OptionList.length; i++) {
        var TempButtons = new Array();
        TempButtons.push(OptionList[i].getElementsByTagName("Department")[0].childNodes[0].nodeValue);
        TempButtons.push(OptionList[i].getElementsByTagName("ColumnTitle")[0].childNodes[0].nodeValue);
        TempButtons.push(OptionList[i].getElementsByTagName("Enable-Disable")[0].childNodes[0].nodeValue);
        TempButtons.push(OptionList[i].getElementsByTagName("AuthenticationGroup")[0].childNodes[0].nodeValue);
        TempButtons.push(OptionList[i].getElementsByTagName("WebsiteName")[0].childNodes[0].nodeValue);
        TempButtons.push(OptionList[i].getElementsByTagName("WebsiteRef")[0].childNodes[0].nodeValue);

        MenuOptions.push(TempButtons);
    }

    MenuOptions = MenuOptions.slice();
    MenuButtons = MenuButtons.slice();
    MenuNumbers = MenuNumbers.slice();
    SubsNumbers = SubsNumbers.slice();

    for (var i = 0; i < MenuOptions.length; i++) {
        if (i == 0) {
            MenuNumbers.push(i);
            SubsNumbers.push(i);
        }
        else {
            if (MenuOptions[i - 1][0] != MenuOptions[i][0]) {
                MenuNumbers.push(i);
            }
            if (MenuOptions[i - 1][1] != MenuOptions[i][1]) {
                SubsNumbers.push(i);
            }
        }
    }
    BuildMainMenu(MenuOptions, MenuNumbers, SubsNumbers);
};

function BuildMainMenu(MenuOptions, MenuNumbers, SubsNumbers) {
    var MainMenu
    var MenuCounter = 0;
    var SubsCounter = 0;

    BuildNavBar(MainMenu);

    for (i = 0; i < MenuOptions.length; i++) {
        if (MenuCounter < MenuNumbers.length) {
            if (MenuOptions[i][0] == MenuOptions[MenuNumbers[MenuCounter]][0]) {
                BuildNavButton(MainMenu, MenuOptions[MenuNumbers[MenuCounter]][0]);
                MenuCounter++;
            }
        }

        if (SubsCounter < SubsNumbers.length) {
            if (MenuOptions[i][1] == MenuOptions[SubsNumbers[SubsCounter]][1]) {
                BuildNavDropDown(MenuOptions,
                    MenuOptions[MenuNumbers[MenuCounter - 1]][0],
                    MenuOptions[SubsNumbers[SubsCounter]][1],
                    SubsNumbers[SubsCounter],
                    SubsNumbers[SubsCounter + 1]);
                SubsCounter++;
            }
        }
    }
};

function BuildNavBar(MainMenu) {
    var MenuPanel = document.getElementById('ctl00_CSSMainMenuPanel');
    MainMenu = document.createElement('div');
    MainMenu.id = "Main Menu NavBar";
    MainMenu.className = "MainMenuNavBar";
    MenuPanel.appendChild(MainMenu);
};

function BuildNavButton(MainMenu, MenuName) {
    var NavButtonCheck = document.getElementById(MenuName + 'Buttons');
    if (NavButtonCheck == null) {
        var ButtonContainer = document.createElement('div');
        ButtonContainer.onmouseenter = delayDropDown;
        ButtonContainer.onmouseleave = closeDropDown;
        ButtonContainer.onmouseout = checkButtonLeave;
        ButtonContainer.className = "MainMenuDropDown";

        var MenuButtons = document.createElement('button');
        MenuButtons.id = MenuName + " Buttons";
        MenuButtons.className = "MainMenuButton";
        MenuButtons.innerHTML = MenuName;

        var DropDown = document.createElement('div');
        DropDown.id = MenuName + " DropDown";
        DropDown.className = "MainMenuDropDownContent";

        var Row = document.createElement('div');
        Row.id = "Row" + MenuName;
        Row.className = "MainMenuRows";

        DropDown.appendChild(Row);
        ButtonContainer.appendChild(MenuButtons);
        ButtonContainer.appendChild(DropDown);

        var MainMenu = document.getElementById('Main Menu NavBar');
        MainMenu.appendChild(ButtonContainer);
    }
};

function BuildNavDropDown(MenuOptions, MenuName, SubsName, ThisSub, NextSub) {
    var CurrentRow = document.getElementById("Row" + MenuName);
    var ColumnCheck = document.getElementById("Column" + SubsName);

    if (ColumnCheck == null) {
        var Column = document.createElement("div");
        Column.id = "Column" + SubsName;
        Column.className = "MainMenuColumns";

        var Header = document.createElement('h3');
        Header.innerHTML = SubsName;
        Column.appendChild(Header);
    } else {
        Column = document.getElementById("Column" + SubsName);
    }

    if (NextSub == null) {
        NextSub = MenuOptions.length;
    }

    for (var i = ThisSub; i < NextSub; i++) {
        var webpage = document.createElement('a');
        var Roles = document.getElementById("Roles").value;
        var Check = Roles.search(MenuOptions[i][3]);

        if (MenuOptions[i][2] = "Enabled") {
            if (Check != -1) {
                webpage.innerHTML = MenuOptions[i][4];
                webpage.href = MenuOptions[i][5];

                Column.appendChild(webpage);
            }
            else {
                webpage.innerHTML = MenuOptions[i][4];
                webpage.style.color = 'gray';

                Column.appendChild(webpage);
            }
        }
    }
    if (ColumnCheck == null) {
        CurrentRow.appendChild(Column);
    }
};

var closeDD;
var highLightButton;

function delayDropDown(e) {
    var buttonClass = document.getElementsByClassName('MainMenuButton');
    for (i = 0; i < buttonClass.length; i++) {
        buttonClass[i].style.color = 'black';
        buttonClass[i].style.backgroundColor = '#80befd';

        if (e.target.childNodes[0].id.split(" Buttons")[0] == buttonClass[i].id.split(" Buttons")[0]) {
            buttonClass[i].style.color = 'white';
            buttonClass[i].style.backgroundColor = '#0057bf';
        }
    }

    closeDD = true;
    setTimeout(function () {
        if (closeDD == true) {
            highLightButton = true;
            var dropDownClass = document.getElementsByClassName('MainMenuDropDownContent');
            
            for (i = 0; i < dropDownClass.length; i++) {
                dropDownClass[i].style.display = 'none';

                if (e.target.childNodes[0].id.split(" Buttons")[0] == dropDownClass[i].id.split(" DropDown")[0]) {
                    dropDownClass[i].style.display = 'block';
                }
            }

        }
    }, 250);
};

function checkButtonLeave() {
    closeDD = false;

    if (highLightButton == false) {
        var buttonClass = document.getElementsByClassName('MainMenuButton');
        for (i = 0; i < buttonClass.length; i++) {
            buttonClass[i].style.color = 'black';
            buttonClass[i].style.backgroundColor = '#80befd';
        }   
    }
};

function closeDropDown() {
    var dropDownClass = document.getElementsByClassName('MainMenuDropDownContent');
    var buttonClass = document.getElementsByClassName('MainMenuButton');

    for (i = 0; i < dropDownClass.length; i++) {
        dropDownClass[i].style.display = 'none';
    }
    
    for (i = 0; i < buttonClass.length; i++) {
        buttonClass[i].style.color = 'black';
        buttonClass[i].style.backgroundColor = '#80befd';
    }  

    closeDD = false;
    highLightButton = false;
};