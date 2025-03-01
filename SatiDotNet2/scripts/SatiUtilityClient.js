var DelayTimer = 500;

function satiUtilityDisableButtonClick(btnName) {
    document.getElementById(btnName.id).disabled = true;
}

function satiUtilityEnableButtonClick(btnName) {
    setTimeout(function () {
        document.getElementById(btnName.id).disabled = false;
    }, DelayTimer);
}